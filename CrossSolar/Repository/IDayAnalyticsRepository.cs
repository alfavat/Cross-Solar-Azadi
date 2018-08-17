using CrossSolar.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrossSolar.Repository
{
    public interface IDayAnalyticsRepository : IGenericRepository<OneDayElectricityModel>
    {
        List<OneDayElectricityModel> GetPanelData(string panelId);
    }
}