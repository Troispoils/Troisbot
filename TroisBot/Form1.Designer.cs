namespace TroisBot
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox_info_Player = new System.Windows.Forms.GroupBox();
            this.label_energie = new System.Windows.Forms.Label();
            this.label_vie = new System.Windows.Forms.Label();
            this.label_level = new System.Windows.Forms.Label();
            this.label_name = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_info_Target = new System.Windows.Forms.GroupBox();
            this.pictureBox_maps = new System.Windows.Forms.PictureBox();
            this.button_start = new System.Windows.Forms.Button();
            this.timer_ScanInfo = new System.Windows.Forms.Timer(this.components);
            this.button_move = new System.Windows.Forms.Button();
            this.groupBox_info_Player.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_maps)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_info_Player
            // 
            this.groupBox_info_Player.Controls.Add(this.label_energie);
            this.groupBox_info_Player.Controls.Add(this.label_vie);
            this.groupBox_info_Player.Controls.Add(this.label_level);
            this.groupBox_info_Player.Controls.Add(this.label_name);
            this.groupBox_info_Player.Controls.Add(this.label4);
            this.groupBox_info_Player.Controls.Add(this.label3);
            this.groupBox_info_Player.Controls.Add(this.label2);
            this.groupBox_info_Player.Controls.Add(this.label1);
            this.groupBox_info_Player.Location = new System.Drawing.Point(12, 12);
            this.groupBox_info_Player.Name = "groupBox_info_Player";
            this.groupBox_info_Player.Size = new System.Drawing.Size(260, 100);
            this.groupBox_info_Player.TabIndex = 0;
            this.groupBox_info_Player.TabStop = false;
            this.groupBox_info_Player.Text = "Player Info";
            // 
            // label_energie
            // 
            this.label_energie.AutoSize = true;
            this.label_energie.Location = new System.Drawing.Point(77, 71);
            this.label_energie.Name = "label_energie";
            this.label_energie.Size = new System.Drawing.Size(35, 13);
            this.label_energie.TabIndex = 7;
            this.label_energie.Text = "label8";
            // 
            // label_vie
            // 
            this.label_vie.AutoSize = true;
            this.label_vie.Location = new System.Drawing.Point(77, 54);
            this.label_vie.Name = "label_vie";
            this.label_vie.Size = new System.Drawing.Size(35, 13);
            this.label_vie.TabIndex = 6;
            this.label_vie.Text = "label7";
            // 
            // label_level
            // 
            this.label_level.AutoSize = true;
            this.label_level.Location = new System.Drawing.Point(77, 37);
            this.label_level.Name = "label_level";
            this.label_level.Size = new System.Drawing.Size(35, 13);
            this.label_level.TabIndex = 5;
            this.label_level.Text = "label6";
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Location = new System.Drawing.Point(77, 19);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(35, 13);
            this.label_name.TabIndex = 4;
            this.label_name.Text = "label5";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Energie : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Vie : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Level : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name : ";
            // 
            // groupBox_info_Target
            // 
            this.groupBox_info_Target.Location = new System.Drawing.Point(13, 119);
            this.groupBox_info_Target.Name = "groupBox_info_Target";
            this.groupBox_info_Target.Size = new System.Drawing.Size(259, 100);
            this.groupBox_info_Target.TabIndex = 1;
            this.groupBox_info_Target.TabStop = false;
            this.groupBox_info_Target.Text = "Target Info";
            // 
            // pictureBox_maps
            // 
            this.pictureBox_maps.BackColor = System.Drawing.Color.White;
            this.pictureBox_maps.Location = new System.Drawing.Point(12, 254);
            this.pictureBox_maps.Name = "pictureBox_maps";
            this.pictureBox_maps.Size = new System.Drawing.Size(260, 260);
            this.pictureBox_maps.TabIndex = 2;
            this.pictureBox_maps.TabStop = false;
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(197, 225);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 3;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // timer_ScanInfo
            // 
            this.timer_ScanInfo.Tick += new System.EventHandler(this.timer_ScanInfo_Tick);
            // 
            // button_move
            // 
            this.button_move.Location = new System.Drawing.Point(116, 224);
            this.button_move.Name = "button_move";
            this.button_move.Size = new System.Drawing.Size(75, 23);
            this.button_move.TabIndex = 4;
            this.button_move.Text = "Test_Move";
            this.button_move.UseVisualStyleBackColor = true;
            this.button_move.Click += new System.EventHandler(this.button_move_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 527);
            this.Controls.Add(this.button_move);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.pictureBox_maps);
            this.Controls.Add(this.groupBox_info_Target);
            this.Controls.Add(this.groupBox_info_Player);
            this.Name = "Form1";
            this.Text = "TroisBot";
            this.groupBox_info_Player.ResumeLayout(false);
            this.groupBox_info_Player.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_maps)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_info_Player;
        private System.Windows.Forms.GroupBox groupBox_info_Target;
        private System.Windows.Forms.PictureBox pictureBox_maps;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Label label_energie;
        private System.Windows.Forms.Label label_vie;
        private System.Windows.Forms.Label label_level;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer_ScanInfo;
        private System.Windows.Forms.Button button_move;
    }
}

