using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubControl
{
    public class User
    {
        public User(Dictionary<string, string> fromApi)
        {
            this.name = fromApi["login"];
            this.state = State.Existing;
            this.avatarUrl = fromApi["avatar_med"];
            this.gender = fromApi["sex"].ToGender();
        }
        public override string ToString()
        {
            return "{name:" + name + ",state:" + state.ToString() + ",gender:" + gender.ToString() + "}";
        }
        public string GetName()
        {
            return name;
        }
        public string GetAvatar()
        {
            return avatarUrl;
        }
        public string GetGender()
        {
            return gender.ToString();
        }
        string name;
        State state;
        int timestamp;
        string avatarUrl;
        Gender gender;
    }
}
