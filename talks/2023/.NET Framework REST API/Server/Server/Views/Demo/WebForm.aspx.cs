using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Server.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Server.Views.Demo
{
    public partial class WebForm : System.Web.UI.Page
    {
        public WeatherDemo Weather = new WeatherDemo();
        private HttpClient client;

        protected async void Page_Load(object sender, EventArgs e)
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri($"https://localhost:{Request.Url.Port}/")
            };

            var url = Search.Text.IsNullOrWhiteSpace()
                    ? "api/weather"
                    : $"api/weather/locations/{Search.Text}";
            var response = await client.GetStringAsync(url).ConfigureAwait(false);
            AddRequest($"GET {url}", "");
            if (response != null)
            {
                Weather.WeatherData = JsonConvert.DeserializeObject<List<WeatherInfo>>(response);
            }
        }

        protected void Search_TextChanged(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                var url = $"api/weather/locations/{Search.Text}";
                var response = await client.GetStringAsync(url).ConfigureAwait(false);
                AddRequest($"GET {url}", "");
                if (response != null)
                {
                    Weather.WeatherData = JsonConvert.DeserializeObject<List<WeatherInfo>>(response);
                    Response.Redirect(Request.RawUrl, true);
                }
            });
        }

        void AddRequest(string url, string data)
        {
            if (ViewState["requests"] == null)
            {
                ViewState["requests"] = new List<(string, string)>();
            };
            (ViewState["requests"] as List<(string, string)>).Insert(0, (url, data));
        }
    }
}