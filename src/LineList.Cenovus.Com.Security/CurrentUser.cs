using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace LineList.Cenovus.Com.Security
{
    [Serializable]
    public class CurrentUser
    {
        public string Username { get; internal set; }
        public string FullName { get; internal set; }
        public string Email { get; internal set; }

        public bool IsReadOnly { get; internal set; }
        public bool IsCenovusAdmin { get; internal set; }
        public bool IsEpAdmin { get; internal set; }
        public bool IsEpUser { get; internal set; }

        public bool IsIODFieldCLUser { get; internal set; }
        public bool IsIODFieldCLAdmUser { get; internal set; }
        public bool IsIODFieldFCUser { get; internal set; }
        public bool IsIODFieldFCAdmUser { get; internal set; }

        public Guid[] EpUser { get; internal set; }
        public Guid[] EpAdmin { get; internal set; }
        public Guid[] EppLeadEng { get; internal set; }
        public Guid[] EppDataEnt { get; internal set; }
        public Guid[] EppRsv { get; internal set; }

        public Guid? EpCompanyId { get; internal set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IUserPreferenceService _userPreferenceService;
        private readonly IEpCompanyService _epCompanyService;
        private readonly IEpProjectUserRoleService _epProjectUserRoleService;
        private readonly UserManager _userManager;

        public CurrentUser(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IUserPreferenceService userPreferenceService,
            IEpCompanyService epCompanyService,
            IEpProjectUserRoleService epProjectUserRoleService,
            UserManager userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _userPreferenceService = userPreferenceService;
            _epCompanyService = epCompanyService;
            _epProjectUserRoleService = epProjectUserRoleService;
            _userManager = userManager;

            var user = _httpContextAccessor.HttpContext?.User;

            // 🚨 Local development bypass
            if (_configuration["Environment"] == "DEV-BYPASS")
            {
                var dummyCompanyId = Guid.NewGuid();

                this.Username = "devuser";
                this.FullName = "DEV\\Dev User";
                this.Email = "devuser@example.com";
                this.IsReadOnly = false;
                this.IsCenovusAdmin = true;
                this.IsEpAdmin = true;
                this.IsEpUser = true;
                this.IsIODFieldCLUser = true;
                this.IsIODFieldCLAdmUser = false;
                this.IsIODFieldFCUser = false;
                this.IsIODFieldFCAdmUser = false;

                this.EpAdmin = new[] { dummyCompanyId };
                this.EpUser = new[] { dummyCompanyId };
                this.EpCompanyId = dummyCompanyId;

                this.EppLeadEng = Array.Empty<Guid>();
                this.EppDataEnt = Array.Empty<Guid>();
                this.EppRsv = Array.Empty<Guid>();

                return; // Skip DB and AD
            }

            // 🧾 Normal flow for production or integrated environments
            this.Username = user?.Identity?.Name ?? "Unknown User";
            this.IsReadOnly = user?.IsInRole(_configuration.GetSection(ConfigKey.ReadOnly).Value) ?? false;
            this.IsCenovusAdmin = user?.IsInRole(_configuration.GetSection(ConfigKey.CenovusAdmin).Value) ?? false;
            this.IsIODFieldCLUser = user?.IsInRole("LL_PRD_IODFIELD_CL") ?? false;
            this.IsIODFieldFCUser = user?.IsInRole("LL_PRD_IODFIELD_FC") ?? false;
            this.IsIODFieldCLAdmUser = user?.IsInRole("LL_PRD_IODFIELD_CL_ADM") ?? false;
            this.IsIODFieldFCAdmUser = user?.IsInRole("LL_PRD_IODFIELD_FC_ADM") ?? false;

            var userPref = _userManager.GetUserPreference(this.Username);
            this.FullName = userPref?.Result.FullName ?? "No Name";
            this.Email = userPref?.Result.Email ?? "No Email";

            SetEpUserAndAdminRoles(user);
            SetProjectRoles(user);
        }

        private void SetEpUserAndAdminRoles(ClaimsPrincipal user)
        {
            List<Guid> userList = new List<Guid>();
            List<Guid> adminList = new List<Guid>();

            var epCompanies = _epCompanyService.GetAll().Result
                .Where(m => !string.IsNullOrEmpty(m.ActiveDirectoryGroup) && m.IsActive);

            foreach (var company in epCompanies)
            {
                string roleNameAdmin = _configuration.GetSection(ConfigKey.EpAdmin).Value.Replace("{0}", company.ActiveDirectoryGroup);
                string roleNameUser = _configuration.GetSection(ConfigKey.EpUser).Value.Replace("{0}", company.ActiveDirectoryGroup);

                if (user.IsInRole(roleNameAdmin))
                {
                    adminList.Add(company.Id);
                    userList.Add(company.Id);
                }
                else if (user.IsInRole(roleNameUser))
                {
                    userList.Add(company.Id);
                }
            }

            this.EpAdmin = adminList.ToArray();
            this.EpUser = userList.ToArray();
            this.IsEpAdmin = this.EpAdmin.Length > 0;
            this.IsEpUser = this.EpUser.Length > 0;
            this.EpCompanyId = this.EpUser.Length > 0 ? this.EpUser.First() : (Guid?)null;
        }

        private void SetProjectRoles(ClaimsPrincipal user)
        {
            List<Guid> eng = new List<Guid>();
            List<Guid> ent = new List<Guid>();
            List<Guid> rsv = new List<Guid>();

            string engName = _configuration.GetSection(ConfigKey.RoleEppLeadEng).Value;
            string entName = _configuration.GetSection(ConfigKey.RoleEppDataEnt).Value;
            string rsvName = _configuration.GetSection(ConfigKey.RoleEppRsv).Value;

            var userName = this.FullName.Contains('\\') ? this.FullName.Split('\\').Last() : this.FullName;
            var epProjectRoles = _epProjectUserRoleService.GetAll().Result
                .Where(m => m.UserName == this.FullName || m.UserName == userName);

            foreach (var role in epProjectRoles)
            {
                if (role.EpProjectRole.Name == engName) eng.Add(role.EpProjectId);
                else if (role.EpProjectRole.Name == entName) ent.Add(role.EpProjectId);
                else if (role.EpProjectRole.Name == rsvName) rsv.Add(role.EpProjectId);
            }

            this.EppDataEnt = ent.ToArray();
            this.EppLeadEng = eng.ToArray();
            this.EppRsv = rsv.ToArray();
        }
    }
}
