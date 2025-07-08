using LineList.Cenovus.Com.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        new Task<List<Category>> GetAll();
        new Task<Category> GetById(int id);      
    }
}
