using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.NewDonateShop.Configs;
using Whistler.NewDonateShop.Enums;
using Whistler.NewDonateShop.Interfaces;
using Whistler.NewDonateShop.Models;
using Whistler.SDK;

namespace Whistler.NewDonateShop
{   
    public class PrimeWallet: Wallet
    {
        protected override string GetPayUrl(int sum, int orderId, string email, string comment)
        {
            using var client = new HttpClient();
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("action", "initPayment"),
                new KeyValuePair<string, string>("project", Main.ServerConfig.DonateConfig.ProjectId.ToString()),
                new KeyValuePair<string, string>("sum", sum.ToString()),
                new KeyValuePair<string, string>("currency",  Main.ServerConfig.DonateConfig.Currency),
                new KeyValuePair<string, string>("innerID", orderId.ToString()),
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("sign", GetRequestSign(orderId, sum, email)),
                new KeyValuePair<string, string>("returnLink", "1"),
                new KeyValuePair<string, string>("lang",  Main.ServerConfig.DonateConfig.Language),
                new KeyValuePair<string, string>("comment",  comment)
            });
            var resultString = client.PostAsync(Main.ServerConfig.DonateConfig.PayUrl, formContent).Result.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Result>(resultString);
            return result.result;
        }

        private static string GetRequestSign(int innerID, int sum, string email)
        {
            var key = $"{ Main.ServerConfig.DonateConfig.Secret}{"initPayment"}{ Main.ServerConfig.DonateConfig.ProjectId}{sum}{ Main.ServerConfig.DonateConfig.Currency}{innerID}{email}";
            using (var sha = MD5.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(key));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
