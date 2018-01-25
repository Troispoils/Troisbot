using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic;
using static TroisBot.Memory.Offset;
using TroisBot.Mouvement;

namespace TroisBot.Object
{
    class ManageObject
    {
        uint ClientConnection = 0;
        uint ObjectManager = 0;
        uint FirstObject = 0;

        BlackMagic WowReader = new BlackMagic();

        //test CTM
        CTM mouve = new CTM();
        Random rand = new Random();
        

        Objects LocalPlayer = new Objects();
        Objects LocalTarget = new Objects();
        Objects CurrentObject = new Objects();
        Objects TempObject = new Objects();

        ArrayList CurrentPlayers = new ArrayList();

        public ManageObject(BlackMagic read)
        {
            if (LoadAddresses(read))
                Console.WriteLine("Load Addresses ok");
            else
                Console.WriteLine("Load Addresses nok");
        }

        private Boolean LoadAddresses(BlackMagic read)
        {
            WowReader = read;

            ClientConnection = read.ReadUInt((uint)ClientOffsets.StaticClientConnection);
            ObjectManager = read.ReadUInt((uint)(ClientConnection + ClientOffsets.ObjectManagerOffset));
            FirstObject = read.ReadUInt((uint)(ObjectManager + ClientOffsets.FirstObjectOffset));
            LocalTarget.Guid = read.ReadUInt64((uint)(ClientOffsets.LocalTargetGUID));
            LocalPlayer.Guid = read.ReadUInt64((uint)(ObjectManager + ClientOffsets.LocalGuidOffset));

            // if the local guid is zero it means that something failed.
            if (LocalPlayer.Guid == 0)
                return false;
            else
                return true;
        }

