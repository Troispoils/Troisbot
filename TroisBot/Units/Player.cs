using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TroisBot.Units
{
    class Player : ICloneable
    {
        // general properties
        public ulong Guid = 0;
        public ulong SummonedBy = 0;
        public float XPos = 0;
        public float YPos = 0;
        public float ZPos = 0;
        public float Rotation = 0;
        public uint BaseAddress = 0;
        public uint UnitFieldsAddress = 0;
        public short Type = 0;
        public String Name = "";


        // more specialised properties (player or mob)
        public uint CurrentHealth = 0;
        public uint MaxHealth = 0;
        public uint CurrentEnergy = 0; // mana, rage and energy will all fall under energy.
        public uint MaxEnergy = 0;
        public uint Level = 0;

        public bool targeted = false;

        public bool isDead = false;

        public Player()
        {

        }

        public Player(ulong cGuid, ulong cSummonedBy, float cXPos, float cYPos, float cZPos, float cRotation, uint cBaseAddress, uint cUnitFieldsAddress, short cType, String cName, uint cCurrentHealth, uint cMaxHealth, uint cCurrentEnergy, uint cMaxEnergy, uint cLevel, bool cisDead)
        {
            Guid = cGuid;
            SummonedBy = cSummonedBy;
            XPos = cXPos;
            YPos = cYPos;
            ZPos = cZPos;
            Rotation = cRotation;
            BaseAddress = cBaseAddress;
            UnitFieldsAddress = cUnitFieldsAddress;
            Type = cType;
            Name = cName;
            CurrentHealth = cCurrentHealth;
            MaxHealth = cMaxHealth;
            CurrentEnergy = cCurrentEnergy;
            MaxEnergy = cMaxEnergy;
            Level = cLevel;

            isDead = cisDead;
        }

        public object Clone()
        {
            return new Player(Guid, SummonedBy, XPos, YPos, ZPos, Rotation, BaseAddress, UnitFieldsAddress, Type, Name, CurrentHealth, MaxHealth, CurrentEnergy, MaxEnergy, Level, isDead);
        }
    }
}
