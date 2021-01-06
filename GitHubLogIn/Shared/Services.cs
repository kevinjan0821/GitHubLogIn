using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Net;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Linq;

namespace GitHubLogIn.Shared
{
    public class Services
    {



        //static HttpClient client = new HttpClient();
        public string LogInResponse;



        public string LogToGit(string userName, string password)
        {

            var t = Task.Run(() => ExecuteAsync(userName, password));

            t.Wait();

            return t.Result;
        }

        public static async Task<string> ExecuteAsync(string userName, string password)
        {
            string response = string.Empty;

            var handler = new HttpClientHandler();
 
            handler.UseDefaultCredentials = true;

            var client = new HttpClient(handler);

            client.BaseAddress = new Uri("https://api.github.com");

            
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Product", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("CookiName", "cookine_value");

            var authenticationBytes = Encoding.ASCII.GetBytes(userName + ":" + password);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationBytes));

            var result = await client.GetAsync("/users/"+ userName).ConfigureAwait(false);
           

            if (result.IsSuccessStatusCode)
            {
                response = await result.Content.ReadAsStringAsync();

            }
            else
            {
                response = "Failed to connect to GitHub";
            }


            return response;


        }

    }
}
