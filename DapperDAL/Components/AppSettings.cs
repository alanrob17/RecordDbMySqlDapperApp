using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDAL.Components
{
    internal class AppSettings
    {
        private static AppSettings? _Instance;

        public static AppSettings Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new AppSettings();
                }

                return _Instance;
            }
        }

        public string ConnectionString
        {
            get
            {
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                string connectString = config.GetConnectionString("RecordDB");

                if (string.IsNullOrEmpty(connectString))
                {
                    connectString = config["RecordDB"];
                }

                return connectString;
            }
        }
    }
}
