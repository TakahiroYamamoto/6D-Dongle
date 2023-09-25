namespace hasp_rehost
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
            this.ButtonAttach = new System.Windows.Forms.Button();
            this.SaveH2H = new System.Windows.Forms.SaveFileDialog();
            this.ButtonRehost = new System.Windows.Forms.Button();
            this.RadioRemote = new System.Windows.Forms.RadioButton();
            this.RadioLocal = new System.Windows.Forms.RadioButton();
            this.ComboHaspId = new System.Windows.Forms.ComboBox();
            this.OpenH2H = new System.Windows.Forms.OpenFileDialog();
            this.ComboRemoteDestination = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonAttach
            // 
            this.ButtonAttach.Location = new System.Drawing.Point(150, 43);
            this.ButtonAttach.Name = "ButtonAttach";
            this.ButtonAttach.Size = new System.Drawing.Size(138, 28);
            this.ButtonAttach.TabIndex = 62;
            this.ButtonAttach.Text = "Apply License";
            this.ButtonAttach.UseVisualStyleBackColor = true;
            this.ButtonAttach.Click += new System.EventHandler(this.ButtonAttach_Click);
            // 
            // SaveH2H
            // 
            this.SaveH2H.DefaultExt = "h2h";
            this.SaveH2H.FileName = "rehost_sample.h2h";
            this.SaveH2H.Filter = "h2h File|*.h2h|All files|*.*";
            this.SaveH2H.Title = "Save H2H file";
            // 
            // ButtonRehost
            // 
            this.ButtonRehost.Location = new System.Drawing.Point(154, 174);
            this.ButtonRehost.Name = "ButtonRehost";
            this.ButtonRehost.Size = new System.Drawing.Size(135, 27);
            this.ButtonRehost.TabIndex = 60;
            this.ButtonRehost.Text = "Rehost License";
            this.ButtonRehost.UseVisualStyleBackColor = true;
            this.ButtonRehost.Click += new System.EventHandler(this.ButtonRehost_Click);
            // 
            // RadioRemote
            // 
            this.RadioRemote.AutoSize = true;
            this.RadioRemote.Location = new System.Drawing.Point(6, 113);
            this.RadioRemote.Name = "RadioRemote";
            this.RadioRemote.Size = new System.Drawing.Size(108, 17);
            this.RadioRemote.TabIndex = 55;
            this.RadioRemote.Text = "Remote recipient:";
            this.RadioRemote.UseVisualStyleBackColor = true;
            this.RadioRemote.CheckedChanged += new System.EventHandler(this.RadioRemote_CheckedChanged_1);
            // 
            // RadioLocal
            // 
            this.RadioLocal.AutoSize = true;
            this.RadioLocal.Checked = true;
            this.RadioLocal.Location = new System.Drawing.Point(6, 90);
            this.RadioLocal.Name = "RadioLocal";
            this.RadioLocal.Size = new System.Drawing.Size(188, 17);
            this.RadioLocal.TabIndex = 54;
            this.RadioLocal.TabStop = true;
            this.RadioLocal.Text = "Local recipient (test purposes only)";
            this.RadioLocal.UseVisualStyleBackColor = true;
            // 
            // ComboHaspId
            // 
            this.ComboHaspId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboHaspId.FormattingEnabled = true;
            this.ComboHaspId.Location = new System.Drawing.Point(224, 36);
            this.ComboHaspId.Name = "ComboHaspId";
            this.ComboHaspId.Size = new System.Drawing.Size(267, 21);
            this.ComboHaspId.TabIndex = 53;
            this.ComboHaspId.SelectedIndexChanged += new System.EventHandler(this.ComboHaspId_SelectedIndexChanged);
            // 
            // OpenH2H
            // 
            this.OpenH2H.DefaultExt = "h2h";
            this.OpenH2H.FileName = "rehost_sample.h2h";
            this.OpenH2H.Filter = "h2h File|*.h2h|All files|*.*";
            this.OpenH2H.Title = "Open H2H file";
            // 
            // ComboRemoteDestination
            // 
            this.ComboRemoteDestination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboRemoteDestination.Enabled = false;
            this.ComboRemoteDestination.FormattingEnabled = true;
            this.ComboRemoteDestination.Location = new System.Drawing.Point(224, 109);
            this.ComboRemoteDestination.Name = "ComboRemoteDestination";
            this.ComboRemoteDestination.Size = new System.Drawing.Size(267, 21);
            this.ComboRemoteDestination.TabIndex = 56;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 52;
            this.label1.Text = "Select Key Id";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ComboHaspId);
            this.groupBox1.Controls.Add(this.ButtonRehost);
            this.groupBox1.Controls.Add(this.RadioLocal);
            this.groupBox1.Controls.Add(this.ComboRemoteDestination);
            this.groupBox1.Controls.Add(this.RadioRemote);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(511, 220);
            this.groupBox1.TabIndex = 63;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Host";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ButtonAttach);
            this.groupBox2.Location = new System.Drawing.Point(13, 238);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(510, 108);
            this.groupBox2.TabIndex = 64;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recipient";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 358);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sentinel LDK Rehost License Sample ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonAttach;
        private System.Windows.Forms.SaveFileDialog SaveH2H;
        private System.Windows.Forms.Button ButtonRehost;
        private System.Windows.Forms.RadioButton RadioRemote;
        private System.Windows.Forms.RadioButton RadioLocal;
        private System.Windows.Forms.ComboBox ComboHaspId;
        private System.Windows.Forms.OpenFileDialog OpenH2H;
        private System.Windows.Forms.ComboBox ComboRemoteDestination;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

