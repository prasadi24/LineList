using LineList.Cenovus.Com.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ISubCategoryRepository : IRepository<SubCategory>
    {
        new Task<List<SubCategory>> GetAll();
        new Task<SubCategory> GetById(int id);
        Task<List<SubCategory>> SearchSubCategory(string searchedValue);

    }
}
