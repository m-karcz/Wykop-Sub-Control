using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace SubControl
{
    public class ApiRequest
    {
        public ApiRequest(string type, string method, List<string> parameters)
        {
            this.type = type;
            this.method = method;
            this.methodParameters = parameters;
        }
        public ApiRequest(string type, string method)
        {
            this.type = type;
            this.method = method;
            this.methodParameters = new List<string>();
        }
        string type;
        string method;
        List<string> methodParameters;
        public Dictionary<string, string> queryParameters = new Dictionary<string, string>();
        public SortedDictionary<string, string> postParameters = new SortedDictionary<string, string>();
        public override string ToString()
        {
            return "http://a.wykop.pl/"
                    + type + "/"
                    + method + "/"
                    + string.Join("", methodParameters.Select(s => s + "/"))
                    + "appkey," + ApiData.ApiKey+"/"
                    + string.Join("", queryParameters.Select(s=>s.Key+","+s.Value+"/"));
        }
        public async Task<string> MakeRequest()
        {
            HttpClient client = new HttpClient();
            Dictionary<string, string> result = new Dictionary<string, string>();
            client.DefaultRequestHeaders.Add("apisign", CalculateMD5Hash());
            //temporary without post
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(this.ToString(), "");
            return await responseMessage.Content.ReadAsStringAsync();
            
        }
        public static Dictionary<string, string> GetSingleRecord(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
        public static List<Dictionary<string, string>> GetListOfRecords(string json)
        {
              return JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);
        }
        public string CalculateMD5Hash()
        {
            MD5 md5 = MD5.Create();
            //temporary without POST parameters
            byte[] inputBytes = Encoding.ASCII.GetBytes(ApiData.Secret+this.ToString());
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
