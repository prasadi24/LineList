using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IXrayService
    {
        Task<IEnumerable<Xray>> GetAll();

        Task<Xray> GetById(Guid id);

        Task<Xray> Add(Xray xray);

        Task<Xray> Update(Xray xray);

        Task<bool> Remove(Xray xray);

        Task<IEnumerable<Xray>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}