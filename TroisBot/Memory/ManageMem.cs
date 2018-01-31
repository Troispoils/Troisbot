using Magic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TroisBot.Radar;
using TroisBot.Units;
using TroisBot.Move;
using static TroisBot.Memory.Offset;

namespace TroisBot.Memory
{
    class ManageMem
    {
        uint ClientConnection = 0;
        uint ObjectManager = 0;
        uint FirstObject = 0;

        BlackMagic WowReader = new BlackMagic();


        Player LocalPlayer = new Player();
        Player LocalTarget = new Player();
        Player CurrentObject = new Player();
        Player TempObject = new Player();

        ArrayList CurrentPlayers = new ArrayList();

        //zonetest
        /*ManageGraph graph = new ManageGraph();
        public Bitmap test(Bitmap test)
        {
            Bitmap coco = new Bitmap(260, 260);
            foreach (Player i in CurrentPlayers)
            {
                if (i.Type == 3)
                {
                    coco = graph.TracePlayerInMap(test, LocalPlayer, i, Color.Red);
                }
                else if (i.Type == 4)
                {
                    if (i.Name == LocalPlayer.Name)
                        coco = graph.TracePlayerInMap(test, LocalPlayer, i, Color.Blue);
                    else
                        coco = graph.TracePlayerInMap(test, LocalPlayer, i, Color.Green);
                }
                //coco = graph.TracePlayerInMap(test, LocalPlayer, i);
            }
            if(LocalTarget.Guid != 0)
                coco = graph.TracePlayerInMap(test, LocalPlayer, LocalTarget, Color.Yellow, true);

            return coco;
        }*/

        public void testmoveAsync()
        {
            CTM move = new CTM(LocalPlayer, LocalTarget, WowReader);
            move.StartThread();
        }
        //finzonetest

        public ManageMem()
        {
            if (WowReader.OpenProcessAndThread(SProcess.GetProcessFromProcessName("Wow")))
            {
                Console.WriteLine("Found and Attached the World of Warcraft Process!");
                if (LoadAddresses())
                    Console.WriteLine("ok");
            }
            else
            {
                Console.WriteLine("Process not found");

            }
        }

        private Boolean LoadAddresses()
        {

            ClientConnection = WowReader.ReadUInt((uint)ClientOffsets.StaticClientConnection);
            ObjectManager = WowReader.ReadUInt((uint)(ClientConnection + ClientOffsets.ObjectManagerOffset));
            FirstObject = WowReader.ReadUInt((uint)(ObjectManager + ClientOffsets.FirstObjectOffset));
            LocalTarget.Guid = WowReader.ReadUInt64((uint)(ClientOffsets.LocalTargetGUID));
            LocalPlayer.Guid = WowReader.ReadUInt64((uint)(ObjectManager + ClientOffsets.LocalGuidOffset));

            // if the local guid is zero it means that something failed.
            if (LocalPlayer.Guid == 0)
                return false;
            else
                return true;
        }

        public Player ScanPlayer()
        {
            Player player = new Player();

            player.Guid = LocalPlayer.Guid;
            player.BaseAddress = GetObjectBaseByGuid(LocalPlayer.Guid);
            /*player.XPos = WowReader.ReadFloat((uint)(LocalPlayer.BaseAddress + ObjectOffsets.Pos_X));
            player.YPos = WowReader.ReadFloat((uint)(LocalPlayer.BaseAddress + ObjectOffsets.Pos_Y));
            player.ZPos = WowReader.ReadFloat((uint)(LocalPlayer.BaseAddress + ObjectOffsets.Pos_Z));*/
            player.Rotation = WowReader.ReadFloat((uint)(player.BaseAddress + ObjectOffsets.Rot));
            player.UnitFieldsAddress = WowReader.ReadUInt((uint)(player.BaseAddress + ObjectOffsets.UnitFields));
            player.CurrentHealth = WowReader.ReadUInt((uint)(player.UnitFieldsAddress + UnitOffsets.Health));
            player.CurrentEnergy = WowReader.ReadUInt((uint)(player.UnitFieldsAddress + UnitOffsets.Energy));
            player.MaxHealth = WowReader.ReadUInt((uint)(player.UnitFieldsAddress + UnitOffsets.MaxHealth));
            player.Level = WowReader.ReadUInt((uint)(player.UnitFieldsAddress + UnitOffsets.Level));
            player.MaxEnergy = WowReader.ReadUInt((uint)(player.UnitFieldsAddress + UnitOffsets.MaxEnergy));
            player.MapId = WowReader.ReadUInt((uint)ObjectOffsets.mapId);
            player.Name = PlayerNameFromGuid(LocalPlayer.Guid);
            player.Set(WowReader.ReadFloat((uint)(player.BaseAddress + ObjectOffsets.Pos_X)), WowReader.ReadFloat((uint)(player.BaseAddress + ObjectOffsets.Pos_Y)), WowReader.ReadFloat((uint)(player.BaseAddress + ObjectOffsets.Pos_Z)));
            //player.majPosition();
            if (player.CurrentHealth <= 0) { player.isDead = true; }

            return player;
        }

