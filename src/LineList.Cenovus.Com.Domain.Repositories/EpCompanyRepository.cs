using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class EpCompanyRepository : Repository<EpCompany>, IEpCompanyRepository
    {
        private readonly LineListDbContext _context;

        public EpCompanyRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.EpCompanyAlphas.Any(m => m.EpCompanyId == id)
                || _context.EpProjects.Any(m => m.EpCompanyId == id);
        }

        public async Task<bool> UpdateCompanyLogo(Guid companyId, string base64String)
        {
            var company = await _context.EpCompanies.FindAsync(companyId);
            if (company == null) return false;

            try
            {
                // Remove "data:image/png;base64," if present
                if (base64String.Contains(","))
                {
                    base64String = base64String.Split(',')[1];
                }

                // Convert to byte[]
                company.Logo = Convert.FromBase64String(base64String);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting Base64 string: {ex.Message}");
                return false; // Return false if conversion fails
            }
            await Update(company);

            return true;
        }
    }
}