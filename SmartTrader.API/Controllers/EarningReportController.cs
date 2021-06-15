using System;
using Microsoft.AspNetCore.Mvc;
using SmartTrader.Core.Inerfaces;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EarningReportController : ControllerBase
    {
        private ISmartTraderContext _context;
        private IEarningReportRepository _repository;

        public EarningReportController(ISmartTraderContext context, IEarningReportRepository repository)
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

        [HttpGet("GetByCompanyName/{companyName}/{year}")]
        public IActionResult GetByCompanyName(string companyName,int year)
        {
            var result = _repository.GetByCompanyName(companyName, year);
            return Ok(result);
        }


        [HttpGet("GetUpcommingReports")]
        public IActionResult GetUpcommingReports()
        {
            var result = _repository.GetAll();
            return Ok(result);
        }


    }
}
