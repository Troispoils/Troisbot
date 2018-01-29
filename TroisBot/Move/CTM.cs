using Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TroisBot.Units;
using static TroisBot.Memory.Offset;
using DetourCLI;
using System.Threading;
using TroisBot.Units.Definition;

namespace TroisBot.Move
{
    class CTM
    {

        Player player;
        Player target;
        BlackMagic write;

        public CTM()
        {

        }

        public CTM(Player p, Player t, BlackMagic w)
        {
            player = p;
            target = t;
            write = w;
        }

        public void StartThread()
        {
            Thread th = new Thread(GenerathPathAsync);
            th.Start();
        }

        public void GenerathPathAsync()
        {
            float x, xsav;
            float y;
            float z;
            //float dist;

            Position dest = new Position();

            using (var detour = new Detour())
            {
                List<MapCLI.Point> resultPath;
                var result = detour.FindPath(player.YPos, player.XPos, player.ZPos,
                                        target.YPos, target.XPos, target.ZPos,
                                        (int)player.MapId, out resultPath);
                for(int i = 0; i < resultPath.Count; i++)
                {
                    x = write.ReadFloat((uint)(player.BaseAddress + ObjectOffsets.Pos_X));
                    y = write.ReadFloat((uint)(player.BaseAddress + ObjectOffsets.Pos_Y));
                    z = write.ReadFloat((uint)(player.BaseAddress + ObjectOffsets.Pos_Z));
                    player.position.SetPosition(x, y, z);
                    dest.SetPosition(resultPath[i].X, resultPath[i].Y, resultPath[i].Z);

                    //var dist = dest - player.position;

                    if (dest == player.position)
                        ClickToMove(resultPath[i].X, resultPath[i].Y, resultPath[i].Z);
                    else if (x == resultPath[i].X)
                    {
                        ClickToMove(resultPath[i].X, resultPath[i].Y, resultPath[i].Z);
                    }
                    else
                        i--;

                    Thread.Sleep(100);
                }
            }
        }

        public bool ClickToMove(float x, float y, float z)
        {
            write.WriteFloat((uint)ClickToMovex.CTM_Base + (uint)ClickToMovex.CTM_X, x);
            write.WriteFloat((uint)ClickToMovex.CTM_Base + (uint)ClickToMovex.CTM_Y, y);
            write.WriteFloat((uint)ClickToMovex.CTM_Base + (uint)ClickToMovex.CTM_Z, z);
            write.WriteUInt((uint)ClickToMovex.CTM_Base + (uint)ClickToMovex.CTM_Action, 4);
            return true;
        }

        public void Dispose()
        {

        }
    }
}
