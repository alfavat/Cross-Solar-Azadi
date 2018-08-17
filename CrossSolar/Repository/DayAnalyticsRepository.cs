using CrossSolar.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CrossSolar.Repository
{
    public class DayAnalyticsRepository : GenericRepository<OneDayElectricityModel>, IDayAnalyticsRepository
    {
        public DayAnalyticsRepository(CrossSolarDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<OneDayElectricityModel> GetPanelData(string panelId)
        {
            var data = _dbContext.OneHourElectricitys.AsNoTracking().AsQueryable()
                .Where(d=>d.PanelId==panelId)
                .GroupBy(f => f.DateTime.ToShortDateString())
                .Select(f => new OneDayElectricityModel
                {
                    Average = f.Average(g => g.KiloWatt),
                    Maximum = f.Max(g => g.KiloWatt),
                    Minimum = f.Min(g => g.KiloWatt),
                    Sum = f.Sum(g => g.KiloWatt),
                    DateTime = f.FirstOrDefault().DateTime.ToShortDateString()
                }).ToList();            
            return data;
        }
    }
}