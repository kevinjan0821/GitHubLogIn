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
using System.Linq;

namespace GitHubLogIn.Shared
{
    public class Services
    {
        private class EventModel
        {
            public string eventType;
            public DateTime time; 

        }


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
            string jsonResult = string.Empty;
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler();
 
                handler.UseDefaultCredentials = true;
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                handler.UseCookies = true;

            var client = new HttpClient(handler);
            var authenticationBytes = Encoding.ASCII.GetBytes(userName + ":" + password);

            // Address
            string address = "https://github.com/settings/security-log";
            var uri = new Uri(address);

            // Set headers and url details for authentication and to access the web page
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "application/xhtml+xml");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationBytes));
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

            /* NOTE:
            // I was able to retrieve the https://github.com/settings/security-log by mannually injecting the cookie
            // from inspecting the web page and still looking for a way to make it dynamic*/
            // client.DefaultRequestHeaders.Add("Cookie", "INJECT COOKIE VALUE FOR TEST");

            var result =  await client.GetAsync(uri).ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
            {

                // Get HTML
                var htmlstring = await result.Content.ReadAsStringAsync()
                var html = new HtmlDocument();
                html.LoadHtml(htmlstring);

                // parse the XML value,  div class = "TableObject-item TableObject-item--primary" 
                // got using inspection on the website
                var items = html.DocumentNode.Descendants("div")
                            .Where(div => div.GetAttributeValue("class", "")
                            .Equals("TableObject-item TableObject-item--primary")).ToList();

                var eventList = new List<EventModel>();

                // create the list of events and time for JSON
                foreach (var item in items)
                {
                    var eventModel = new EventModel()
                    {
                        eventType = item.Descendants("span")
                            .Where(span => span.GetAttributeValue("class", "").Equals("audit-type")).FirstOrDefault().InnerText,
                        time = Convert.ToDateTime(item.Descendants("relative-time").FirstOrDefault().ChildAttributes("datetime").FirstOrDefault().Value)
                    };

                    eventList.Add(eventModel);
                }
              
                // Convert to JSON and return for display
                jsonResult = JsonConvert.SerializeObject(eventList);

            }
           else
            {
                jsonResult = result.ReasonPhrase;
            }


            return jsonResult;

        }


    }
}
