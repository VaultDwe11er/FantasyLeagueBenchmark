namespace FantasyLeagueBenchmark
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
            this.dgvTeams = new System.Windows.Forms.DataGridView();
            this.Team = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TmCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TmTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TmIsInValid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCost = new System.Windows.Forms.DataGridView();
            this.TotalCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costIsInvalid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPercPicked = new System.Windows.Forms.DataGridView();
            this.PercPicked = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ppTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ppIsInValid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvNotPickable = new System.Windows.Forms.DataGridView();
            this.NotPickable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGetData = new System.Windows.Forms.Button();
            this.btnRunModel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSelect = new System.Windows.Forms.TextBox();
            this.btnShowData = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSite = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPercPicked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotPickable)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTeams
            // 
            this.dgvTeams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTeams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Team,
            this.TmCurrent,
            this.TmTarget,
            this.TmIsInValid});
            this.dgvTeams.Location = new System.Drawing.Point(12, 66);
            this.dgvTeams.Name = "dgvTeams";
            this.dgvTeams.RowHeadersWidth = 20;
            this.dgvTeams.Size = new System.Drawing.Size(406, 299);
            this.dgvTeams.TabIndex = 0;
            // 
            // Team
            // 
            this.Team.HeaderText = "Team";
            this.Team.Name = "Team";
            // 
            // TmCurrent
            // 
            this.TmCurrent.HeaderText = "Current";
            this.TmCurrent.Name = "TmCurrent";
            this.TmCurrent.ReadOnly = true;
            this.TmCurrent.Width = 80;
            // 
            // TmTarget
            // 
            this.TmTarget.HeaderText = "Target";
            this.TmTarget.Name = "TmTarget";
            this.TmTarget.Width = 80;
            // 
            // TmIsInValid
            // 
            this.TmIsInValid.HeaderText = "IsInValid";
            this.TmIsInValid.Name = "TmIsInValid";
            this.TmIsInValid.ReadOnly = true;
            this.TmIsInValid.Width = 80;
            // 
            // dgvCost
            // 
            this.dgvCost.AllowUserToAddRows = false;
            this.dgvCost.AllowUserToDeleteRows = false;
            this.dgvCost.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCost.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TotalCost,
            this.costTarget,
            this.costIsInvalid});
            this.dgvCost.Location = new System.Drawing.Point(424, 12);
            this.dgvCost.Name = "dgvCost";
            this.dgvCost.RowHeadersVisible = false;
            this.dgvCost.Size = new System.Drawing.Size(306, 48);
            this.dgvCost.TabIndex = 1;
            // 
            // TotalCost
            // 
            this.TotalCost.HeaderText = "Total Cost";
            this.TotalCost.Name = "TotalCost";
            this.TotalCost.ReadOnly = true;
            // 
            // costTarget
            // 
            this.costTarget.HeaderText = "Target";
            this.costTarget.Name = "costTarget";
            // 
            // costIsInvalid
            // 
            this.costIsInvalid.HeaderText = "IsInvalid";
            this.costIsInvalid.Name = "costIsInvalid";
            // 
            // dgvPercPicked
            // 
            this.dgvPercPicked.AllowUserToAddRows = false;
            this.dgvPercPicked.AllowUserToDeleteRows = false;
            this.dgvPercPicked.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPercPicked.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PercPicked,
            this.ppTarget,
            this.ppIsInValid});
            this.dgvPercPicked.Location = new System.Drawing.Point(424, 66);
            this.dgvPercPicked.Name = "dgvPercPicked";
            this.dgvPercPicked.RowHeadersVisible = false;
            this.dgvPercPicked.Size = new System.Drawing.Size(306, 48);
            this.dgvPercPicked.TabIndex = 1;
            // 
            // PercPicked
            // 
            this.PercPicked.HeaderText = "% Picked";
            this.PercPicked.Name = "PercPicked";
            this.PercPicked.ReadOnly = true;
            // 
            // ppTarget
            // 
            this.ppTarget.HeaderText = "Target";
            this.ppTarget.Name = "ppTarget";
            // 
            // ppIsInValid
            // 
            this.ppIsInValid.HeaderText = "IsInvalid";
            this.ppIsInValid.Name = "ppIsInValid";
            // 
            // dgvNotPickable
            // 
            this.dgvNotPickable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotPickable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NotPickable});
            this.dgvNotPickable.Location = new System.Drawing.Point(424, 215);
            this.dgvNotPickable.Name = "dgvNotPickable";
            this.dgvNotPickable.RowHeadersWidth = 20;
            this.dgvNotPickable.Size = new System.Drawing.Size(242, 150);
            this.dgvNotPickable.TabIndex = 2;
            // 
            // NotPickable
            // 
            this.NotPickable.HeaderText = "Players Not Pickable";
            this.NotPickable.Name = "NotPickable";
            this.NotPickable.Width = 200;
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(424, 168);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(143, 41);
            this.btnGetData.TabIndex = 3;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // btnRunModel
            // 
            this.btnRunModel.Location = new System.Drawing.Point(573, 168);
            this.btnRunModel.Name = "btnRunModel";
            this.btnRunModel.Size = new System.Drawing.Size(148, 41);
            this.btnRunModel.TabIndex = 3;
            this.btnRunModel.Text = "Run Model";
            this.btnRunModel.UseVisualStyleBackColor = true;
            this.btnRunModel.Click += new System.EventHandler(this.btnRunModel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(424, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Top # to select:";
            // 
            // tbSelect
            // 
            this.tbSelect.Location = new System.Drawing.Point(513, 126);
            this.tbSelect.Name = "tbSelect";
            this.tbSelect.Size = new System.Drawing.Size(54, 20);
            this.tbSelect.TabIndex = 5;
            // 
            // btnShowData
            // 
            this.btnShowData.Location = new System.Drawing.Point(596, 120);
            this.btnShowData.Name = "btnShowData";
            this.btnShowData.Size = new System.Drawing.Size(125, 41);
            this.btnShowData.TabIndex = 3;
            this.btnShowData.Text = "Show player data";
            this.btnShowData.UseVisualStyleBackColor = true;
            this.btnShowData.Click += new System.EventHandler(this.btnShowData_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 384);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(740, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Site:";
            // 
            // cbSite
            // 
            this.cbSite.FormattingEnabled = true;
            this.cbSite.Location = new System.Drawing.Point(46, 12);
            this.cbSite.Name = "cbSite";
            this.cbSite.Size = new System.Drawing.Size(121, 21);
            this.cbSite.TabIndex = 8;
            this.cbSite.SelectedIndexChanged += new System.EventHandler(this.cbSite_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 406);
            this.Controls.Add(this.cbSite);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tbSelect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnShowData);
            this.Controls.Add(this.btnRunModel);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.dgvNotPickable);
            this.Controls.Add(this.dgvPercPicked);
            this.Controls.Add(this.dgvCost);
            this.Controls.Add(this.dgvTeams);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPercPicked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotPickable)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTeams;
        private System.Windows.Forms.DataGridView dgvCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn costTarget;
        private System.Windows.Forms.DataGridViewTextBoxColumn costIsInvalid;
        private System.Windows.Forms.DataGridView dgvPercPicked;
        private System.Windows.Forms.DataGridViewTextBoxColumn PercPicked;
        private System.Windows.Forms.DataGridViewTextBoxColumn ppTarget;
        private System.Windows.Forms.DataGridViewTextBoxColumn ppIsInValid;
        private System.Windows.Forms.DataGridView dgvNotPickable;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotPickable;
        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.Button btnRunModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSelect;
        private System.Windows.Forms.Button btnShowData;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Team;
        private System.Windows.Forms.DataGridViewTextBoxColumn TmCurrent;
        private System.Windows.Forms.DataGridViewTextBoxColumn TmTarget;
        private System.Windows.Forms.DataGridViewTextBoxColumn TmIsInValid;
        private System.Windows.Forms.ComboBox cbSite;
    }
}