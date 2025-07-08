using LineList.Cenovus.Com.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ISubCategoryService : IDisposable
    {
        Task<IEnumerable<SubCategory>> GetAll();
        Task<SubCategory> GetById(int id);
        Task<SubCategory> Add(SubCategory category);
        Task<SubCategory> Update(SubCategory category);
        Task<bool> Remove(SubCategory category);
        Task<IEnumerable<SubCategory>> Search(string categoryName);
        Task<IEnumerable<SubCategory>> SearchSubCategory(string categoryName);
        
    }
}
