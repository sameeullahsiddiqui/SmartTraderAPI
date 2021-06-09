using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Models
{
    public class FileCategory
    {
        public int FileCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Url { get; set; }
        public string TargetPath { get; set; }
        public FileType FileType { get; set; }
        public bool IsUniqueFile { get; set; }
        public bool IsEnabled { get; set; }
        public string FileName { get;  set; }

    }

    public enum FileType
    {
        Zip,
        Text,
        Csv,
        BackUp,
        Dat
    }

}
