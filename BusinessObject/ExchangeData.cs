using System;

namespace BusinessObject
{
    public class ExchangeData
    {
        public string BaseCurrency { get; set; }
        public string CounterCurrency { get; set; }
        public DateTime? TradeDate { get; set; }
        public DateTime? ValueDate { get; set; }
        public double BaseCurrencyAmount { get; set; }
        public double CounterCurrencyAmount { get; set; }
    }
}
