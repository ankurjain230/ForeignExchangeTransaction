using BusinessObject;

namespace FixerApi
{
    public interface IFixerClient
    {
        double GetExchangeRate(ExchangeData exchangeData);
    }
}
