namespace Automat
{
    partial class ForexConsole
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForexConsole));
            this.labelStatus = new System.Windows.Forms.Label();
            this.getButton = new System.Windows.Forms.Button();
            this.currencyTwo = new System.Windows.Forms.ComboBox();
            this.currencyOne = new System.Windows.Forms.ComboBox();
            this.logoutButton = new System.Windows.Forms.Button();
            this.loginButton = new System.Windows.Forms.Button();
            this.reportingBox = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitButton = new System.Windows.Forms.Button();
            this.forexResultLabel = new System.Windows.Forms.Label();
            this.marketStatus = new System.Windows.Forms.Label();
            this.forexActivity = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(20, 35);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(34, 13);
            this.labelStatus.TabIndex = 1;
            this.labelStatus.Text = "         ";
            // 
            // getButton
            // 
            this.getButton.Location = new System.Drawing.Point(321, 372);
            this.getButton.Name = "getButton";
            this.getButton.Size = new System.Drawing.Size(82, 23);
            this.getButton.TabIndex = 49;
            this.getButton.Text = "Initiate";
            this.getButton.UseVisualStyleBackColor = true;
            this.getButton.Click += new System.EventHandler(this.GetButtonClick);
            // 
            // currencyTwo
            // 
            this.currencyTwo.FormattingEnabled = true;
            this.currencyTwo.Items.AddRange(new object[] {
            "USD"});
            this.currencyTwo.Location = new System.Drawing.Point(304, 138);
            this.currencyTwo.Name = "currencyTwo";
            this.currencyTwo.Size = new System.Drawing.Size(78, 21);
            this.currencyTwo.TabIndex = 48;
            this.currencyTwo.Text = "USD";
            // 
            // currencyOne
            // 
            this.currencyOne.FormattingEnabled = true;
            this.currencyOne.Items.AddRange(new object[] {
            "EUR"});
            this.currencyOne.Location = new System.Drawing.Point(304, 111);
            this.currencyOne.Name = "currencyOne";
            this.currencyOne.Size = new System.Drawing.Size(78, 21);
            this.currencyOne.TabIndex = 47;
            this.currencyOne.Text = "EUR";
            // 
            // logoutButton
            // 
            this.logoutButton.Location = new System.Drawing.Point(197, 401);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(82, 23);
            this.logoutButton.TabIndex = 46;
            this.logoutButton.Text = "Logout";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.LogoutButtonClick);
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(23, 401);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(82, 23);
            this.loginButton.TabIndex = 45;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.LoginButtonClick);
            // 
            // reportingBox
            // 
            this.reportingBox.Location = new System.Drawing.Point(23, 114);
            this.reportingBox.Multiline = true;
            this.reportingBox.Name = "reportingBox";
            this.reportingBox.ReadOnly = true;
            this.reportingBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.reportingBox.Size = new System.Drawing.Size(275, 281);
            this.reportingBox.TabIndex = 44;
            // 
            // Label5
            // 
            this.Label5.Location = new System.Drawing.Point(20, 95);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(208, 16);
            this.Label5.TabIndex = 43;
            this.Label5.Text = "Server Response Log:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(301, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 52;
            this.label1.Text = "Currencies:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(427, 24);
            this.menuStrip1.TabIndex = 55;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(365, 401);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(38, 23);
            this.exitButton.TabIndex = 56;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButtonClick);
            // 
            // forexResultLabel
            // 
            this.forexResultLabel.AutoSize = true;
            this.forexResultLabel.Location = new System.Drawing.Point(20, 59);
            this.forexResultLabel.Name = "forexResultLabel";
            this.forexResultLabel.Size = new System.Drawing.Size(34, 13);
            this.forexResultLabel.TabIndex = 57;
            this.forexResultLabel.Text = "         ";
            // 
            // marketStatus
            // 
            this.marketStatus.AutoSize = true;
            this.marketStatus.Location = new System.Drawing.Point(294, 35);
            this.marketStatus.Name = "marketStatus";
            this.marketStatus.Size = new System.Drawing.Size(34, 13);
            this.marketStatus.TabIndex = 58;
            this.marketStatus.Text = "         ";
            // 
            // forexActivity
            // 
            this.forexActivity.Location = new System.Drawing.Point(302, 174);
            this.forexActivity.Multiline = true;
            this.forexActivity.Name = "forexActivity";
            this.forexActivity.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.forexActivity.Size = new System.Drawing.Size(117, 192);
            this.forexActivity.TabIndex = 59;
            // 
            // ForexConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 442);
            this.Controls.Add(this.forexActivity);
            this.Controls.Add(this.marketStatus);
            this.Controls.Add(this.forexResultLabel);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.getButton);
            this.Controls.Add(this.currencyTwo);
            this.Controls.Add(this.currencyOne);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.reportingBox);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "ForexConsole";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Step One - Forex Automat";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button getButton;
        private System.Windows.Forms.ComboBox currencyTwo;
        private System.Windows.Forms.ComboBox currencyOne;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.Button loginButton;
        internal System.Windows.Forms.TextBox reportingBox;
        internal System.Windows.Forms.Label Label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label forexResultLabel;
        private System.Windows.Forms.Label marketStatus;
        private System.Windows.Forms.TextBox forexActivity;

        #endregion
    }
}
