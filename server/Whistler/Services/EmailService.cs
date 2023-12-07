using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Services
{
    public static class EmailService
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(EmailService));
        public static bool SendNewPasswordTo(string email, string password)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Main.ServerConfig.MailService.Url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                return  client.GetAsync($"?email={email}&password={password}").Result.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _logger.WriteError($"SendNewPasswordTo:\n{e}");
                return false;
            }
        }
    }
}
