using Microsoft.EntityFrameworkCore;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class SubCategoryRepository : Repository<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(LineListDbContext context) : base(context) { }

        public override async Task<List<SubCategory>> GetAll()
        {
            return await Db.SubCategories.AsNoTracking()
                .Include(b => b.Category)
                .OrderBy(b => b.CategoryId)
                .ToListAsync();
        }

        public override async Task<SubCategory> GetById(int id)
        {
            return await Db.SubCategories.AsNoTracking()
                .Include(b => b.Category)
                .Where(b => b.ID == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<SubCategory>> SearchSubCategory(string searchedValue)
        {
            return await Db.SubCategories.AsNoTracking()
                .Include(b => b.Category)
				.Where(b => b.Name.Contains(searchedValue) || b.Category.Name.Contains(searchedValue))
				.ToListAsync();
        }

    }
}
