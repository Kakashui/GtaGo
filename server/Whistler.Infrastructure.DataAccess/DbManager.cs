using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Infrastructure.DataAccess
{
    public class DbManager
    {
        private static ServerContext _serverContext;       
        public static ServerContext GlobalContext => _serverContext ?? (_serverContext = new ServerContext());
        public static ServerContext TemporaryContext => new ServerContext();

        private static ServerContext _fracContext;
        public static ServerContext FractionContext => _fracContext ?? (_fracContext = new ServerContext());



        public static bool Test()
        {
            try
            {
                using (var serverContext = new ServerContext())
                {
                    serverContext.Database.Migrate();
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception on database test: " + e.ToString());
                return false;
            }
        }

        public static async void SaveDatabase()
        {
            try
            {
                await FractionContext.SaveChangesAsync();
                await GlobalContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception on SaveDatabase gtago-core: " + e.ToString());
            }
        }
    }
}
