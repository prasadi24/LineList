using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class WelcomeMessageRepository : Repository<WelcomeMessage>, IWelcomeMessageRepository
    {
        private readonly LineListDbContext _context;

        public WelcomeMessageRepository(LineListDbContext context) : base(context)
        {
        }
        //public async Task UpdateWelcomeMessageAsync(WelcomeMessage welcomeMessage)
        //{
        //    // Ensure that the entity exists before trying to update it
        //    var existingMessage = await _context.WelcomeMessages
        //        .FirstOrDefaultAsync(w => w.Id == welcomeMessage.Id); // Assuming there is an Id field

        //    if (existingMessage != null)
        //    {
        //        // Update the properties
        //        existingMessage.Description = welcomeMessage.Description;
        //        existingMessage.Notes = welcomeMessage.Notes;
        //        existingMessage.Message2 = welcomeMessage.Message2;
        //        existingMessage.Message3 = welcomeMessage.Message3;

        //        // Save the changes to the database
        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        throw new Exception("Welcome message not found");
        //    }
        //}


    }
}