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
        Thread th;

        public CTM()
        {
            //th = new Thread(GenerathPathAsync);
        }

        public CTM(Player p, Player t, BlackMagic w)
        {
            player = p;
            target = t;
            write = w;
        }

        public void majCTM(Player p, Player t, BlackMagic w)
        {
            th = new Thread(GenerathPathAsync);
            player = p;
            target = t;
            write = w;
        }

        public CTM(BlackMagic w)
        {
            write = w;
        }

        public void StartThread()
        {
            
            th.Start();
        }

        public void StopThread()
        {
            th.Abort();
        }

        public bool ThreadAlive()
        {
            if (th.IsAlive)
                return true;
            else
                return false;
        }

        public List<MapCLI.Point> GenerathPath(Player locPlayer, Player loctarget)
        {
            using (var detour = new Detour())
            {
                List<MapCLI.Point> resultPath;
                var result = detour.FindPath(player.XPos, player.YPos, player.ZPos,
                                        target.XPos, target.YPos, target.ZPos,
                                        (int)player.MapID, out resultPath);

                return resultPath;
            }
        }

        private void GenerathPathAsync()
        {
            float x = 0, xt;
            float y = 0, yt;
            float z = 0, zt;
            //float dist;

            Position dest = new Position();

            using (var detour = new Detour())
            {
                bool oneMove = true;
                List<MapCLI.Point> resultPath;
                var result = detour.FindPath(player.X, player.Y, player.Z,
                                        target.X, target.Y, target.Z,
                                        (int)player.MapID, out resultPath);
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
                    //dest.SetPosition(target.XPos, target.YPos, target.ZPos);

                    var dist = target.Direction - player.Direction;

                    /*if (dist.Length < 6f)
                    {
                        new KeySim().KeyDown((int)Keys.Down);
                        new KeySim().KeyUp((int)Keys.Down);
                        break;
                    }
                    else*/ if (x > resultPath[i].X - 0.5F && x < resultPath[i].X + 0.5F && y > resultPath[i].Y - 0.5f && y < resultPath[i].Y + 0.5f)
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
