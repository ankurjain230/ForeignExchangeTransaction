using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using BusinessObject;

namespace FixerApi
{
    public class FixerClient : IFixerClient
    {
        #region Field
        private const string BaseUri = "http://api.fixer.io/";
        #endregion

        /// <summary>
        /// Return Currency Exchange rate
        /// </summary>
        /// <param name="exchangeData"></param>
        /// <returns></returns>
        public double GetExchangeRate(ExchangeData exchangeData)
        {
            return GetRate(exchangeData);
        }


        /// <summary>
        /// Get Rate
        /// </summary>
        /// <param name="exchangeData"></param>
        /// <returns></returns>
        private double GetRate(ExchangeData exchangeData)
        {
            exchangeData.BaseCurrency = exchangeData.BaseCurrency.ToUpper();
            exchangeData.CounterCurrency = exchangeData.CounterCurrency.ToUpper();
            var dateString = exchangeData.ValueDate.HasValue ? exchangeData.ValueDate.Value.ToString("yyyy-MM-dd") : "latest";
            var uri = string.Format("{0}{1}?base={2}&symbols={3}",BaseUri,dateString,exchangeData.BaseCurrency,exchangeData.CounterCurrency);
            try
            {
                using (var client = new WebClient())
                {
                    // this sleep used to send request in some time interval as to avoid 429 error.
                    Thread.Sleep(4000);
                    return JsonParser.JsonProcessor( client.DownloadString(uri),exchangeData.CounterCurrency);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
