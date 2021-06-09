using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTrader.Core.Inerfaces;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectorController : ControllerBase
    {
        private ISmartTraderContext _context;
        private ISectorViewRepository _repository;

        public SectorController(ISmartTraderContext context, ISectorViewRepository repository)
        {

            _context = context;
            _repository = repository;

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _repository.GetAll(); 
            return Ok(result);
        }

        [HttpGet("{date:DateTime}")]
        public IActionResult GetByDate(DateTime date)
        {
            var result = _repository.GetByDate(date);
            return Ok(result);
        }

        [HttpGet("GetBySectorName/{sectorName}")]
        public IActionResult GetBySectorName(string sectorName)
        {
            var result = _repository.GetBySectorName(sectorName);
            return Ok(result);
        }


        [HttpPut("{sector}")]
        public void Put(string sector, [FromBody] string newSector)
        {
            //_repository.Update();
        }

    }
}
