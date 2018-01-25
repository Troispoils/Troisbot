using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic;
using TroisBot.Object;

namespace TroisBot
{
    class Program
    {
        static void Main(string[] args)
        {
            BlackMagic BMagic = new BlackMagic();
            if (BMagic.OpenProcessAndThread(SProcess.GetProcessFromProcessName("Wow")))
            {
                Console.WriteLine("Found and Attached the World of Warcraft Process!");
                //IntPtr BaseWoW = BMagic.MainModule.BaseAddress;
                //Console.WriteLine("Character name is " + pName);

                ManageObject test = new ManageObject(BMagic);
                while(true)
                {
                    test.ScanObject();
                    Console.WriteLine("Scan Finish");
                    test.affichetest();
                }
                    
                

                /*Console.ReadLine();
                BMagic.Close();*/
            }
            else
            {
                Console.WriteLine("The World of Warcraft Process was not found!");
                Console.ReadLine();
            }
        }
    }
}
