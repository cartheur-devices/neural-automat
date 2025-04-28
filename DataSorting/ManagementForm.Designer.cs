namespace DataSorting
{
    partial class ManagementForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagementForm));
            this.displayChart = new NeuralAlgorithm.Controls.Chart();
            this.closeButton = new System.Windows.Forms.Button();
            this.searchSolutionButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.forexWeekBeginning = new System.Windows.Forms.Label();
            this.forexWeekEnding = new System.Windows.Forms.Label();
            this.iterationsTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // displayChart
            // 
            this.displayChart.Location = new System.Drawing.Point(12, 12);
            this.displayChart.Name = "displayChart";
            this.displayChart.Size = new System.Drawing.Size(1307, 241);
            this.displayChart.TabIndex = 0;
            this.displayChart.Text = "chart1";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(1271, 475);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(48, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // searchSolutionButton
            // 
            this.searchSolutionButton.Location = new System.Drawing.Point(1076, 475);
            this.searchSolutionButton.Name = "searchSolutionButton";
            this.searchSolutionButton.Size = new System.Drawing.Size(100, 23);
            this.searchSolutionButton.TabIndex = 2;
            this.searchSolutionButton.Text = "Search solution";
            this.searchSolutionButton.UseVisualStyleBackColor = true;
            this.searchSolutionButton.Click += new System.EventHandler(this.searchSolutionButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 260);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Forex data for the week beginning:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(778, 260);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Forex data for the week ending:";
            // 
            // forexWeekBeginning
            // 
            this.forexWeekBeginning.AutoSize = true;
            this.forexWeekBeginning.Location = new System.Drawing.Point(190, 260);
            this.forexWeekBeginning.Name = "forexWeekBeginning";
            this.forexWeekBeginning.Size = new System.Drawing.Size(16, 13);
            this.forexWeekBeginning.TabIndex = 5;
            this.forexWeekBeginning.Text = "...";
            // 
            // forexWeekEnding
            // 
            this.forexWeekEnding.AutoSize = true;
            this.forexWeekEnding.Location = new System.Drawing.Point(941, 260);
            this.forexWeekEnding.Name = "forexWeekEnding";
            this.forexWeekEnding.Size = new System.Drawing.Size(16, 13);
            this.forexWeekEnding.TabIndex = 6;
            this.forexWeekEnding.Text = "...";
            // 
            // iterationsTextBox
            // 
            this.iterationsTextBox.Location = new System.Drawing.Point(1129, 449);
            this.iterationsTextBox.Name = "iterationsTextBox";
            this.iterationsTextBox.Size = new System.Drawing.Size(47, 20);
            this.iterationsTextBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1073, 452);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Iterations";
            // 
            // ManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1331, 510);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.iterationsTextBox);
            this.Controls.Add(this.forexWeekEnding);
            this.Controls.Add(this.forexWeekBeginning);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchSolutionButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.displayChart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ManagementForm";
            this.Text = "Data Sorting & Management Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NeuralAlgorithm.Controls.Chart displayChart;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button searchSolutionButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label forexWeekBeginning;
        private System.Windows.Forms.Label forexWeekEnding;
        private System.Windows.Forms.TextBox iterationsTextBox;
        private System.Windows.Forms.Label label3;
    }
}

