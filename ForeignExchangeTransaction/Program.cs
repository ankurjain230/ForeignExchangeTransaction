using System;
using System.Configuration;
using System.IO;
using FixerApi;
using CsvFileProcessor;
using System.Collections.Generic;
using BusinessObject;

namespace ForeignExchangeTransaction
{
    class Program
    {

        static void Main(string[] args)
        {
            var filePath = ConfigurationManager.AppSettings["FileLocation"];
            if (!File.Exists(filePath))
            {
                Console.WriteLine("file does not exist.\n\nPress any key to exit...");
            }
            else if(Path.GetExtension(filePath)!=".csv")
            {
                Console.WriteLine("File is not in CSV format. Please provide CSV format file.\n\nPress any key to exit...");
            }
            else
            {
                var client = new FixerClient();
                var fileProcessor = new FileProcessor(client, filePath);
                Console.WriteLine("Please note: If any row in CSV file provided by user contains some invalid data then that row will be skipped. Information related to invalid data will be provided at the last with row number.\n\n");
                Printer(fileProcessor.FileReader());

                if (fileProcessor.ErrorList != null && fileProcessor.ErrorList.Count > 0)
                {
                    foreach (var item in fileProcessor.ErrorList)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
            Console.Read();
        }

        private static void Printer(IEnumerable<ExchangeData> collection)
        {
            foreach (var item in collection)
            {
                if (item.CounterCurrencyAmount != 0)
                    Console.WriteLine("{0}  {1}", item.CounterCurrency, item.CounterCurrencyAmount);
            }
        }
    }
}
