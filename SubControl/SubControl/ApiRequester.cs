using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubControl
{
    public static class ApiRequester
    {
        public async static Task<List<User>> GetAllFollowers()
        {
            var userList = new List<User>();
            for(int i=1; i<255; i++)
            {
                var apiRequest = new ApiRequest("profile", "followers", new List<string>() { ApiData.Nick });
                apiRequest.queryParameters.Add("page", i.ToString());
                var json = await apiRequest.MakeRequest();
                if ("[]" == json)
                    break;
                userList.AddRange(ApiRequest.GetListOfRecords(json).ToUserList());
            }
            return userList;
        }
        public async static void PrintFollowers()
        {
            foreach(var user in await GetAllFollowers())
            {
                Console.WriteLine(user.ToString());
            }
        }
    }
}
