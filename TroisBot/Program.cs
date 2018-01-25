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
                bool switchControl = true;

                ManageObject test = new ManageObject(BMagic);
                while(switchControl)
                {
                    test.ScanObject();
                    Console.WriteLine("Scan Finish");
                    test.affichetest();

                    Wait(10);
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

        static void Wait(int Seconds)
        {
            DateTime Tthen = DateTime.Now;
            do
            {
                 //Or something else or leave empty;
            } while (Tthen.AddSeconds(Seconds) > DateTime.Now);
        }
    }
}
