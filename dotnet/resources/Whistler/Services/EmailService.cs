using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Services
{
    public static class EmailService
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(EmailService));

        private static HttpClient httpClient = new HttpClient();
        private static bool _initialized = false;
        public static async Task<bool> SendNewPasswordTo(string email, string password)
        {
            try
            {
                if (!_initialized)
                {
                    httpClient.BaseAddress = new Uri(Main.ServerConfig.MailService.Url);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                    _initialized = true;
                }
                HttpResponseMessage result = await httpClient.GetAsync($"?email={email}&password={password}");
                return result.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _logger.WriteError($"SendNewPasswordTo:\n{e}");
                return false;
            }
        }
    }
}
