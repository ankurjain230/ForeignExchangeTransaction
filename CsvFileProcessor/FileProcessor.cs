using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BusinessObject;
using FixerApi;

namespace CsvFileProcessor
{
    public class FileProcessor
    {
        #region Field
        private IFixerClient _client;
        private string _filePath;
        private int _count = 0;
        #endregion

        public List<string> ErrorList { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="filePath"></param>
        public FileProcessor(IFixerClient client, string filePath)
        {
            _client = client;
            _filePath = filePath;
            ErrorList = new List<string>();
        }

        /// <summary>
        /// Read CSV file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ExchangeData> FileReader()
        {
            IEnumerable<ExchangeData> exchangeData;

            exchangeData = File.ReadAllLines(_filePath).Select(x => GetDataFromCsv(x));

            return exchangeData;
        }

        /// <summary>
        /// Convert data from CSV file into Exchange data object
        /// </summary>
        /// <param name="csvLines"></param>
        /// <returns>Return ExchangeData object</returns>
        private ExchangeData GetDataFromCsv(string csvLines)
        {
            var exchangeData = new ExchangeData();

            try
            {
                if (!string.IsNullOrEmpty(csvLines))
                {
                    ++_count;
                    var splitValue = csvLines.Split(',');

                    exchangeData.TradeDate = ConvertStringToDate(splitValue[0]);
                    exchangeData.BaseCurrency = CurrencySymbolValidator(splitValue[1]);
                    exchangeData.CounterCurrency = CurrencySymbolValidator(splitValue[2]);
                    exchangeData.BaseCurrencyAmount = Convert.ToDouble(splitValue[3]);
                    exchangeData.ValueDate = ConvertStringToDate(splitValue[4]);
                    exchangeData.CounterCurrencyAmount = GetAmount(exchangeData);

                }
            }
            catch (Exception ex)
            {
                ErrorList.Add(string.Format("Error {0} at line number {1}", ex.Message, _count));
            }

            return exchangeData;
        }

        private double GetAmount(ExchangeData exchangeData)
        {
            return exchangeData.BaseCurrencyAmount * _client.GetExchangeRate(exchangeData);
        }

        private DateTime ConvertStringToDate(string data)
        {
            DateTime dt;
            if (!DateTime.TryParse(data, out dt))
            {
                throw new FormatException(string.Format("Not a valid date {0}", data));
            }
            if (dt != null && dt < new DateTime(1999, 1, 1))
            {
                throw new NotSupportedException(string.Format("Only Currency information after 1999 is available. User Provided Date {0}", dt));
            }
            return dt;
        }
        private string CurrencySymbolValidator(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentNullException("Symbol cannot be null or whitespace.");
            }
            else if (symbol.Length != 3)
            {
                throw new NotSupportedException(string.Format("Invalid Currency Code {0}", symbol));
            }
            foreach (char c in symbol)
            {
                if (!char.IsLetter(c))
                {
                    throw new NotSupportedException(string.Format("Invalid Currency Code {0}", symbol));
                }
            }
            return symbol;
        }
    }
}
