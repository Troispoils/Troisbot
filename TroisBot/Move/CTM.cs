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
using TroisBot.Simu;
using System.Windows.Forms;

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
            float x, xt;
            float y, yt;
            float z, zt;
            //float dist;

            Position dest = new Position();

            using (var detour = new Detour())
            {
                bool oneMove = true;
                List<MapCLI.Point> resultPath;
                var result = detour.FindPath(player.XPos, player.YPos, player.ZPos,
                                        target.XPos, target.YPos, target.ZPos,
                                        (int)player.MapId, out resultPath);
                //ClickToMove(resultPath[0].X, resultPath[0].Y, resultPath[0].Z);
                for (int i = 0; i < resultPath.Count; i++)
                {
                    if (oneMove)
                        ClickToMove(resultPath[i].X, resultPath[i].Y, resultPath[i].Z);
                    else
                        oneMove = false;

                    x = write.ReadFloat((uint)(player.BaseAddress + ObjectOffsets.Pos_X));
                    y = write.ReadFloat((uint)(player.BaseAddress + ObjectOffsets.Pos_Y));
                    z = write.ReadFloat((uint)(player.BaseAddress + ObjectOffsets.Pos_Z));
                    /*xt = write.ReadFloat((uint)(target.BaseAddress + ObjectOffsets.Pos_X));
                    yt = write.ReadFloat((uint)(target.BaseAddress + ObjectOffsets.Pos_Y));
                    zt = write.ReadFloat((uint)(target.BaseAddress + ObjectOffsets.Pos_Z));*/
                    player.SetPosition(x, y, z);
                    dest.SetPosition(target.XPos, target.YPos, target.ZPos);

                    var dist = dest - player;

                    if (dist.Length < 6f)
                    {
                        new KeySim().KeyDown((int)Keys.Down);
                        new KeySim().KeyUp((int)Keys.Down);
                        break;
                    }
                    else if (y > resultPath[i].X - 0.5F && y < resultPath[i].X + 0.5F && x > resultPath[i].Y - 0.5f && x < resultPath[i].Y + 0.5f)
                    {
                        ClickToMove(resultPath[i].X, resultPath[i].Y, resultPath[i].Z);
                    }
                    else
                        i--;

                    //Thread.Sleep(100);
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
