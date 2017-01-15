using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubControl
{
    public static class Extensions
    {
        public static List<User> ToUserList(this List<Dictionary<string, string>> fromApi) => fromApi.Select(u => new User(u)).ToList();
        public static Gender ToGender(this string str)
        {
            switch (str)
            {
                case "male":
                    return Gender.Male;
                case "female":
                    return Gender.Female;
                default:
                    return Gender.None;
            }
        }
    }
}
