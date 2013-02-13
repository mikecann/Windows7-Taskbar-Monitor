namespace TaskbarSystemMonitor
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._netBut = new System.Windows.Forms.Button();
            this._memBut = new System.Windows.Forms.Button();
            this._cpuBut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(115, 328);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(86, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "by Michael Cann";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 328);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "v0.1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TaskbarSystemMonitor.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(29, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(161, 90);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // _netBut
            // 
            this._netBut.ForeColor = System.Drawing.SystemColors.InfoText;
            this._netBut.Image = global::TaskbarSystemMonitor.Properties.Resources.Network_icon;
            this._netBut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._netBut.Location = new System.Drawing.Point(31, 247);
            this._netBut.Name = "_netBut";
            this._netBut.Size = new System.Drawing.Size(142, 57);
            this._netBut.TabIndex = 5;
            this._netBut.Text = "NETWORK";
            this._netBut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._netBut.UseVisualStyleBackColor = true;
            this._netBut.Click += new System.EventHandler(this._netBut_Click);
            // 
            // _memBut
            // 
            this._memBut.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._memBut.Image = global::TaskbarSystemMonitor.Properties.Resources.mem;
            this._memBut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._memBut.Location = new System.Drawing.Point(31, 184);
            this._memBut.Name = "_memBut";
            this._memBut.Size = new System.Drawing.Size(142, 57);
            this._memBut.TabIndex = 4;
            this._memBut.Text = "MEMORY";
            this._memBut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._memBut.UseVisualStyleBackColor = true;
            this._memBut.Click += new System.EventHandler(this._memBut_Click);
            // 
            // _cpuBut
            // 
            this._cpuBut.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._cpuBut.Image = global::TaskbarSystemMonitor.Properties.Resources.cpu;
            this._cpuBut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._cpuBut.Location = new System.Drawing.Point(31, 121);
            this._cpuBut.Name = "_cpuBut";
            this._cpuBut.Size = new System.Drawing.Size(142, 57);
            this._cpuBut.TabIndex = 3;
            this._cpuBut.Text = "CPU";
            this._cpuBut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._cpuBut.UseVisualStyleBackColor = true;
            this._cpuBut.Click += new System.EventHandler(this._cpuBut_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 349);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this._netBut);
            this.Controls.Add(this._memBut);
            this.Controls.Add(this._cpuBut);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "CPU %";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _cpuBut;
        private System.Windows.Forms.Button _memBut;
        private System.Windows.Forms.Button _netBut;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;









    }
}

