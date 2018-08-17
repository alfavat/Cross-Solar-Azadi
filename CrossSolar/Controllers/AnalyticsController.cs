using CrossSolar.Domain;
using CrossSolar.Extensions;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace CrossSolar.Controllers
{
    [Route("panel")]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        private readonly IPanelRepository _panelRepository;
        private readonly IDayAnalyticsRepository _dayAnaliticsRepositpry;
        public AnalyticsController(IAnalyticsRepository analyticsRepository, IPanelRepository panelRepository, IDayAnalyticsRepository dayAnalyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
            _panelRepository = panelRepository;
            _dayAnaliticsRepositpry = dayAnalyticsRepository;
        }
        // GET panel/XXXX1111YYYY2222/analytics
        [HttpGet("{panelId}/[controller]")]
        public async Task<IActionResult> Get([FromRoute] string panelId)
        {
            try
            {
                var panel = await _panelRepository.Query()
                    .FirstOrDefaultAsync(x => x.Serial.Equals(panelId, StringComparison.CurrentCultureIgnoreCase));

                if (panel == null) return NotFound();

                var analytics = await _analyticsRepository.Query()
                    .Where(x => x.PanelId.Equals(panelId, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();

                var result = new OneHourElectricityListModel
                {
                    OneHourElectricitys = analytics.Select(c => new OneHourElectricityModel
                    {
                        Id = c.Id,
                        KiloWatt = c.KiloWatt,
                        DateTime = c.DateTime
                    })
                };

                return Ok(result);
            }
            catch (Exception ec) { ec.LogException(); return Content("-100"); }
        }

        // GET panel/XXXX1111YYYY2222/analytics/day
        [HttpGet("{panelId}/[controller]/day")]
        public async Task<IActionResult> DayResults([FromRoute] string panelId)
        {
            var data = _dayAnaliticsRepositpry.GetPanelData(panelId);
            return Ok(data);
        }

        // POST panel/XXXX1111YYYY2222/analytics
        [HttpPost("{panelId}/[controller]")]
        public async Task<IActionResult> Post([FromRoute] string panelId, [FromBody] OneHourElectricityModel value)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (panelId.IsNullOrEmpty())
                    return Content("-1");

                var oneHourElectricityContent = new OneHourElectricity
                {
                    PanelId = panelId,
                    KiloWatt = value.KiloWatt,
                    DateTime = DateTime.UtcNow
                };
                await _analyticsRepository.InsertAsync(oneHourElectricityContent);
                var result = new OneHourElectricityModel
                {
                    Id = oneHourElectricityContent.Id,
                    KiloWatt = oneHourElectricityContent.KiloWatt,
                    DateTime = oneHourElectricityContent.DateTime
                };
                return Created($"panel/{panelId}/analytics/{result.Id}", result);
            }
            catch (Exception ec)
            {
                ec.LogException();
                return Content("-100");
            }
        }       
    }
}