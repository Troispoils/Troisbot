using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TroisBot.Memory
{
    class Offset
    {
        public enum ClientOffsets : uint
        {
            StaticClientConnection = 0x00C79CE0,
            ObjectManagerOffset = 0x2ED0,
            FirstObjectOffset = 0xAC,
            LocalGuidOffset = 0xC0,
            NextObjectOffset = 0x3C,
            LocalPlayerGUID = 0xBD07A8,
            LocalTargetGUID = 0x00BD07B0,
        }

        public enum NameOffsets : ulong
        {
            nameStore = 0x00C5D938 + 0x8,
            nameMask = 0x24,
            nameBase = 0x1C,
            nameString = 0x20
        }

        public enum ObjectOffsets : uint
        {
            Type = 0x14,
            Pos_X = 0x79C,
            Pos_Y = 0x798,
            Pos_Z = 0x7A0,
            Rot = 0x7A8,
            Guid = 0x30,
            UnitFields = 0x8
        }

        public enum UnitOffsets : uint
        {
            Level = 0x36 * 4,
            Health = 0x18 * 4,
            Energy = 0x19 * 4,
            MaxHealth = 0x20 * 4,
            SummonedBy = 0xE * 4,
            MaxEnergy = 0x21 * 4
        }

        public enum ClickToMovex : uint
        {
            CGPlayer_C__ClickToMove = 0x00727400,       // 3.3.5a 12340
            GetClickToMoveStruct = 0x00715CF0,          // 3.3.5a 12340
            CTM_Activate_Pointer = 0xBD08F4,            // 3.3.5a 12340
            CTM_Activate_Offset = 0x30,                 // 3.3.5a 12340

            CTM_Base = 0x00CA11D8,                      // 3.3.5a 12340
            CTM_X = 0x8C,                               // 3.3.5a 12340
            CTM_Y = 0x90,                               // 3.3.5a 12340
            CTM_Z = 0x94,                               // 3.3.5a 12340
            CTM_TurnSpeed = 0x4,                        // 3.3.5a 12340
            CTM_Distance = 0xC,                         // 3.3.5a 12340
            CTM_Action = 0x1C,                          // 3.3.5a 12340
            CTM_GUID = 0x20,                            // 3.3.5a 12340
        }
    }
}
