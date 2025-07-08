using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IWelcomeMessageRepository : IRepository<WelcomeMessage>
    {
        new Task<List<WelcomeMessage>> GetAll();

        new Task<WelcomeMessage> GetById(Guid id);

        //Task UpdateWelcomeMessageAsync(WelcomeMessage welcomeMessage);
    }
}