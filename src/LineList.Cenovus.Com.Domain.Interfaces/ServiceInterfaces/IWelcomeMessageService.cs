using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IWelcomeMessageService
    {
        Task<IEnumerable<WelcomeMessage>> GetAll();

        Task<WelcomeMessage> GetById(Guid id);

        Task<WelcomeMessage> Add(WelcomeMessage welcomeMessage);

        Task<WelcomeMessage> Update(WelcomeMessage welcomeMessage);
        //Task UpdateWelcomeMessageAsync(string description, string notes, string message1, string message2);

        Task<bool> Remove(WelcomeMessage welcomeMessage);
    }
}