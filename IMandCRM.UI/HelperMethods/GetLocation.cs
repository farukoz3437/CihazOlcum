using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IMandCRM.UI.HelperMethods
{
    public class GetLocation
    {
        public async Task<string> GetUserCountryByIp()
        {
            var Ip_Api_Url = $"http://ip-api.com/json";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(Ip_Api_Url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
    public class Coordinates
    {
        public Coordinates(string json)
        {
            JObject jObject = JObject.Parse(json);
            lat = (string)jObject["lat"];
            lon = (string)jObject["lon"];

        }

        public string lat { get; set; }
        public string lon { get; set; }

    }

}
