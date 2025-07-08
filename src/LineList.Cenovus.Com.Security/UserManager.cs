using Azure.Identity;
using LineList.Cenovus.Com.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models.ODataErrors;
using System.Collections.Concurrent;

namespace LineList.Cenovus.Com.Security
{
    public class UserManager
    {
        private readonly IConfiguration _configuration;
        private readonly GraphServiceClient _graphClient;

        public UserManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _graphClient = GetGraphServiceClient(configuration);
        }

        // Initialize Microsoft Graph API Client
        private GraphServiceClient GetGraphServiceClient(IConfiguration configuration)
        {
            var clientId = configuration["AzureAd:ClientId"];
            var tenantId = configuration["AzureAd:TenantId"];
            var clientSecret = configuration["AzureAd:ClientSecret"];

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var graphClient = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });

            return graphClient;
        }

        public async Task<UserPreference> GetUserPreference(string userPrincipalName)
        {
            if (string.IsNullOrWhiteSpace(userPrincipalName)) return null;

            var targetDate = DateTime.Now.AddDays(-7);

            // Fetch user details from Azure AD
            var userPref = await UpdateUserCache(userPrincipalName);

            // Return cached user if found within last 7 days
            if (userPref != null && userPref.ModifiedOn > targetDate && !string.IsNullOrWhiteSpace(userPref.Email))
            {
                return userPref;
            }

            return userPref;
        }

        // Update user cache by fetching details from Azure AD
        public async Task<UserPreference> UpdateUserCache(string userPrincipalName)
        {
            var userPref = new UserPreference { UserName = userPrincipalName };

            userPref.FullName = await GetFullNameFromAzureAd(userPrincipalName);
            userPref.Email = await GetEmailFromAzureAd(userPrincipalName);
            userPref.ModifiedOn = DateTime.Now;

            return userPref;
        }

        // Determines if local accounts are used
        public bool IsLocalAccounts()
        {
            return string.Equals(_configuration.GetValue<string>(ConfigKey.LocalAccounts), "True", StringComparison.OrdinalIgnoreCase);
        }

        // Default email if none is found
        public string DefaultEmail()
        {
            return _configuration.GetValue<string>(ConfigKey.DefaultEmail) ?? "line.list@cenovus.com";
        }

        // Get Full Name from Azure AD
        public async Task<string> GetFullNameFromAzureAd(string userPrincipalName)
        {
            try
            {
                var user = await _graphClient.Users[userPrincipalName].GetAsync();
                return user?.DisplayName ?? userPrincipalName;
            }
            catch (ODataError ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
                return userPrincipalName;
            }
        }

        // Get Email from Azure AD
        public async Task<string> GetEmailFromAzureAd(string userPrincipalName)
        {
            try
            {
                var user = await _graphClient.Users[userPrincipalName].GetAsync();
                return !string.IsNullOrWhiteSpace(user?.Mail) ? user.Mail : DefaultEmail();
            }
            catch (ODataError ex)
            {
                Console.WriteLine($"Error fetching email: {ex.Message}");
                return DefaultEmail();
            }
        }

        // Get Members of an Azure AD Group
        public async Task<List<string>> GetGroupMembersFromAzureAd(string groupId)
        {
            var members = new List<string>();

            try
            {
                var groupMembers = await _graphClient.Groups[groupId].Members.GetAsync();

                if (groupMembers?.Value != null)
                {
                    foreach (var member in groupMembers.Value)
                    {
                        if (member is Microsoft.Graph.Models.User user)
                        {
                            members.Add(user.UserPrincipalName);
                        }
                    }
                }
            }
            catch (ODataError ex)
            {
                Console.WriteLine($"Error fetching group members: {ex.Message}");
            }

            return members;
        }

        // Check if an Azure AD Group Exists
        public async Task<bool> GroupExistsInAzureAd(string groupId)
        {
            try
            {
                var group = await _graphClient.Groups[groupId].GetAsync();
                return group != null;
            }
            catch (ODataError)
            {
                return false; // Return false if group does not exist
            }
        }

        // Get all users from Azure AD
        public async Task<string[]> GetAllUsersFromAzureAd()
        {
            var usersList = new HashSet<string>();

            try
            {
                var users = await _graphClient.Users.GetAsync();

                if (users?.Value != null)
                {
                    usersList.UnionWith(users.Value.Select(u => u.UserPrincipalName));
                }
            }
            catch (ODataError ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
            }

            return usersList.Select(RemoveDomain).OrderBy(u => u).ToArray();
        }

        // Get Users for a Specific EP (Enterprise Project)
        public async Task<List<KeyValuePair<string, string>>> GetUsersForEpAsync(string groupId, bool includeAdmins)
        {
            var list = new ConcurrentBag<KeyValuePair<string, string>>();
            var isLocal = IsLocalAccounts();

            var userNames = new List<string>();
            userNames.AddRange(await GetGroupMembersFromAzureAd(groupId));

            if (includeAdmins)
                userNames.AddRange(await GetGroupMembersFromAzureAd(groupId)); // Modify this to fetch admin groups separately

            foreach (var userName in userNames.Distinct())
            {
                string cleanUserName = RemoveDomain(userName);
                list.Add(new KeyValuePair<string, string>(cleanUserName, await GetFullNameFromAzureAd(cleanUserName)));
            }

            return list.OrderBy(m => m.Value).ToList();
        }

        // Remove domain from username (e.g., domain\user → user)
        public static string RemoveDomain(string userName)
        {
            return userName.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? userName;
        }
    }
}