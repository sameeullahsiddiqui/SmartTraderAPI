using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartTrader.Core.Inerfaces;
using SmartTrader.Core.Models;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperstarPortfolioController : ControllerBase
    {
        private readonly ISmartTraderContext _context;
        private readonly ISuperstarPortfolioRepository _repository;
        private readonly IStockPriceRepository _stockPriceRepository;

        public SuperstarPortfolioController(ISmartTraderContext context, ISuperstarPortfolioRepository repository, IStockPriceRepository stockPriceRepository)
        {

            _context = context;
            _repository = repository;
            _stockPriceRepository = stockPriceRepository;

        }

        [HttpGet("{activeOnly}")]
        public IActionResult GetAll(bool activeOnly)
        {
            var result = _repository.GetAll(activeOnly);
            return Ok(result);
        }

        [HttpPost()]
        public IActionResult Add(SuperstarPortfolio model)
        {
            model.Date = DateTime.Now;

            _repository.Add(model);
            _repository.SaveChanges();

            return Ok();
        }


        [HttpPut("{SuperstarPortfolioId}")]
        public IActionResult Put(int SuperstarPortfolioId, SuperstarPortfolio model)
        {
            var existingSuperstarPortfolio = _repository.Get(model.SuperstarPortfolioId);

            if (existingSuperstarPortfolio != null && SuperstarPortfolioId == existingSuperstarPortfolio.SuperstarPortfolioId)
            {
                existingSuperstarPortfolio.InvestorName = model.InvestorName;
                existingSuperstarPortfolio.Price = model.Price;
                existingSuperstarPortfolio.ReasonToWatch = model.ReasonToWatch;
                existingSuperstarPortfolio.Status = model.Status;
                existingSuperstarPortfolio.UpdateTime = DateTime.Now;

                _repository.SaveChanges();

            }
            else
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("Sync")]
        public IActionResult SyncPrice()
        {
            var openSuperstarPortfolios = _repository.Find(x => x.Status == "Open");

            var codes = openSuperstarPortfolios.Select(x => x.Symbol).Distinct().ToList<string>();

            var lastDate = _context.StockPrices.Max(t => t.Date);

            var latestPrices = _stockPriceRepository.Find(x => codes.Contains(x.SymbolName) && x.Date == lastDate).ToList();

            foreach (var item in openSuperstarPortfolios)
            {
                var current = latestPrices.FirstOrDefault(x => x.SymbolName == item.Symbol);
                if (current != null)
                {
                    item.CurrentPrice = (double)current.Close;
                    if(item.Price == 0)
                        item.Price = item.CurrentPrice;
                    item.UpdateTime = lastDate;
                    item.ChangeSinceAdded = ((item.CurrentPrice - item.Price) / item.Price * 100);
                    item.Days = (int)(lastDate - item.Date).TotalDays;
                }

            }
            _repository.SaveChanges();

            return Ok();
        }

        [HttpDelete("{SuperstarPortfolioId}")]
        public IActionResult Delete(int SuperstarPortfolioId)
        {
            if (SuperstarPortfolioId <= 0)
                return BadRequest("Not a valid watch list id");

            var existingSuperstarPortfolio = _repository.Get(SuperstarPortfolioId);

            _repository.Remove(existingSuperstarPortfolio);
            _repository.SaveChanges();
            return Ok();
        }

    }

}

