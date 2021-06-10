using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTrader.Core.Helpers;
using SmartTrader.Core.Inerfaces;
using SmartTrader.Core.Models;

namespace SmartTrader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockPriceController : ControllerBase
    {
        private ISmartTraderContext _context;
        private IStockPriceRepository _repository;
        private IEarningRepository _earningRepository;
        private readonly ISymbolRepository _symbolRepository;

        public StockPriceController(ISmartTraderContext context, IStockPriceRepository repository, IEarningRepository earningRepository, ISymbolRepository symbolRepository)
        {

            _context = context;
            _repository = repository;
            _earningRepository = earningRepository;
            _symbolRepository = symbolRepository;

        }

        [HttpGet("{stockname}/{date:DateTime}")]
        public IActionResult GetByName(string stockname, DateTime date)
        {
            var result = _repository.GetByName(stockname, date);
            return Ok(result);
        }


        [HttpGet("UpdateEarningReports/{year}")]
        public IActionResult UpdateEarningReports(int year)
        {

            var earningReport = _earningRepository.Find(x => x.Year == year).ToList();
            var companyNames = earningReport.Select(x => x.Company).Distinct().ToList();
            var symbols = _symbolRepository.Find(x => companyNames.Contains(x.Name)).ToList();

            foreach (var symbol in symbols)
            {
                var currentReport = earningReport.FirstOrDefault(x => x.Company == symbol.Name);
                if (currentReport == null)
                {
                    continue;
                }

                var earningDate = Utilities.PreviousWorkDay(currentReport.Date.Date);
                var stockPrice = _repository.Find(x => x.SymbolName == symbol.Code && x.Date == earningDate).FirstOrDefault();
                if (stockPrice == null)
                    continue;

                var tooltip = "";
                var qStatus = false;
                var yStatus = false;
                switch (currentReport.CurrentQuarter)
                {
                    case "Q1":
                        SetTooltip(currentReport.QoQGrossProfit_Q1,
                                     currentReport.QoQNetProfit_Q1,
                                     currentReport.QoQSales_Q1,
                                     currentReport.YoYGrossProfit_Q1,
                                     currentReport.YoYNetProfit_Q1,
                                     currentReport.YoYSales_Q1,
                                     stockPrice);
                        break;
                    case "Q2":
                        SetTooltip(currentReport.QoQGrossProfit_Q2,
                                     currentReport.QoQNetProfit_Q2,
                                     currentReport.QoQSales_Q2,
                                     currentReport.YoYGrossProfit_Q2,
                                     currentReport.YoYNetProfit_Q2,
                                     currentReport.YoYSales_Q2,
                                     stockPrice);
                        break;
                    case "Q3":
                        SetTooltip(currentReport.QoQGrossProfit_Q3,
                                     currentReport.QoQNetProfit_Q3,
                                     currentReport.QoQSales_Q3,
                                     currentReport.YoYGrossProfit_Q3,
                                     currentReport.YoYNetProfit_Q3,
                                     currentReport.YoYSales_Q3,
                                     stockPrice);
                        break;
                    case "Q4":
                        SetTooltip(currentReport.QoQGrossProfit_Q4,
                                     currentReport.QoQNetProfit_Q4,
                                     currentReport.QoQSales_Q4,
                                     currentReport.YoYGrossProfit_Q4,
                                     currentReport.YoYNetProfit_Q4,
                                     currentReport.YoYSales_Q4,
                                     stockPrice);
                        break;
                    default:
                        break;
                }

            }
            _repository.SaveChanges();
            return Ok();
        }

        private static void SetTooltip(double qoqGrossProfit, double qoqNetProfit, double qoqSales, 
            double yoyGrossProfit, double yoyNetProfit, double yoySales, StockPrice stockPrice)
        {
            string tooltip;
            bool qStatus;
            bool yStatus;

            tooltip = $"<b>QoQ</b>" +
                        $"<br> GP:{ GetColouredSpan(qoqGrossProfit)}% " +
                        $"<br> NP: {GetColouredSpan(qoqNetProfit)}% " +
                        $"<br> Sales:{GetColouredSpan(qoqSales)}% <br>" +
                      $"<br> <b>YoY</b>" +
                      $"<br> GP:{GetColouredSpan(yoyGrossProfit)}% " +
                      $"<br> NP: {GetColouredSpan(yoyNetProfit)}% " +
                      $"<br> Sales:{GetColouredSpan(yoySales)}%";

            qStatus = qoqGrossProfit > 0 && qoqNetProfit > 0 && qoqSales > 0;
            yStatus = yoyGrossProfit > 0 && yoyNetProfit > 0 && yoySales > 0;
            stockPrice.Reason = "E(Q" + (qStatus ? "+" : "-") + ",Y" + (yStatus ? "+)" : "-)");
            stockPrice.Tooltip = tooltip;
        }

        private static string GetColouredSpan(double value)
        {

            var result = "";
            if(value <0)
                result = "<span style='text-align:right; color:red; width:100%; display:block;'>"+ value + "</span>";
            else if (value > 0)
                result = "<span style='text-align:right; color:green; width:100%; display:block;'>" + value + "</span>";

            return result;
        }


        [HttpGet("Split/{stockname}/{date:DateTime}/{operationType}/{oldFaceValue}/{newFaceValue}")]
        public IActionResult Split(string stockname, DateTime date, string operationType, int oldFaceValue, int newFaceValue)
        {
            if (newFaceValue > 0)
            {
                var allStockPrices = _repository.Find(x => x.SymbolName == stockname).OrderBy(x => x.Date).ToList();
                var splitDate = Utilities.PreviousWorkDay(date);
                var currentPrice = allStockPrices.FirstOrDefault(x => x.Date == splitDate);
                if(currentPrice.Reason == "S" || currentPrice.Reason == "B")
                {
                    return NotFound();
                }

                var beforeSplit = allStockPrices.Where(x => x.Date <= splitDate).ToList();

                foreach (var item in beforeSplit)
                {
                    if (operationType == "Split")
                    {
                        item.Open /= (decimal)(oldFaceValue / newFaceValue);
                        item.High /= (decimal)(oldFaceValue / newFaceValue);
                        item.Low /= (decimal)(oldFaceValue / newFaceValue);
                        item.Close /= (decimal)(oldFaceValue / newFaceValue);
                    }
                    else if (operationType == "Bonus")
                    {
                        item.Open *= (oldFaceValue/ (decimal)(oldFaceValue + newFaceValue));
                        item.High *= (oldFaceValue / (decimal)(oldFaceValue + newFaceValue));
                        item.Low *= (oldFaceValue / (decimal)(oldFaceValue + newFaceValue));
                        item.Close *= (oldFaceValue / (decimal)(oldFaceValue + newFaceValue));
                    }
                }

                foreach (var entry in allStockPrices)
                {
                    var monthlyData = allStockPrices.Where(x=>x.Date <= entry.Date).Select(x => Convert.ToDecimal(x.Close)).TakeLast(22).ToArray();
                    if (monthlyData.Length >= 22)
                    {
                        entry.Monthly = Utilities.CalculateChange((decimal)monthlyData.FirstOrDefault(), (decimal)monthlyData.LastOrDefault());
                    }

                    var weeklyData = allStockPrices.Where(x => x.Date <= entry.Date).OrderBy(x => x.Date).Select(x => Convert.ToDecimal(x.Close)).TakeLast(7).ToArray();
                    if (weeklyData.Length >= 7)
                    {
                        entry.Weekly = Utilities.CalculateChange((decimal)weeklyData.FirstOrDefault(), (decimal)weeklyData.LastOrDefault());
                    }
                }

                
                currentPrice.Reason = operationType == "Split"? "S" : "B";
                currentPrice.Tooltip = $"{operationType}({oldFaceValue}:{newFaceValue})";

                _repository.SaveChanges();
            }

            return Ok();
        }


        [HttpGet("UpdateIndicators/{stockname}/{date:DateTime}")]
        public IActionResult UpdateIndicators(string stockname, DateTime date)
        {
            var stockPrices = _repository.Find(x => x.Date <= date && x.SymbolName == stockname).OrderBy(x => x.Date).ToList();

            var lowVolDays = 0;
            foreach (var item in stockPrices)
            {

                if (item.Date == date)
                {
                    Console.WriteLine("Current Date");
                }

                CheckIfLast40DaysHighBroken(stockPrices, item);

                SetCandleStickPattern(item);

                lowVolDays = CheckIfVolumeSpickAfter4Days(lowVolDays, item);
            }

            _repository.SaveChanges();

            return Ok();
        }

        private static void SetCandleStickPattern(StockPrice currentStockPrice)
        {

            var open = currentStockPrice.Open;
            var high = currentStockPrice.High;
            var low = currentStockPrice.Low;
            var close = currentStockPrice.Close;

            var isGreenCandle = open < close;
            var isRedCandle = open > close;

            if (isGreenCandle)
                HammerCandlePattern(currentStockPrice, open, high, low, close);
            else if (isRedCandle)
                HammerCandlePattern(currentStockPrice, close, high, low, open);

        }

        private static void HammerCandlePattern(StockPrice currentStockPrice, decimal open, decimal high, decimal low, decimal close)
        {
            var upperWick = high - close;
            var body = close - open;
            var lowerWick = open - low;

            if (upperWick < lowerWick && 3 * body < lowerWick)
                currentStockPrice.Reason = "Hammer";
            if (currentStockPrice.Reason == "Hammer" && currentStockPrice.DelRatio >= 2)
            {
                currentStockPrice.Reason = "Strong Hammer";
            }
        }


        private static int CheckIfVolumeSpickAfter4Days(int lowVolDays, StockPrice item)
        {
            if (item.DelRatio < 1)
            {
                lowVolDays++;
            }
            else if ((double)item.DelRatio > 1.5 && lowVolDays >= 4 && string.IsNullOrEmpty(item.Reason))
            {
                item.Reason = "Volume Spike";
                lowVolDays = 0;
            }
            else if ((double)item.DelRatio > 5 && string.IsNullOrEmpty(item.Reason))
            {
                item.Reason = "Volume Spike";
                lowVolDays = 0;
            }
            else if (item.DelRatio >= 2)
            {
                lowVolDays = 0;
            }
            else
                lowVolDays = 0;
            return lowVolDays;
        }

        private static void CheckIfLast40DaysHighBroken(List<StockPrice> stockPrices, StockPrice item)
        {
            //take last 40 days high
            var last3MonthData = stockPrices.Where(x => x.Date < item.Date).TakeLast(30).ToList();
            var last3MonthHigh = item.High;

            var is3MBrokenRecently = last3MonthData.Any(x => x.Reason == "3M broken");

            if (last3MonthData.Count() == 30 && !is3MBrokenRecently && (item.Reason != "B" || item.Reason != "S"))
            {
                last3MonthHigh = last3MonthData.Max(x => x.High);

                if (item.Close > last3MonthHigh)
                {
                    item.Reason = "3M broken";
                }
            }
        }
    }
}
