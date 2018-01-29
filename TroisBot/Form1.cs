using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBCStoresCLI;
using DetourCLI;
using MapCLI;
using TroisBot.Memory;
using TroisBot.Properties;
using TroisBot.Units;
using VMapCLI;

namespace TroisBot
{
    public partial class Form1 : Form
    {
        Bitmap DrawArea;

        ManageMem mem = new ManageMem();

        public Form1()
        {
            InitializeComponent();

            //Initialise maps and dbc
            Detour.Initialize(Settings.Default.mmaps);
            Map.Initialize(Settings.Default.maps);
            VMap.Initialize(Settings.Default.vmaps);
            DBCStores.Initialize(Settings.Default.dbc);

            DrawArea = new Bitmap(260, 260);
            pictureBox_maps.Image = DrawArea;

            timer_ScanInfo.Interval = 100; 
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            timer_ScanInfo.Start();
        }

        private void timer_ScanInfo_Tick(object sender, EventArgs e)
        {
            ClearBitmap(ref DrawArea);

            mem.ScanObject();

            label_name.Text = mem.GetNamePlayer();
            label_level.Text = mem.GetLevelPlayer().ToString();
            label_vie.Text = mem.GetLifePlayer();
            label_energie.Text = mem.GetEnergyPlayer();

            pictureBox_maps.Image = mem.test(DrawArea);
        }

        private void ClearBitmap(ref Bitmap img)
        {
            Graphics G = Graphics.FromImage(img);
            G.Clear(Color.White);
            G.Dispose();
        }

        private void button_move_Click(object sender, EventArgs e)
        {
            mem.testmoveAsync();
        }

        private void MajPlayerInfo(Player player)
        {
            label_name.Text = player.Name;
            label_level.Text = player.Level.ToString();
            label_vie.Text = player.CurrentHealth.ToString() + "/" + player.MaxHealth.ToString();
            label_energie.Text = player.CurrentEnergy.ToString() + "/" + player.MaxEnergy.ToString();
        }

        private void MajTargetInfo(Player target)
        {
            label_target_name.Text = target.Name;
            label_target_level.Text = target.Level.ToString();
            label_target_vie.Text = target.CurrentHealth.ToString() + "/" + target.MaxHealth.ToString();
        }
    }
}
