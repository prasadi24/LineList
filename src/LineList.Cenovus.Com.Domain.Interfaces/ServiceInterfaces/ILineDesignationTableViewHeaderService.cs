using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ILineDesignationTableViewHeaderService
    {
        Task<IEnumerable<LineDesignationTableViewHeader>> GetAll();

        Task<LineDesignationTableViewHeader> GetById(Guid id);

        Task<LineDesignationTableViewHeader> Add(LineDesignationTableViewHeader entity);

        Task<LineDesignationTableViewHeader> Update(LineDesignationTableViewHeader entity);

        Task<bool> Remove(LineDesignationTableViewHeader entity);

        Task<IEnumerable<LineDesignationTableViewHeader>> Search(string searchCriteria);
    }
}