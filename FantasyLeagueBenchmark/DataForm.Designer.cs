namespace FantasyLeagueBenchmark
{
    partial class DataForm
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
            this.dgv = new System.Windows.Forms.DataGridView();
            this.Player = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Team = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Points = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PercPicked = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsPicked = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pickable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Player,
            this.Position,
            this.Team,
            this.Price,
            this.Points,
            this.PercPicked,
            this.Seq,
            this.IsPicked,
            this.Pickable});
            this.dgv.Location = new System.Drawing.Point(13, 13);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.Size = new System.Drawing.Size(853, 325);
            this.dgv.TabIndex = 0;
            // 
            // Player
            // 
            this.Player.HeaderText = "Name";
            this.Player.Name = "Player";
            this.Player.ReadOnly = true;
            // 
            // Position
            // 
            this.Position.HeaderText = "Position";
            this.Position.Name = "Position";
            this.Position.ReadOnly = true;
            // 
            // Team
            // 
            this.Team.HeaderText = "Team";
            this.Team.Name = "Team";
            this.Team.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // Points
            // 
            this.Points.HeaderText = "Points";
            this.Points.Name = "Points";
            this.Points.ReadOnly = true;
            // 
            // PercPicked
            // 
            this.PercPicked.HeaderText = "% Picked";
            this.PercPicked.Name = "PercPicked";
            this.PercPicked.ReadOnly = true;
            // 
            // Seq
            // 
            this.Seq.HeaderText = "Seq";
            this.Seq.Name = "Seq";
            this.Seq.ReadOnly = true;
            this.Seq.Width = 30;
            // 
            // IsPicked
            // 
            this.IsPicked.HeaderText = "IsPicked";
            this.IsPicked.Name = "IsPicked";
            this.IsPicked.ReadOnly = true;
            // 
            // Pickable
            // 
            this.Pickable.HeaderText = "Pickable";
            this.Pickable.Name = "Pickable";
            this.Pickable.ReadOnly = true;
            // 
            // DataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 350);
            this.Controls.Add(this.dgv);
            this.Name = "DataForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn Player;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position;
        private System.Windows.Forms.DataGridViewTextBoxColumn Team;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Points;
        private System.Windows.Forms.DataGridViewTextBoxColumn PercPicked;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seq;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsPicked;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pickable;
    }
}

