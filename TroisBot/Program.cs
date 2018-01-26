using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic;
using TroisBot.Object;
using DetourCLI;

namespace TroisBot
{
    class Program
    {
        static void Main(string[] args)
        {
            DetourCLI.Detour.Initialize("C:/data/mmaps");
            VMapCLI.VMap.Initialize("C:/data/vmaps");
            MapCLI.Map.Initialize("C:/data/maps");

            using (var detour = new DetourCLI.Detour())
            {
                List<MapCLI.Point> resultPath;
                var result = detour.FindPath(-8896.072266f, -82.352325f, 86.421661f,
                                        -8915.272461f, -111.634041f, 82.275642f,
                                        0, out resultPath);
                
                foreach(var point in resultPath)
                {
                    Console.WriteLine(point.X + " : " + point.Y);
                }
            }
            /*BlackMagic BMagic = new BlackMagic();
            if (BMagic.OpenProcessAndThread(SProcess.GetProcessFromProcessName("Wow")))
            {
                Console.WriteLine("Found and Attached the World of Warcraft Process!");
                //IntPtr BaseWoW = BMagic.MainModule.BaseAddress;
                //Console.WriteLine("Character name is " + pName);
                bool switchControl = true;

                ManageObject test = new ManageObject(BMagic);
                /*while(switchControl)
                {
                    test.ScanObject();
                    Console.WriteLine("Scan Finish");
                    test.affichetest();

                    Wait(10);
                }*/



            /*Console.ReadLine();
            BMagic.Close();
        }
        else
        {
            Console.WriteLine("The World of Warcraft Process was not found!");
            Console.ReadLine();
        }*/

            Console.ReadLine();
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
