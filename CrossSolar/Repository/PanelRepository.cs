using CrossSolar.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossSolar.Repository
{
    public class PanelRepository : GenericRepository<Panel>, IPanelRepository
    {
        public PanelRepository(CrossSolarDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}