//using System;
//using System.Collections.Generic;
//using System.Text;
//using System;
//using System.Collections;
//using SmartTrader.Core.Models;
//using System.Linq;

//namespace SmartTrader.Core.Helpers
//{
//    public class SwingHighLow
//    {
//        #region Properties
//        public List<decimal> High { get; set; }
//        public List<decimal> Low { get; set; }

//        //[Parameter("Strength", DefaultValue = 5, MinValue = 1)]
//        public int strength { get; set; }

//        //[Output("Swing high", Color = Colors.Green, Thickness = 4, PlotType = PlotType.Points)]
//        public List<decimal> SwingHighPlot { get; set; }

//        //[Output("Swing low", Color = Colors.Orange, Thickness = 4, PlotType = PlotType.Points)]
//        public List<decimal> SwingLowPlot { get; set; }
//        #endregion

//        #region Variables
//        private double currentSwingHigh = 0;
//        private double currentSwingLow = 0;
//        private double lastSwingHighValue = 0;
//        private double lastSwingLowValue = 0;
//        private int CurrentBar, Count;
//        private int saveCurrentBar = -1;
//        private ArrayList lastHighCache, lastLowCache;
//        private List<decimal> swingHighSeries, swingHighSwings, swingLowSeries, swingLowSwings;
//        #endregion

//        public void GenerateIndicators(List<StockPrice> prices)
//        {

//            lastHighCache = new ArrayList();
//            lastLowCache = new ArrayList();

//            //swingHighSeries = prices.Select(x=>x.High).ToList();
//            //swingHighSwings = CreateList<decimal>();
//            //swingLowSeries = CreateList<decimal>();
//            //swingLowSwings = CreateList<decimal>();

//            High = prices.Select(x=>x.High).ToList();
//            Low = prices.Select(x => x.Low).ToList();
//            //swingLowSeries = CreateList<decimal>();
//            //swingLowSwings = CreateList<decimal>();

//        }

//        public void Calculate(int index)
//        {
//            CurrentBar = index;
//            Count = High.Count;

//            if (saveCurrentBar != CurrentBar)
//            {
//                swingHighSwings[index] = 0;
//                swingLowSwings[index] = 0;
//                swingHighSeries[index] = 0;
//                swingLowSeries[index] = 0;
//                lastHighCache.Add(High.Last());

//                if (lastHighCache.Count > (2 * strength) + 1)
//                    lastHighCache.RemoveAt(0);

//                lastLowCache.Add(Low.Last());

//                if (lastLowCache.Count > (2 * strength) + 1)
//                    lastLowCache.RemoveAt(0);

//                if (lastHighCache.Count == (2 * strength) + 1)
//                {
//                    bool isSwingHigh = true;
//                    double swingHighCandidateValue = (double)lastHighCache[strength];

//                    for (int i = 0; i < strength; i++)
//                        if ((double)lastHighCache[i] >= swingHighCandidateValue - double.Epsilon)
//                            isSwingHigh = false;

//                    for (int i = strength + 1; i < lastHighCache.Count; i++)
//                        if ((double)lastHighCache[i] > swingHighCandidateValue - double.Epsilon)
//                            isSwingHigh = false;

//                    swingHighSwings[index - strength] = isSwingHigh ? swingHighCandidateValue : 0.0;

//                    if (isSwingHigh)
//                        lastSwingHighValue = swingHighCandidateValue;

//                    if (isSwingHigh)
//                    {
//                        currentSwingHigh = swingHighCandidateValue;
//                        for (int i = 0; i <= strength; i++)
//                            SwingHighPlot[index - i] = currentSwingHigh;
//                    }
//                    else if (High.Last(0) > currentSwingHigh)
//                    {
//                        currentSwingHigh = 0.0;
//                        SwingHighPlot[index] = double.NaN;
//                    }
//                    else
//                        SwingHighPlot[index] = currentSwingHigh;

//                    if (isSwingHigh)
//                    {
//                        for (int i = 0; i <= strength; i++)
//                            swingHighSeries[index - i] = lastSwingHighValue;
//                    }
//                    else
//                    {
//                        swingHighSeries[index] = lastSwingHighValue;
//                    }
//                }

//                if (lastLowCache.Count == (2 * strength) + 1)
//                {
//                    bool isSwingLow = true;
//                    double swingLowCandidateValue = (double)lastLowCache[strength];
//                    for (int i = 0; i < strength; i++)
//                        if ((double)lastLowCache[i] <= swingLowCandidateValue + double.Epsilon)
//                            isSwingLow = false;

//                    for (int i = strength + 1; i < lastLowCache.Count; i++)
//                        if ((double)lastLowCache[i] < swingLowCandidateValue + double.Epsilon)
//                            isSwingLow = false;

//                    swingLowSwings[index - strength] = isSwingLow ? swingLowCandidateValue : 0.0;
//                    if (isSwingLow)
//                        lastSwingLowValue = swingLowCandidateValue;

//                    if (isSwingLow)
//                    {
//                        currentSwingLow = swingLowCandidateValue;
//                        for (int i = 0; i <= strength; i++)
//                            SwingLowPlot[index - i] = currentSwingLow;
//                    }
//                    else if (Low.Last(0) < currentSwingLow)
//                    {
//                        currentSwingLow = double.MaxValue;
//                        SwingLowPlot[index] = double.NaN;
//                    }
//                    else
//                        SwingLowPlot[index] = currentSwingLow;

//                    if (isSwingLow)
//                    {
//                        for (int i = 0; i <= strength; i++)
//                            swingLowSeries[index - i] = lastSwingLowValue;
//                    }
//                    else
//                    {
//                        swingLowSeries[index] = lastSwingLowValue;
//                    }
//                }

//                saveCurrentBar = CurrentBar;
//            }
//            else
//            {
//                if (High.Last(0) > High.Last(strength) && swingHighSwings.Last(strength) > 0.0)
//                {
//                    swingHighSwings[index - strength] = 0.0;
//                    for (int i = 0; i <= strength; i++)
//                        SwingHighPlot[index - i] = double.NaN;
//                    currentSwingHigh = 0.0;
//                }
//                else if (High.Last(0) > High.Last(strength) && currentSwingHigh != 0.0)
//                {
//                    SwingHighPlot[index] = double.NaN;
//                    currentSwingHigh = 0.0;
//                }
//                else if (High.Last(0) <= currentSwingHigh)
//                    SwingHighPlot[index] = currentSwingHigh;

//                if (Low.Last(0) < Low.Last(strength) && swingLowSwings.Last(strength) > 0.0)
//                {
//                    swingLowSwings[index - strength] = 0.0;
//                    for (int i = 0; i <= strength; i++)
//                        SwingLowPlot[index - i] = double.NaN;
//                    currentSwingLow = double.MaxValue;
//                }
//                else if (Low.Last(0) < Low.Last(strength) && currentSwingLow != double.MaxValue)
//                {
//                    SwingLowPlot[index] = double.NaN;
//                    currentSwingLow = double.MaxValue;
//                }
//                else if (Low.Last(0) >= currentSwingLow)
//                    SwingLowPlot[index] = currentSwingLow;
//            }
//        }
//    }
//}
