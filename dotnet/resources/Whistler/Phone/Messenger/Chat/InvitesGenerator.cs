using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Infrastructure.DataAccess;

namespace Whistler.Phone.Messenger.Chat
{
    internal class InvitesGenerator
    {
        public async Task<string> Generate()
        {
            using (var context = DbManager.TemporaryContext)
            {
                string code;

                do
                {
                    code = GenerateRandomStr(6);
                }
                while (await context.Chats.AnyAsync(c => c.InviteCode == code));

                return code;
            }
        }

        private string GenerateRandomStr(int digitsNumber)
        {
            var chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var builder = new StringBuilder(digitsNumber);
            var rndPicker = new Random();

            for (int i = 0; i < digitsNumber; i++)
                builder.Append(chars[rndPicker.Next(0, chars.Length)]);

            return builder.ToString();
        }
    }
}
