using MultipleChoiceApplication.Models;
using MultipleChoiceApplication.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MultipleChoiceApplication.Services
{
    internal class AuthenticationService : BaseService
    {
        public int Login(string email, string password)
        {
            var passUtil = new SecurityUtil();
            using (var conn = GetConnection())
            {
                User user;
                conn.Open();
                string sql = "SELECT * FROM Users WHERE email = @Email";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32("id"),
                                Email = reader.GetString("email"),
                                Username = reader.GetString("username"),
                                Password = reader.GetString("password")
                            };
                            if (passUtil.HashPassword(password).Equals(user.Password))
                            {
                                return user.Id;
                            }
                            return -1;
                        }
                    }
                }
                conn.Close();
            }
            return -1;
        }
    }
}
