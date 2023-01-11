namespace Traffic_Signal
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GreenStopLight = new System.Windows.Forms.PictureBox();
            this.YellowStopLight = new System.Windows.Forms.PictureBox();
            this.RedStopLight = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GreenStopLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YellowStopLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedStopLight)).BeginInit();
            this.SuspendLayout();
            // 
            // GreenStopLight
            // 
            this.GreenStopLight.Image = global::Traffic_Signal.Properties.Resources.IMG_20221227_095035;
            this.GreenStopLight.Location = new System.Drawing.Point(222, 12);
            this.GreenStopLight.Name = "GreenStopLight";
            this.GreenStopLight.Size = new System.Drawing.Size(137, 381);
            this.GreenStopLight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.GreenStopLight.TabIndex = 2;
            this.GreenStopLight.TabStop = false;
            // 
            // YellowStopLight
            // 
            this.YellowStopLight.Image = global::Traffic_Signal.Properties.Resources.IMG_20221227_095016;
            this.YellowStopLight.Location = new System.Drawing.Point(222, 12);
            this.YellowStopLight.Name = "YellowStopLight";
            this.YellowStopLight.Size = new System.Drawing.Size(137, 381);
            this.YellowStopLight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.YellowStopLight.TabIndex = 1;
            this.YellowStopLight.TabStop = false;
            // 
            // RedStopLight
            // 
            this.RedStopLight.Image = global::Traffic_Signal.Properties.Resources.IMG_20221227_094956;
            this.RedStopLight.Location = new System.Drawing.Point(222, 12);
            this.RedStopLight.Name = "RedStopLight";
            this.RedStopLight.Size = new System.Drawing.Size(137, 381);
            this.RedStopLight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.RedStopLight.TabIndex = 0;
            this.RedStopLight.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(467, 65);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(115, 35);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(467, 229);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(115, 35);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.GreenStopLight);
            this.Controls.Add(this.YellowStopLight);
            this.Controls.Add(this.RedStopLight);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.GreenStopLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YellowStopLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedStopLight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox RedStopLight;
        private System.Windows.Forms.PictureBox YellowStopLight;
        private System.Windows.Forms.PictureBox GreenStopLight;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Timer timer1;
    }
}

