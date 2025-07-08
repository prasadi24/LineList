using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class LineDesignationTableViewHeaderRepository : Repository<LineDesignationTableViewHeader>, ILineDesignationTableViewHeaderRepository
{
    public LineDesignationTableViewHeaderRepository(LineListDbContext context) : base(context)
    {
    }
}