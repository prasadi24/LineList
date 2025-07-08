using LineList.Cenovus.Com.Security;
using LineList.Cenovus.Com.UI.New.Models;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.ViewComponents
{
    /// <summary>
    /// Renders the navigation bar taking into account security and roles
    /// </summary>
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IConfiguration _roleConfig;
        private readonly CurrentUser _currentUser;

        public NavigationViewComponent(IConfiguration configuration, CurrentUser currentUser)
        {
            _roleConfig = configuration.GetSection("SiteSettings");
            _currentUser = currentUser;
        }

        /// <summary>
        /// Checks the users roles and sets which menus they can view
        /// </summary>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["IsReadOnly"] = _currentUser.IsReadOnly;
            ViewData["IsCenovusAdmin"] = _currentUser.IsCenovusAdmin;
            ViewData["IsEpAdmin"] = _currentUser.IsEpAdmin;
            ViewData["EppLeadEng"] = _currentUser.EppLeadEng.ToList();
            ViewData["EppDataEnt"] = _currentUser.EppDataEnt.ToList();
            ViewData["EppRsv"] = _currentUser.EppRsv.ToList();


            var model = new NavigationModel();
            var roleGroup = _roleConfig["RoleGroup"];

            model.IsCenovusAdminVisible =  _currentUser.IsCenovusAdmin;
            model.IsReadOnlyVisible = _currentUser.IsReadOnly && !_currentUser.IsEpUser && !_currentUser.IsEpAdmin && !_currentUser.IsCenovusAdmin;
            model.IsEpProjectVisible = !model.IsReadOnlyVisible;
            model.IsLDTImportVisible = _currentUser.IsCenovusAdmin;
            return View(model);
        }
    }
}