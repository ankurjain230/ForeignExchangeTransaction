namespace FixerApi
{
    public class JsonParser
    {
        /// <summary>
        /// Parse Json 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="counterCurrency"></param>
        /// <returns></returns>
        public static double JsonProcessor(string data, string counterCurrency)
        {
            var root = Newtonsoft.Json.Linq.JObject.Parse(data);
            var rates = root.Value<Newtonsoft.Json.Linq.JObject>("rates");
            var rate = rates.Value<double>(counterCurrency);
            return rate;
        }
    }
}