        public void ScanObject()
        {
            CurrentPlayers.Clear();

            CurrentObject.BaseAddress = FirstObject;

            LocalPlayer.BaseAddress = GetObjectBaseByGuid(LocalPlayer.Guid);
            LocalPlayer.XPos = WowReader.ReadFloat((uint)(LocalPlayer.BaseAddress + ObjectOffsets.Pos_X));
            LocalPlayer.YPos = WowReader.ReadFloat((uint)(LocalPlayer.BaseAddress + ObjectOffsets.Pos_Y));
            LocalPlayer.ZPos = WowReader.ReadFloat((uint)(LocalPlayer.BaseAddress + ObjectOffsets.Pos_Z));
            LocalPlayer.Rotation = WowReader.ReadFloat((uint)(LocalPlayer.BaseAddress + ObjectOffsets.Rot));
            LocalPlayer.UnitFieldsAddress = WowReader.ReadUInt((uint)(LocalPlayer.BaseAddress + ObjectOffsets.UnitFields));
            LocalPlayer.CurrentHealth = WowReader.ReadUInt((uint)(LocalPlayer.UnitFieldsAddress + UnitOffsets.Health));
            LocalPlayer.CurrentEnergy = WowReader.ReadUInt((uint)(LocalPlayer.UnitFieldsAddress + UnitOffsets.Energy));
            LocalPlayer.MaxHealth = WowReader.ReadUInt((uint)(LocalPlayer.UnitFieldsAddress + UnitOffsets.MaxHealth));
            LocalPlayer.Level = WowReader.ReadUInt((uint)(LocalPlayer.UnitFieldsAddress + UnitOffsets.Level));
            LocalPlayer.MaxEnergy = WowReader.ReadUInt((uint)(LocalPlayer.UnitFieldsAddress + UnitOffsets.MaxEnergy));
            LocalPlayer.Name = PlayerNameFromGuid(LocalPlayer.Guid);
            if (LocalPlayer.CurrentHealth <= 0) { LocalPlayer.isDead = true; }

            LocalTarget.Guid = WowReader.ReadUInt64((uint)(ClientOffsets.LocalTargetGUID));

            if (LocalTarget.Guid != 0)
            {
                LocalTarget.BaseAddress = GetObjectBaseByGuid(LocalTarget.Guid);
                LocalTarget.XPos = WowReader.ReadFloat((uint)(LocalTarget.BaseAddress + ObjectOffsets.Pos_X));
                LocalTarget.YPos = WowReader.ReadFloat((uint)(LocalTarget.BaseAddress + ObjectOffsets.Pos_Y));
                LocalTarget.ZPos = WowReader.ReadFloat((uint)(LocalTarget.BaseAddress + ObjectOffsets.Pos_Z));
                LocalTarget.Type = (short)WowReader.ReadUInt((uint)(LocalTarget.BaseAddress + ObjectOffsets.Type));
                LocalTarget.Rotation = WowReader.ReadFloat((uint)(LocalTarget.BaseAddress + ObjectOffsets.Rot));
                LocalTarget.UnitFieldsAddress = WowReader.ReadUInt((uint)(LocalTarget.BaseAddress + ObjectOffsets.UnitFields));
                LocalTarget.CurrentHealth = WowReader.ReadUInt((uint)(LocalTarget.UnitFieldsAddress + UnitOffsets.Health));
                LocalTarget.CurrentEnergy = WowReader.ReadUInt((uint)(LocalTarget.UnitFieldsAddress + UnitOffsets.Energy));
                LocalTarget.MaxHealth = WowReader.ReadUInt((uint)(LocalTarget.UnitFieldsAddress + UnitOffsets.MaxHealth));
                LocalTarget.Level = WowReader.ReadUInt((uint)(LocalTarget.UnitFieldsAddress + UnitOffsets.Level));
                LocalTarget.SummonedBy = WowReader.ReadUInt64((uint)(LocalTarget.UnitFieldsAddress + UnitOffsets.SummonedBy));
                LocalTarget.MaxEnergy = WowReader.ReadUInt((uint)(LocalTarget.UnitFieldsAddress + UnitOffsets.MaxEnergy));

                if (LocalTarget.Type == 3) // not a human player
                    LocalTarget.Name = MobNameFromGuid(LocalTarget.Guid);
                if (LocalTarget.Type == 4) // a human player
                    LocalTarget.Name = PlayerNameFromGuid(LocalTarget.Guid);
                if (LocalTarget.CurrentHealth <= 0) { LocalTarget.isDead = true; }
                //we don't add LocalTarget to the ArrayList because he or she will appear again later
            }

            // read the object manager from first object to last.
            while (CurrentObject.BaseAddress != 0 && CurrentObject.BaseAddress % 2 == 0)
            {
                CurrentObject.Type = (short)(WowReader.ReadUInt((uint)(CurrentObject.BaseAddress + ObjectOffsets.Type)));

                if (CurrentObject.Type == 4)
                {
                    CurrentObject.UnitFieldsAddress = WowReader.ReadUInt((uint)(CurrentObject.BaseAddress + ObjectOffsets.UnitFields));
                    CurrentObject.CurrentHealth = WowReader.ReadUInt((uint)(CurrentObject.UnitFieldsAddress + UnitOffsets.Health));
                    CurrentObject.CurrentEnergy = WowReader.ReadUInt((uint)(LocalPlayer.UnitFieldsAddress + UnitOffsets.Energy));
                    CurrentObject.MaxHealth = WowReader.ReadUInt((uint)(CurrentObject.UnitFieldsAddress + UnitOffsets.MaxHealth));
                    CurrentObject.XPos = WowReader.ReadFloat((uint)(CurrentObject.BaseAddress + ObjectOffsets.Pos_X));
                    CurrentObject.YPos = WowReader.ReadFloat((uint)(CurrentObject.BaseAddress + ObjectOffsets.Pos_Y));
                    CurrentObject.ZPos = WowReader.ReadFloat((uint)(CurrentObject.BaseAddress + ObjectOffsets.Pos_Z));
                    CurrentObject.Rotation = WowReader.ReadFloat((uint)(CurrentObject.BaseAddress + ObjectOffsets.Rot));
                    CurrentObject.Guid = WowReader.ReadUInt64((uint)(CurrentObject.BaseAddress + ObjectOffsets.Guid));
                    CurrentObject.Level = WowReader.ReadUInt((uint)(CurrentObject.UnitFieldsAddress + UnitOffsets.Level));
                    CurrentObject.MaxEnergy = WowReader.ReadUInt((uint)(CurrentObject.UnitFieldsAddress + UnitOffsets.MaxEnergy));
                    CurrentObject.Name = PlayerNameFromGuid(CurrentObject.Guid);
                    // check to see whether this player is dead or not
                    if (CurrentObject.CurrentHealth <= 0)
                    {
                        CurrentObject.isDead = true;
                    }
                    CurrentPlayers.Add((Objects)CurrentObject.Clone());
                }
                if (CurrentObject.Type == 3)
                {
                    CurrentObject.UnitFieldsAddress = WowReader.ReadUInt((uint)(CurrentObject.BaseAddress + ObjectOffsets.UnitFields));
                    CurrentObject.CurrentHealth = WowReader.ReadUInt((uint)(CurrentObject.UnitFieldsAddress + UnitOffsets.Health));
                    CurrentObject.CurrentEnergy = WowReader.ReadUInt((uint)(LocalPlayer.UnitFieldsAddress + UnitOffsets.Energy));
                    CurrentObject.MaxHealth = WowReader.ReadUInt((uint)(CurrentObject.UnitFieldsAddress + UnitOffsets.MaxHealth));
                    CurrentObject.XPos = WowReader.ReadFloat((uint)(CurrentObject.BaseAddress + ObjectOffsets.Pos_X));
                    CurrentObject.YPos = WowReader.ReadFloat((uint)(CurrentObject.BaseAddress + ObjectOffsets.Pos_Y));
                    CurrentObject.ZPos = WowReader.ReadFloat((uint)(CurrentObject.BaseAddress + ObjectOffsets.Pos_Z));
                    CurrentObject.Rotation = WowReader.ReadFloat((uint)(CurrentObject.BaseAddress + ObjectOffsets.Rot));
                    CurrentObject.Guid = WowReader.ReadUInt64((uint)(CurrentObject.BaseAddress + ObjectOffsets.Guid));
                    CurrentObject.Level = WowReader.ReadUInt((uint)(CurrentObject.UnitFieldsAddress + UnitOffsets.Level));
                    CurrentObject.MaxEnergy = WowReader.ReadUInt((uint)(CurrentObject.UnitFieldsAddress + UnitOffsets.MaxEnergy));
                    CurrentObject.Name = MobNameFromGuid(CurrentObject.Guid);
                    // check to see whether this player is dead or not
                    if (CurrentObject.CurrentHealth <= 0)
                    {
                        CurrentObject.isDead = true;
                    }
                    CurrentPlayers.Add((Objects)CurrentObject.Clone());
                }
                // set the current object as the next object in the object manager
                CurrentObject.BaseAddress = WowReader.ReadUInt((uint)(CurrentObject.BaseAddress + ClientOffsets.NextObjectOffset));
            }
        }

