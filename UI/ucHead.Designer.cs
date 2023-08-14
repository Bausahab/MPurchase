
namespace MPurchase.UI
{
    partial class ucHead
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucHead));
            this.lblAcYear = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblDyce = new System.Windows.Forms.Label();
            this.lblDtCurr = new System.Windows.Forms.Label();
            this.lblLastpurDtl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tbTrNo = new System.Windows.Forms.TextBox();
            this.tbLastPurEntryNo = new System.Windows.Forms.TextBox();
            this.tbTransprt = new System.Windows.Forms.TextBox();
            this.dtpDtCurr = new System.Windows.Forms.DateTimePicker();
            this.tbGrNo = new System.Windows.Forms.TextBox();
            this.tbAc = new System.Windows.Forms.TextBox();
            this.lbllastpurchaseinterval = new System.Windows.Forms.Label();
            this.brnViewDtls = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAcYear
            // 
            this.lblAcYear.AutoSize = true;
            this.lblAcYear.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAcYear.Location = new System.Drawing.Point(3, 0);
            this.lblAcYear.Name = "lblAcYear";
            this.lblAcYear.Size = new System.Drawing.Size(108, 38);
            this.lblAcYear.TabIndex = 0;
            this.lblAcYear.Text = "Last Purchase Entry No:";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(237, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(55, 19);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "Tr. No.";
            // 
            // lblDyce
            // 
            this.lblDyce.AutoSize = true;
            this.lblDyce.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDyce.Location = new System.Drawing.Point(3, 73);
            this.lblDyce.Name = "lblDyce";
            this.lblDyce.Size = new System.Drawing.Size(47, 19);
            this.lblDyce.TabIndex = 2;
            this.lblDyce.Text = "Date:";
            // 
            // lblDtCurr
            // 
            this.lblDtCurr.AutoSize = true;
            this.lblDtCurr.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDtCurr.Location = new System.Drawing.Point(507, 0);
            this.lblDtCurr.Name = "lblDtCurr";
            this.lblDtCurr.Size = new System.Drawing.Size(67, 19);
            this.lblDtCurr.TabIndex = 3;
            this.lblDtCurr.Text = "Gr. No. :";
            // 
            // lblLastpurDtl
            // 
            this.lblLastpurDtl.AutoSize = true;
            this.lblLastpurDtl.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastpurDtl.Location = new System.Drawing.Point(3, 105);
            this.lblLastpurDtl.Name = "lblLastpurDtl";
            this.lblLastpurDtl.Size = new System.Drawing.Size(74, 19);
            this.lblLastpurDtl.TabIndex = 4;
            this.lblLastpurDtl.Text = "Transport";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(237, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "OrgA/C Frt.";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 7;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5323F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.28424F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.560723F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.37984F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.560723F));
            this.tableLayoutPanelMain.Controls.Add(this.tbTrNo, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.lblAcYear, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.lblDyce, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.lblLastpurDtl, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.lblUser, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tbLastPurEntryNo, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tbTransprt, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.dtpDtCurr, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tbGrNo, 5, 0);
            this.tableLayoutPanelMain.Controls.Add(this.label1, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tbAc, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.lbllastpurchaseinterval, 3, 2);
            this.tableLayoutPanelMain.Controls.Add(this.brnViewDtls, 5, 2);
            this.tableLayoutPanelMain.Controls.Add(this.btnReload, 6, 0);
            this.tableLayoutPanelMain.Controls.Add(this.lblDtCurr, 4, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.73874F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.27749F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.50262F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(824, 191);
            this.tableLayoutPanelMain.TabIndex = 6;
            // 
            // tbTrNo
            // 
            this.tbTrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTrNo.Location = new System.Drawing.Point(340, 3);
            this.tbTrNo.Name = "tbTrNo";
            this.tbTrNo.Size = new System.Drawing.Size(100, 22);
            this.tbTrNo.TabIndex = 7;
            // 
            // tbLastPurEntryNo
            // 
            this.tbLastPurEntryNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbLastPurEntryNo.Location = new System.Drawing.Point(120, 3);
            this.tbLastPurEntryNo.Name = "tbLastPurEntryNo";
            this.tbLastPurEntryNo.Size = new System.Drawing.Size(100, 22);
            this.tbLastPurEntryNo.TabIndex = 6;
            // 
            // tbTransprt
            // 
            this.tbTransprt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTransprt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTransprt.Location = new System.Drawing.Point(120, 108);
            this.tbTransprt.Name = "tbTransprt";
            this.tbTransprt.Size = new System.Drawing.Size(111, 22);
            this.tbTransprt.TabIndex = 10;
            // 
            // dtpDtCurr
            // 
            this.dtpDtCurr.CustomFormat = "dd/mm/yyyy";
            this.dtpDtCurr.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDtCurr.Location = new System.Drawing.Point(120, 76);
            this.dtpDtCurr.Name = "dtpDtCurr";
            this.dtpDtCurr.Size = new System.Drawing.Size(100, 22);
            this.dtpDtCurr.TabIndex = 13;
            // 
            // tbGrNo
            // 
            this.tbGrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGrNo.Location = new System.Drawing.Point(585, 3);
            this.tbGrNo.Name = "tbGrNo";
            this.tbGrNo.Size = new System.Drawing.Size(100, 22);
            this.tbGrNo.TabIndex = 8;
            // 
            // tbAc
            // 
            this.tbAc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbAc.Location = new System.Drawing.Point(340, 76);
            this.tbAc.Name = "tbAc";
            this.tbAc.Size = new System.Drawing.Size(161, 22);
            this.tbAc.TabIndex = 9;
            // 
            // lbllastpurchaseinterval
            // 
            this.lbllastpurchaseinterval.AutoSize = true;
            this.lbllastpurchaseinterval.Location = new System.Drawing.Point(340, 105);
            this.lbllastpurchaseinterval.Name = "lbllastpurchaseinterval";
            this.lbllastpurchaseinterval.Size = new System.Drawing.Size(139, 15);
            this.lbllastpurchaseinterval.TabIndex = 14;
            this.lbllastpurchaseinterval.Text = "Last Purchase { #} Ago..";
            // 
            // brnViewDtls
            // 
            this.brnViewDtls.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.brnViewDtls.Location = new System.Drawing.Point(585, 108);
            this.brnViewDtls.Name = "brnViewDtls";
            this.brnViewDtls.Size = new System.Drawing.Size(114, 26);
            this.brnViewDtls.TabIndex = 15;
            this.brnViewDtls.Text = "View Details";
            this.brnViewDtls.UseVisualStyleBackColor = true;
            // 
            // btnReload
            // 
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.Image = ((System.Drawing.Image)(resources.GetObject("btnReload.Image")));
            this.btnReload.Location = new System.Drawing.Point(744, 3);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(68, 67);
            this.btnReload.TabIndex = 16;
            this.btnReload.UseVisualStyleBackColor = true;
            // 
            // ucHead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ucHead";
            this.Size = new System.Drawing.Size(824, 191);
            this.Tag = "HeadCtrl";
            this.Load += new System.EventHandler(this.ucHead_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAcYear;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblDyce;
        private System.Windows.Forms.Label lblDtCurr;
        private System.Windows.Forms.Label lblLastpurDtl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TextBox tbTrNo;
        private System.Windows.Forms.TextBox tbLastPurEntryNo;
        private System.Windows.Forms.TextBox tbGrNo;
        private System.Windows.Forms.TextBox tbAc;
        private System.Windows.Forms.TextBox tbTransprt;
        private System.Windows.Forms.DateTimePicker dtpDtCurr;
        private System.Windows.Forms.Label lbllastpurchaseinterval;
        private System.Windows.Forms.Button brnViewDtls;
        private System.Windows.Forms.Button btnReload;
    }
}
