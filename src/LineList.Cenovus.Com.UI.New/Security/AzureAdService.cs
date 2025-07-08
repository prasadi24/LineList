using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;


namespace LineList.Cenovus.Com.UI.Security
{
    /// <summary>
    /// The Azure AD Service is used to configure and access the Entra directory using MS Graph
    /// </summary>
    public class AzureAdService
    {
        private readonly string _clientId;
        private readonly string _tenantId;
        private readonly string _clientSecret;

        /// <summary>
        /// Fetch the configuration values to access Azure
        /// </summary>
        /// <param name="configuration"></param>
        public AzureAdService(IConfiguration configuration)
        {
            var azureAdConfig = configuration.GetSection("AzureAd");
            _clientId = azureAdConfig["ClientId"];
            _tenantId = azureAdConfig["TenantId"];
            _clientSecret = azureAdConfig["ClientSecret"];
        }

        /// <summary>
        /// Create the Graph Service client and provide it the credentials 
        /// </summary>
        private GraphServiceClient GetGraphClient()
        {
            //set creds
            var clientSecretCredential = new ClientSecretCredential(
                _tenantId,
                _clientId,
                _clientSecret);

            //create client
            var graphServiceClient = new GraphServiceClient(clientSecretCredential);
            return graphServiceClient;
        }

        /// <summary>
        /// Get the users list from azure ad gourp
        /// </summary>
        /// <param name="adGroupName">Azure Ad group Name</param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetUsersFromGroup(string adGroupName)
        {
            //adGroupName = "APP-CLO-LL-ABC-EP-TQA";
            var users = new Dictionary<string, string>();
            var graphClient = GetGraphClient();
            Group? matchedGroup = null;
            try
            {
                var groupsPage = await graphClient.Groups.GetAsync();

                var groupIterator = PageIterator<Group, GroupCollectionResponse>.CreatePageIterator(
                     graphClient,
                     groupsPage,
                     group =>
                     {
                         if (!string.IsNullOrWhiteSpace(group.DisplayName))
                         {
                             var normalized = group.DisplayName.Trim().ToLowerInvariant();
                             var target = adGroupName.Trim().ToLowerInvariant();

                             if (normalized == target)
                             {
                                 matchedGroup = group;
                                 return false; // stop
                             }
                         }
                         return true;
                     });

                await groupIterator.IterateAsync();

                if (matchedGroup == null)
                {
                    Console.WriteLine("Group not found.");
                    //return users;

                }

                Console.WriteLine($"\nGroup found: {matchedGroup.DisplayName} (ID: {matchedGroup.Id})\n");
                var membersPage = await graphClient.Groups[matchedGroup.Id].Members.GetAsync();

                var memberIterator = PageIterator<DirectoryObject, DirectoryObjectCollectionResponse>.CreatePageIterator(
                    graphClient,
                    membersPage,
                    member =>
                    {
                        if (member is User user)
                        {
                            string userDisplay = user.DisplayName + " <" + user.Mail + ">";
                            users.TryAdd(user.DisplayName, userDisplay);
                            Console.WriteLine($"- {user.DisplayName} ({user.Mail ?? "no email"})");
                        }
                        return true;
                    });

                await memberIterator.IterateAsync();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return users;
        }
    }
}

