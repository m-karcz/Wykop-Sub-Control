using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubControl
{
    class Program
    {
   /*     static async void testUpdate()
        {
            DatabaseUpdater updater = new DatabaseUpdater();
            await updater.UpdateSubs();
            Console.WriteLine("ok");
            return;
        }*/
        static void Main(string[] args)
        {
          //  if (args.Count() == 1)
            {

                DatabaseUpdater updater = new DatabaseUpdater();
             //   if (args[0] == "-subs")
                    updater.UpdateSubs().Wait();
            //    else if (args[0] == "-avatars")
                    ;
            }
        }
    }
}
