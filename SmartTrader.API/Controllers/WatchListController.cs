using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartTrader.Core.Inerfaces;
using SmartTrader.Core.Models;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private readonly ISmartTraderContext _context;
        private readonly IWatchListRepository _repository;
        private readonly IStockPriceRepository _stockPriceRepository;

        public WatchListController(ISmartTraderContext context, IWatchListRepository repository, IStockPriceRepository stockPriceRepository)
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
        public IActionResult Add(WatchList model)
        {
            model.Date = DateTime.Now;

            _repository.Add(model);
            _repository.SaveChanges();

            return Ok();
        }


        [HttpPut("{watchListId}")]
        public IActionResult Put(int watchListId, WatchList model)
        {
            var existingWatchList = _repository.Get(model.WatchListId);

            if (existingWatchList != null && watchListId == existingWatchList.WatchListId)
            {
                existingWatchList.Description = model.Description;
                //existingWatchList.Price = model.Price;
                existingWatchList.ReasonToWatch = model.ReasonToWatch;
                existingWatchList.Status = model.Status;
                existingWatchList.UpdateTime = DateTime.Now;

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
            var openWatchLists = _repository.Find(x => x.Status == "Open");

            var codes = openWatchLists.Select(x => x.Symbol).Distinct().ToList<string>();

            var lastDate = _context.StockPrices.Max(t => t.Date);

            var latestPrices = _stockPriceRepository.Find(x => codes.Contains(x.SymbolName) && x.Date == lastDate).ToList();

            foreach (var item in openWatchLists)
            {
                var current = latestPrices.FirstOrDefault(x => x.SymbolName == item.Symbol);
                if (current != null)
                {
                    item.CurrentPrice = (double)current.Close;
                    item.UpdateTime = lastDate;
                    item.ChangeSinceAdded = ((item.CurrentPrice - item.Price) / item.Price * 100);
                    item.Days = (int)(lastDate - item.Date).TotalDays;
                }

            }
            _repository.SaveChanges();

            return Ok();
        }

        [HttpDelete("{watchListId}")]
        public IActionResult Delete(int watchListId)
        {
            if (watchListId <= 0)
                return BadRequest("Not a valid watch list id");

            var existingWatchList = _repository.Get(watchListId);

            _repository.Remove(existingWatchList);
            _repository.SaveChanges();
            return Ok();
        }

    }

}