        public void affichetest()
        {
            Console.WriteLine("Nombre de mob visible: " + CurrentPlayers.Count);
            Objects test = (Objects)CurrentPlayers[rand.Next(CurrentPlayers.Count)];

            if(LocalTarget.Guid != 0)
                mouve.ClickToMove(test.YPos, test.XPos, WowReader);

            //WowReader.WriteUInt64((uint)ClientOffsets.LocalTargetGUID, (uint)test.Guid);
        }

        private string MobNameFromGuid(ulong Guid)
        {
            uint ObjectBase = GetObjectBaseByGuid(Guid);
            return WowReader.ReadASCIIString((uint)(WowReader.ReadUInt((uint)(WowReader.ReadUInt((uint)(ObjectBase + 0x964)) + 0x05C))), 20);
        }

        public string PlayerNameFromGuid(ulong guid)
        {
            ulong mask, base_, offset, current, shortGUID, testGUID;

            mask = WowReader.ReadUInt((uint)((ulong)NameOffsets.nameStore + (ulong)NameOffsets.nameMask));
            base_ = WowReader.ReadUInt((uint)((ulong)NameOffsets.nameStore + (ulong)NameOffsets.nameBase));

            shortGUID = guid & 0xffffffff;
            offset = 12 * (mask & shortGUID);

            current = WowReader.ReadUInt((uint)(base_ + offset + 8));
            offset = WowReader.ReadUInt((uint)(base_ + offset));

            if ((current & 0x1) == 0x1) { return ""; }

            testGUID = WowReader.ReadUInt((uint)(current));

            while (testGUID != shortGUID)
            {
                current = WowReader.ReadUInt((uint)(current + offset + 4));

                if ((current & 0x1) == 0x1) { return ""; }
                testGUID = WowReader.ReadUInt((uint)(current));
            }

            return WowReader.ReadASCIIString((uint)(current + NameOffsets.nameString), 20);
        }

        private uint GetObjectBaseByGuid(ulong Guid)
        {
            TempObject.BaseAddress = FirstObject;

            while (TempObject.BaseAddress != 0)
            {
                TempObject.Guid = WowReader.ReadUInt64((uint)(TempObject.BaseAddress + ObjectOffsets.Guid));
                if (TempObject.Guid == Guid)
                    return TempObject.BaseAddress;
                TempObject.BaseAddress = WowReader.ReadUInt((uint)(TempObject.BaseAddress + ClientOffsets.NextObjectOffset));
            }

            return 0;
        }

        private ulong GetObjectGuidByBase(uint Base)
        {
            return WowReader.ReadUInt64((uint)(Base + ObjectOffsets.Guid));
        }
    }
}
