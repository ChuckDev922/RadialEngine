namespace howto_piston
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
            this.picCanvas = new System.Windows.Forms.PictureBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.tmrMovePiston = new System.Windows.Forms.Timer(this.components);
            this.btnPause = new System.Windows.Forms.Button();
            this.txtScrollTimer = new System.Windows.Forms.TextBox();
            this.scrTimer = new System.Windows.Forms.HScrollBar();
            this.gbSpeed = new System.Windows.Forms.GroupBox();
            this.lblVelocity1 = new System.Windows.Forms.Label();
            this.lblVelocity2 = new System.Windows.Forms.Label();
            this.lblVelocity4 = new System.Windows.Forms.Label();
            this.lblVelocity3 = new System.Windows.Forms.Label();
            this.lblVelocity5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).BeginInit();
            this.gbSpeed.SuspendLayout();
            this.SuspendLayout();
            // 
            // picCanvas
            // 
            this.picCanvas.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picCanvas.BackColor = System.Drawing.SystemColors.Control;
            this.picCanvas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCanvas.Location = new System.Drawing.Point(244, 12);
            this.picCanvas.Name = "picCanvas";
            this.picCanvas.Size = new System.Drawing.Size(400, 400);
            this.picCanvas.TabIndex = 28;
            this.picCanvas.TabStop = false;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(6, 5);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(78, 23);
            this.btnStartStop.TabIndex = 25;
            this.btnStartStop.Text = "Stop";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // tmrMovePiston
            // 
            this.tmrMovePiston.Enabled = true;
            this.tmrMovePiston.Interval = 1000;
            this.tmrMovePiston.Tick += new System.EventHandler(this.tmrMovePiston_Tick);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(6, 50);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(78, 23);
            this.btnPause.TabIndex = 29;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // txtScrollTimer
            // 
            this.txtScrollTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtScrollTimer.Location = new System.Drawing.Point(173, 24);
            this.txtScrollTimer.Name = "txtScrollTimer";
            this.txtScrollTimer.ReadOnly = true;
            this.txtScrollTimer.Size = new System.Drawing.Size(41, 20);
            this.txtScrollTimer.TabIndex = 61;
            this.txtScrollTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // scrTimer
            // 
            this.scrTimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scrTimer.Location = new System.Drawing.Point(2, 24);
            this.scrTimer.Maximum = 2000;
            this.scrTimer.Minimum = 100;
            this.scrTimer.Name = "scrTimer";
            this.scrTimer.Size = new System.Drawing.Size(157, 17);
            this.scrTimer.TabIndex = 60;
            this.scrTimer.Value = 100;
            this.scrTimer.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrTimer_Scroll);
            // 
            // gbSpeed
            // 
            this.gbSpeed.Controls.Add(this.txtScrollTimer);
            this.gbSpeed.Controls.Add(this.scrTimer);
            this.gbSpeed.Location = new System.Drawing.Point(0, 91);
            this.gbSpeed.Name = "gbSpeed";
            this.gbSpeed.Size = new System.Drawing.Size(229, 65);
            this.gbSpeed.TabIndex = 62;
            this.gbSpeed.TabStop = false;
            this.gbSpeed.Text = "Timer Interval";
            // 
            // lblVelocity1
            // 
            this.lblVelocity1.AutoSize = true;
            this.lblVelocity1.Location = new System.Drawing.Point(12, 174);
            this.lblVelocity1.Name = "lblVelocity1";
            this.lblVelocity1.Size = new System.Drawing.Size(92, 13);
            this.lblVelocity1.TabIndex = 63;
            this.lblVelocity1.Text = "Piston 1 Velocity: 1234.5";
            // 
            // lblVelocity2
            // 
            this.lblVelocity2.AutoSize = true;
            this.lblVelocity2.Location = new System.Drawing.Point(12, 199);
            this.lblVelocity2.Name = "lblVelocity2";
            this.lblVelocity2.Size = new System.Drawing.Size(92, 13);
            this.lblVelocity2.TabIndex = 64;
            this.lblVelocity2.Text = "Piston 2 Velocity: 1234.5";
            // 
            // lblVelocity4
            // 
            this.lblVelocity4.AutoSize = true;
            this.lblVelocity4.Location = new System.Drawing.Point(12, 251);
            this.lblVelocity4.Name = "lblVelocity4";
            this.lblVelocity4.Size = new System.Drawing.Size(92, 13);
            this.lblVelocity4.TabIndex = 66;
            this.lblVelocity4.Text = "Piston 4 Velocity: 1234.5";
            // 
            // lblVelocity3
            // 
            this.lblVelocity3.AutoSize = true;
            this.lblVelocity3.Location = new System.Drawing.Point(12, 226);
            this.lblVelocity3.Name = "lblVelocity3";
            this.lblVelocity3.Size = new System.Drawing.Size(92, 13);
            this.lblVelocity3.TabIndex = 65;
            this.lblVelocity3.Text = "Piston 3 Velocity: 1234.5";
            // 
            // lblVelocity5
            // 
            this.lblVelocity5.AutoSize = true;
            this.lblVelocity5.Location = new System.Drawing.Point(12, 278);
            this.lblVelocity5.Name = "lblVelocity5";
            this.lblVelocity5.Size = new System.Drawing.Size(92, 13);
            this.lblVelocity5.TabIndex = 67;
            this.lblVelocity5.Text = "Piston 5 Velocity: 1234.5";
            // 
            // Form1
            // 
            this.AcceptButton = this.btnStartStop;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 515);
            this.Controls.Add(this.lblVelocity5);
            this.Controls.Add(this.lblVelocity4);
            this.Controls.Add(this.lblVelocity3);
            this.Controls.Add(this.lblVelocity2);
            this.Controls.Add(this.lblVelocity1);
            this.Controls.Add(this.gbSpeed);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.picCanvas);
            this.Controls.Add(this.btnStartStop);
            this.Name = "Form1";
            this.Text = "Radial Engine 10/31/2016 3pm";
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).EndInit();
            this.gbSpeed.ResumeLayout(false);
            this.gbSpeed.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picCanvas;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Timer tmrMovePiston;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.TextBox txtScrollTimer;
        private System.Windows.Forms.HScrollBar scrTimer;
        private System.Windows.Forms.GroupBox gbSpeed;
        private System.Windows.Forms.Label lblVelocity1;
        private System.Windows.Forms.Label lblVelocity2;
        private System.Windows.Forms.Label lblVelocity4;
        private System.Windows.Forms.Label lblVelocity3;
        private System.Windows.Forms.Label lblVelocity5;
    }
}

