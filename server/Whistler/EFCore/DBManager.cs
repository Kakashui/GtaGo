using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Whistler.Helpers;

namespace Whistler.EFCore
{
    class DBManager
    {

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Main));
        private static object _lockObj = new object();
        private static ServerContext _serverContext;
        public static ServerContext GlobalContext
        {
            get
            {
                lock(_lockObj)
                {
                    return _serverContext ??= new ServerContext();
                }
                
            }
        }
        public static ServerContext TemporaryContext => new ServerContext();


        public static bool Migrate()
        {
            try
            {
                using (var serverContext = TemporaryContext)
                {
                    serverContext.Database.EnsureCreated();
                    serverContext.Database.Migrate();
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.WriteError("Exception on database test: " + e.ToString());
                return false;
            }
        }

        public static async void SaveDatabase()
        {
            try
            {
                await GlobalContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.WriteError("Exception on SaveDatabase: " + e.ToString());
            }
        }
    }
}
