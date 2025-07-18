﻿using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class LineDesignationTableViewRevisionRepository : Repository<LineDesignationTableViewRevision>, ILineDesignationTableViewRevisionRepository
{
    public LineDesignationTableViewRevisionRepository(LineListDbContext context) : base(context)
    {
    }
}