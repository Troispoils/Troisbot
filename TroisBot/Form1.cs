using System;
using System.Collections;
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
using TroisBot.Move;
using TroisBot.Properties;
using TroisBot.Radar;
using TroisBot.Simu;
using TroisBot.Units;
using TroisBot.Units.Definition;
using VMapCLI;

namespace TroisBot
{
    public partial class Form1 : Form
    {
        Bitmap DrawArea;

        Player curPlayer = new Player();
        Player curTarget = new Player();
        ArrayList curObject = new ArrayList();

        ManageMem mem = new ManageMem();
        ManageGraph graph = new ManageGraph();
        CTM move = new CTM();
        MapCLI.Point point = new MapCLI.Point();

        bool StartBot = false;
        bool choixCible = false;
        int rand = 0;

        public Form1()
        {
            InitializeComponent();

            //Initialise maps and dbc
            Detour.Initialize(Settings.Default.mmaps);
            Map.Initialize(Settings.Default.maps);
            VMap.Initialize(Settings.Default.vmaps);
            DBCStores.Initialize(Settings.Default.dbc);

            DrawArea = new Bitmap(260, 260);
            //pictureBox_maps.Image = DrawArea;

            timer_ScanInfo.Interval = 100; 
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            timer_ScanInfo.Start();
        }

        private void timer_ScanInfo_Tick(object sender, EventArgs e)
        {
            ClearBitmap(ref DrawArea);

            curPlayer = mem.ScanPlayer();
            curTarget = mem.ScanTarget();
            curObject = mem.ScanListObject();

            MajPlayerInfo(curPlayer);
            MajTargetInfo(curTarget);

            pictureBox_maps.Image = MajGraph();

            //toolStripStatusLabel1.Text = curObject.Count.ToString();

            if(StartBot)
            {
                if(choixCible == false)
                {
                    rand = new Random().Next(0, curObject.Count);
                    Player locTarget = (Player)curObject[rand];
                    move.majCTM(curPlayer, locTarget, mem.GetBlackMagic());
                    move.StartThread();
                    choixCible = true;
                }
                else
                {
                    if (move.ThreadAlive() == true)
                    {
                        choixCible = true;
                        toolStripStatusLabel1.Text = "Thread alive";
                    }
                    else
                    {
                        choixCible = false;
                        toolStripStatusLabel1.Text = "Thread dead";
                        new KeySim().KeyDown((int)Keys.Tab);
                        new KeySim().KeyUp((int)Keys.Tab);
                    }
                }
            }
        }

        private void ClearBitmap(ref Bitmap img)
        {
            Graphics G = Graphics.FromImage(img);
            G.Clear(Color.White);
            G.Dispose();
        }

        private void button_move_Click(object sender, EventArgs e)
        {
            if (StartBot)
                StartBot = false;
            else
                StartBot = true;
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

        private Bitmap MajGraph()
        {
            Bitmap graphForm = new Bitmap(260, 260);
            foreach (Player i in curObject)
            {
                if (i.Type == 3)
                {
                    graphForm = graph.TracePlayerInMap(DrawArea, curPlayer, i, Color.Red);
                }
                else if (i.Type == 4)
                {
                    if (i.Name == curPlayer.Name)
                        graphForm = graph.TracePlayerInMap(DrawArea, curPlayer, i, Color.Blue);
                    else
                        graphForm = graph.TracePlayerInMap(DrawArea, curPlayer, i, Color.Green);
                }
            }
            if (curTarget.Guid != 0)
                graphForm = graph.TracePlayerInMap(DrawArea, curPlayer, curTarget, Color.DarkKhaki, true);
            return graphForm;
        }
    }
}