        public Player ScanTarget()
        {
            Player target = new Player();

            target.Guid = WowReader.ReadUInt64((uint)(ClientOffsets.LocalTargetGUID));

            if (target.Guid != 0)
            {
                target.BaseAddress = GetObjectBaseByGuid(target.Guid);
                /*target.XPos = WowReader.ReadFloat((uint)(LocalTarget.BaseAddress + ObjectOffsets.Pos_X));
                target.YPos = WowReader.ReadFloat((uint)(LocalTarget.BaseAddress + ObjectOffsets.Pos_Y));
                target.ZPos = WowReader.ReadFloat((uint)(LocalTarget.BaseAddress + ObjectOffsets.Pos_Z));*/
                target.Type = (short)WowReader.ReadUInt((uint)(target.BaseAddress + ObjectOffsets.Type));
                target.Rotation = WowReader.ReadFloat((uint)(target.BaseAddress + ObjectOffsets.Rot));
                target.UnitFieldsAddress = WowReader.ReadUInt((uint)(target.BaseAddress + ObjectOffsets.UnitFields));
                target.CurrentHealth = WowReader.ReadUInt((uint)(target.UnitFieldsAddress + UnitOffsets.Health));
                target.CurrentEnergy = WowReader.ReadUInt((uint)(target.UnitFieldsAddress + UnitOffsets.Energy));
                target.MaxHealth = WowReader.ReadUInt((uint)(target.UnitFieldsAddress + UnitOffsets.MaxHealth));
                target.Level = WowReader.ReadUInt((uint)(target.UnitFieldsAddress + UnitOffsets.Level));
                target.SummonedBy = WowReader.ReadUInt64((uint)(target.UnitFieldsAddress + UnitOffsets.SummonedBy));
                target.MaxEnergy = WowReader.ReadUInt((uint)(target.UnitFieldsAddress + UnitOffsets.MaxEnergy));
                target.Set(WowReader.ReadFloat((uint)(target.BaseAddress + ObjectOffsets.Pos_X)), WowReader.ReadFloat((uint)(target.BaseAddress + ObjectOffsets.Pos_Y)), WowReader.ReadFloat((uint)(target.BaseAddress + ObjectOffsets.Pos_Z)));

                if (target.Type == 3) // not a human player
                    target.Name = MobNameFromGuid(target.Guid);
                if (target.Type == 4) // a human player
                    target.Name = PlayerNameFromGuid(target.Guid);
                if (target.CurrentHealth <= 0) { target.isDead = true; }

                target.targeted = true;
                
                //we don't add LocalTarget to the ArrayList because he or she will appear again later
            }

            return target;
        }

