using Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TroisBot.Memory.Offset;

namespace TroisBot.Mouvement
{
    class CTM
    {
        public CTM()
        {

        }

        public void ClickToMove(float newX, float newY, BlackMagic write )
        {
            uint ClientConnection = write.ReadUInt((uint)ClientOffsets.StaticClientConnection);

            uint CTM_X = (uint)ClickToMovex.CTM_Base + (uint)ClickToMovex.CTM_X;
            uint CTM_Y = (uint)ClickToMovex.CTM_Base + (uint)ClickToMovex.CTM_Y;
            uint CTM_Push = (uint)ClickToMovex.CTM_Base + (uint)ClickToMovex.CTM_Action;

            write.WriteFloat((uint)(CTM_X), newX);
            write.WriteFloat((uint)(CTM_Y), newY);
            write.WriteUInt((uint)(CTM_Push), 4);

            //log
            //Console.WriteLine("CTM - " + (ClientConnection + CTM_X) + " = " + newX + " : "  + (ClientConnection + CTM_Y) + " = " + newY);
        }
    }
}
