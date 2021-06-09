using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Models
{
    public class BackgroundJob
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime NextExecution { get; set; }
        public bool IsEnabled { get; set; }

    }
}
