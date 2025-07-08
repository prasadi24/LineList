using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class WelcomeMessageService : IWelcomeMessageService
    {
        private readonly IWelcomeMessageRepository _welcomeMessageRepository;

        public WelcomeMessageService(IWelcomeMessageRepository welcomeMessageRepository)
        {
            _welcomeMessageRepository = welcomeMessageRepository;
        }

        public async Task<IEnumerable<WelcomeMessage>> GetAll()
        {
            return await _welcomeMessageRepository.GetAll();
        }

        public async Task<WelcomeMessage> GetById(Guid id)
        {
            return await _welcomeMessageRepository.GetById(id);
        }

        public async Task<WelcomeMessage> Add(WelcomeMessage welcomeMessage)
        {
            if (_welcomeMessageRepository.Search(c => c.Description == welcomeMessage.Description).Result.Any())
                return null;

            await _welcomeMessageRepository.Add(welcomeMessage);
            return welcomeMessage;
        }

        public async Task<WelcomeMessage> Update(WelcomeMessage welcomeMessage)
        {
            if (_welcomeMessageRepository.Search(c => c.Description == welcomeMessage.Description && c.Id != welcomeMessage.Id).Result.Any())
                return null;

            await _welcomeMessageRepository.Update(welcomeMessage);
            return welcomeMessage;
        }

        //public async Task UpdateWelcomeMessageAsync(string description, string notes, string message2, string message3)
        //{
        //    // Retrieve the current welcome message from the repository
        //    var welcomeMessages = await _welcomeMessageRepository.GetAll();
        //    var welcomeMessage = welcomeMessages.FirstOrDefault();
        //    // If the welcome message does not exist, return or throw an exception
        //    if (welcomeMessage == null)
        //    {
        //        throw new Exception("Welcome message not found");
        //    }

        //    // Update the values
        //    welcomeMessage.Description = description;
        //    welcomeMessage.Notes = notes;
        //    welcomeMessage.Message2 = message2;
        //    welcomeMessage.Message3 = message3;

        //    // Save the updated welcome message in the repository
        //    await _welcomeMessageRepository.UpdateWelcomeMessageAsync(welcomeMessage);
        //}

        public async Task<bool> Remove(WelcomeMessage welcomeMessage)
        {
            await _welcomeMessageRepository.Remove(welcomeMessage);
            return true;
        }

        public void Dispose()
        {
            _welcomeMessageRepository?.Dispose();
        }
    }
}