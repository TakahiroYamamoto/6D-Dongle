namespace DongleManager
{
    partial class LicenseEditor
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
            this.panelLicenseForBurn = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelControl = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panelAllLicense = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.licenseListCtlAll = new DongleManagerLib.LicenseListCtl();
            this.licenseListCtlForBurn = new DongleManagerLib.LicenseListCtl();
            this.panelLicenseForBurn.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelControl.SuspendLayout();
            this.panelAllLicense.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLicenseForBurn
            // 
            this.panelLicenseForBurn.Controls.Add(this.licenseListCtlForBurn);
            this.panelLicenseForBurn.Controls.Add(this.panel2);
            this.panelLicenseForBurn.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLicenseForBurn.Location = new System.Drawing.Point(0, 0);
            this.panelLicenseForBurn.Name = "panelLicenseForBurn";
            this.panelLicenseForBurn.Size = new System.Drawing.Size(525, 181);
            this.panelLicenseForBurn.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(525, 22);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(10, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "License for buning";
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.btnDelete);
            this.panelControl.Controls.Add(this.btnAdd);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl.Location = new System.Drawing.Point(0, 184);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(525, 43);
            this.panelControl.TabIndex = 2;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(84, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 30);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(44, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 181);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(525, 3);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // panelAllLicense
            // 
            this.panelAllLicense.Controls.Add(this.licenseListCtlAll);
            this.panelAllLicense.Controls.Add(this.panel5);
            this.panelAllLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAllLicense.Location = new System.Drawing.Point(0, 227);
            this.panelAllLicense.Name = "panelAllLicense";
            this.panelAllLicense.Size = new System.Drawing.Size(525, 194);
            this.panelAllLicense.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(525, 22);
            this.panel5.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(10, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "All License";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 421);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(525, 31);
            this.panel1.TabIndex = 5;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(137, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(309, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // licenseListCtlAll
            // 
            this.licenseListCtlAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.licenseListCtlAll.Location = new System.Drawing.Point(0, 22);
            this.licenseListCtlAll.Name = "licenseListCtlAll";
            this.licenseListCtlAll.Size = new System.Drawing.Size(525, 172);
            this.licenseListCtlAll.TabIndex = 0;
            // 
            // licenseListCtlForBurn
            // 
            this.licenseListCtlForBurn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.licenseListCtlForBurn.Location = new System.Drawing.Point(0, 22);
            this.licenseListCtlForBurn.Name = "licenseListCtlForBurn";
            this.licenseListCtlForBurn.Size = new System.Drawing.Size(525, 159);
            this.licenseListCtlForBurn.TabIndex = 0;
            // 
            // LicenseEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 452);
            this.Controls.Add(this.panelAllLicense);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panelLicenseForBurn);
            this.Name = "LicenseEditor";
            this.Text = "License Editor";
            this.panelLicenseForBurn.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelControl.ResumeLayout(false);
            this.panelAllLicense.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DongleManagerLib.LicenseListCtl licenseListCtlForBurn;
        private System.Windows.Forms.Panel panelLicenseForBurn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelAllLicense;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label2;
        private DongleManagerLib.LicenseListCtl licenseListCtlAll;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}