        public ArrayList ScanListObject()
        {
            ArrayList ListObject = new ArrayList();
            Player manyObject = new Player();

            manyObject.BaseAddress = FirstObject;

            while (manyObject.BaseAddress != 0 && manyObject.BaseAddress % 2 == 0)
            {
                manyObject.Type = (short)(WowReader.ReadUInt((uint)(manyObject.BaseAddress + ObjectOffsets.Type)));

                if (manyObject.Type == 4)
                {
                    manyObject.UnitFieldsAddress = WowReader.ReadUInt((uint)(manyObject.BaseAddress + ObjectOffsets.UnitFields));
                    manyObject.CurrentHealth = WowReader.ReadUInt((uint)(manyObject.UnitFieldsAddress + UnitOffsets.Health));
                    //manyObject.CurrentEnergy = WowReader.ReadUInt((uint)(LocalPlayer.UnitFieldsAddress + UnitOffsets.Energy));
                    manyObject.MaxHealth = WowReader.ReadUInt((uint)(manyObject.UnitFieldsAddress + UnitOffsets.MaxHealth));
                    /*manyObject.XPos = WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_X));
                    manyObject.YPos = WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_Y));
                    manyObject.ZPos = WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_Z));*/
                    manyObject.Rotation = WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Rot));
                    manyObject.Guid = WowReader.ReadUInt64((uint)(manyObject.BaseAddress + ObjectOffsets.Guid));
                    manyObject.Level = WowReader.ReadUInt((uint)(manyObject.UnitFieldsAddress + UnitOffsets.Level));
                    manyObject.MaxEnergy = WowReader.ReadUInt((uint)(manyObject.UnitFieldsAddress + UnitOffsets.MaxEnergy));
                    manyObject.Name = PlayerNameFromGuid(manyObject.Guid);
                    manyObject.Set(WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_X)), WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_Y)), WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_Z)));
                    // check to see whether this player is dead or not
                    if (manyObject.CurrentHealth <= 0)
                    {
                        manyObject.isDead = true;
                    }

                    manyObject.targeted = false;

                    ListObject.Add((Player)manyObject.Clone());
                }
                if (manyObject.Type == 3)
                {
                    manyObject.UnitFieldsAddress = WowReader.ReadUInt((uint)(manyObject.BaseAddress + ObjectOffsets.UnitFields));
                    manyObject.CurrentHealth = WowReader.ReadUInt((uint)(manyObject.UnitFieldsAddress + UnitOffsets.Health));
                    //CurrentObject.CurrentEnergy = WowReader.ReadUInt((uint)(LocalPlayer.UnitFieldsAddress + UnitOffsets.Energy));
                    manyObject.MaxHealth = WowReader.ReadUInt((uint)(manyObject.UnitFieldsAddress + UnitOffsets.MaxHealth));
                    /*manyObject.XPos = WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_X));
                    manyObject.YPos = WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_Y));
                    manyObject.ZPos = WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_Z));*/
                    manyObject.Rotation = WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Rot));
                    manyObject.Guid = WowReader.ReadUInt64((uint)(manyObject.BaseAddress + ObjectOffsets.Guid));
                    manyObject.Level = WowReader.ReadUInt((uint)(manyObject.UnitFieldsAddress + UnitOffsets.Level));
                    manyObject.MaxEnergy = WowReader.ReadUInt((uint)(manyObject.UnitFieldsAddress + UnitOffsets.MaxEnergy));
                    manyObject.Name = MobNameFromGuid(manyObject.Guid);
                    manyObject.Set(WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_X)), WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_Y)), WowReader.ReadFloat((uint)(manyObject.BaseAddress + ObjectOffsets.Pos_Z)));
                    // check to see whether this player is dead or not
                    if (manyObject.CurrentHealth <= 0)
                    {
                        manyObject.isDead = true;
                    }

                    manyObject.targeted = false;

                    ListObject.Add((Player)manyObject.Clone());
                }
                // set the current object as the next object in the object manager
                manyObject.BaseAddress = WowReader.ReadUInt((uint)(manyObject.BaseAddress + ClientOffsets.NextObjectOffset));
            }

            return ListObject;
        }

        /*public void ScanObject()
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
            LocalPlayer.MapId = WowReader.ReadUInt((uint)ObjectOffsets.mapId);
            LocalPlayer.Name = PlayerNameFromGuid(LocalPlayer.Guid);
            //LocalPlayer.majPosition();
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
                //LocalTarget.majPosition();

                if (LocalTarget.Type == 3) // not a human player
                    LocalTarget.Name = MobNameFromGuid(LocalTarget.Guid);
                if (LocalTarget.Type == 4) // a human player
                    LocalTarget.Name = PlayerNameFromGuid(LocalTarget.Guid);
                if (LocalTarget.CurrentHealth <= 0) { LocalTarget.isDead = true; }

                LocalTarget.targeted = true;

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

                    CurrentObject.targeted = false;

                    CurrentPlayers.Add((Player)CurrentObject.Clone());
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

                    CurrentObject.targeted = false;

                    CurrentPlayers.Add((Player)CurrentObject.Clone());
                }
                // set the current object as the next object in the object manager
                CurrentObject.BaseAddress = WowReader.ReadUInt((uint)(CurrentObject.BaseAddress + ClientOffsets.NextObjectOffset));
            }
        }*/

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

        public Player GetPlayer()
        {
            return LocalPlayer;
        }

        public Player GetTarget()
        {
            return LocalTarget;
        }

        public ArrayList GetListObject()
        {
            return CurrentPlayers;
        }

        public string GetNamePlayer()
        {
            return LocalPlayer.Name;
        }
        public uint GetLevelPlayer()
        {
            return LocalPlayer.Level;
        }
        public string GetLifePlayer()
        {
            return LocalPlayer.CurrentHealth.ToString() + "/" + LocalPlayer.MaxHealth.ToString();
        }
        public string GetEnergyPlayer()
        {
            return LocalPlayer.CurrentEnergy.ToString() + "/" + LocalPlayer.MaxEnergy.ToString();
        }
    }
}
