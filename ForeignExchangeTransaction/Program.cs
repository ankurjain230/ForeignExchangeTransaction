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
                Console.WriteLine("file does not exist.");
            }
            else
            {
                var client = new FixerClient();
                var fileProcessor = new FileProcessor(client,filePath);
                Printer(fileProcessor.FileReader());
            }
            Console.Read();
        }

        private static  void Printer(IEnumerable<ExchangeData> collection)
        {
            foreach (var item in collection)
            {
                Console.WriteLine("{0}  {1}", item.CounterCurrency, item.CounterCurrencyAmount);
            }
        }
    }
}
