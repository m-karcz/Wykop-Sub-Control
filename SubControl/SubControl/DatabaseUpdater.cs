using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubControl
{
    class DatabaseUpdater
    {
        private static int GetTime() => (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        MySql.Data.MySqlClient.MySqlConnection connection=new MySql.Data.MySqlClient.MySqlConnection(ApiData.MysqlAccess);
        public DatabaseUpdater()
        {
            connection.Open();
        }
        ~DatabaseUpdater()
        {
            connection.Close();
        }
        public async Task UpdateSubs()
        {
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("SELECT name, state FROM subs", connection);
            MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();
            List<string> existingInDB = new List<string>();
            List<string> revokedInDB = new List<string>();
            while (reader.Read())
            {
                existingInDB.Add((string)reader[0]);
                if ((string)reader[1] == "Revoked")
                    revokedInDB.Add((string)reader[0]);
            }
            reader.Close();
            var existingOnSite = await ApiRequester.GetAllFollowers();
            var existingOnSiteNames = existingOnSite.Select(u => u.GetName()).ToList();
            var newOnes = existingOnSiteNames.Except(existingInDB);
            var revokedOnes = existingInDB.Except(existingOnSiteNames);
            var returnedOnes = revokedInDB.Intersect(existingOnSiteNames).ToList();
            foreach(var name in newOnes)
            {
                var user = existingOnSite.Find(u => u.GetName() == name);
                var newCmd = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO subs (name, avatar, state, timestamp, gender) VALUES ('"+user.GetName()+"','"+user.GetAvatar()+"','New',"+GetTime()+",'"+user.GetGender()+"');",connection);
                Console.WriteLine(newCmd.CommandText);
                var newReader=newCmd.ExecuteReader();
                while (newReader.Read()) { }
                newReader.Close();
            }
            foreach(var name in revokedOnes)
            {
                var updateCmd = new MySql.Data.MySqlClient.MySqlCommand("UPDATE subs SET state='Revoked', timestamp="+GetTime()+" WHERE name='" + name + "';", connection);
                var updateReader = updateCmd.ExecuteReader();
                while (updateReader.Read()) { }
                updateReader.Close();
            }
            foreach(var name in returnedOnes)
            {
                var returnCmd = new MySql.Data.MySqlClient.MySqlCommand("UPDATE subs SET state='New', timestamp="+GetTime()+" WHERE name='" + name + "';", connection);
                Console.WriteLine(returnCmd.CommandText);
                var returnReader = returnCmd.ExecuteReader();
                while (returnReader.Read()) { }
                returnReader.Close();
            }
            Console.WriteLine("done");
            return;
        }
    }
}
