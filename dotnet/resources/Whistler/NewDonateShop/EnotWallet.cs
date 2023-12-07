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
    public class EnotWallet: Wallet
    {
        protected override string GetPayUrl(int sum, int orderId, string email, string comment)
        {
            using var client = new HttpClient();           
            return $"{Main.ServerConfig.DonateConfig.PayUrl}?m={Main.ServerConfig.DonateConfig.ProjectId}&oa={sum}&o={orderId}&s={GetRequestSign(Main.ServerConfig.DonateConfig.ProjectId, sum, orderId)}";
        }

        private static string GetRequestSign(int merchantId, int sum, int paymentId)
        {
            var key = $"{merchantId}:{sum}:{Main.ServerConfig.DonateConfig.Secret}:{paymentId}";
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
