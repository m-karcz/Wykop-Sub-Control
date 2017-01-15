using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.IO;

namespace SubControl
{
    class Server
    {
        public readonly int port = 4000;
        HttpListener listener = new HttpListener();
        Thread running;
        MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(MySqlLogin.str);
        public Server()
        {
            listener.Prefixes.Add("http://localhost:" + port+ "/");
            listener.Prefixes.Add("http://127.0.0.1:" + port+ "/");
            connection.Open();
            listener.Start();
            running = new Thread(new ThreadStart(run));
            running.Start();
        }
        ~Server()
        {
            running.Abort();
            listener.Stop();
            connection.Close();
        }
        private void run()
        {
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerResponse response = context.Response;
                response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
                response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                response.AddHeader("Access-Control-Max-Age", "1728000");
                response.AppendHeader("Access-Control-Allow-Origin", "*");
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("SELECT name, avatar, state, timestamp, gender FROM subs", connection);
                var reader = cmd.ExecuteReader();
                List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
                while (reader.Read())
                {
                    var user = new Dictionary<string, string>();
                    user.Add("name", (string)reader[0]);
                    user.Add("avatar", (string)reader[1]);
                    user.Add("state", (string)reader[2]);
                    user.Add("timestamp", ((int)reader[3]).ToString());
                    user.Add("gender", (string)reader[4]);
                    data.Add(user);
                }
                reader.Close();
                string result = JsonConvert.SerializeObject(data);
                byte[] buffer = Encoding.UTF8.GetBytes(result);
                response.ContentLength64 = buffer.Length;
                Stream str = response.OutputStream;
                str.Write(buffer, 0, buffer.Length);
                response.Close();
            }
        }
    }
}
