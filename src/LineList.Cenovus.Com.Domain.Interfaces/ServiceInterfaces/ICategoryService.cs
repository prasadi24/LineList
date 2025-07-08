using LineList.Cenovus.Com.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ICategoryService : IDisposable
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<Category> Add(Category category);
        Task<Category> Update(Category category);
        Task<bool> Remove(Category category);
        Task<IEnumerable<Category>> Search(string categoryName);
    }
}
