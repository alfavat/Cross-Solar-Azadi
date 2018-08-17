﻿using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CrossSolar.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PanelController : Controller
    {
        private readonly IPanelRepository _panelRepository;

        public PanelController(IPanelRepository panelRepository)
        {
            _panelRepository = panelRepository;
        }

        // POST api/panel
        public async Task<IActionResult> Register([FromBody] PanelModel value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var panel = new Panel
            {
                Latitude = value.Latitude,
                Longitude = value.Longitude,
                Serial = value.Serial,
                Brand = value.Brand
            };

            await _panelRepository.InsertAsync(panel);

            return Created($"panel/{panel.Id}", panel);
        }
    }
}