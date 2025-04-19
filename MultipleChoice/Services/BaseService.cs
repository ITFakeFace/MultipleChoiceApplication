using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleChoice.Services
{
    public abstract class BaseService
    {

        protected MySqlConnection GetConnection()
        {
            IConfiguration _configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..")))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
            // Lấy các giá trị từ appsettings.json
            string connectionString = _configuration.GetConnectionString("MySqlConnection");
            return new MySqlConnection(connectionString);

        }
    }
}
