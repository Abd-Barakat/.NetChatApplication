namespace ChatApplication
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.ConversationBox = new System.Windows.Forms.TextBox();
            this.MsgBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.InformationPanel = new System.Windows.Forms.Panel();
            this.AddressLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.AddressBox = new System.Windows.Forms.TextBox();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.ControlHelp = new System.Windows.Forms.ToolTip(this.components);
            this.InformationPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConversationBox
            // 
            this.ConversationBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConversationBox.BackColor = System.Drawing.Color.White;
            this.ConversationBox.Location = new System.Drawing.Point(12, 92);
            this.ConversationBox.Multiline = true;
            this.ConversationBox.Name = "ConversationBox";
            this.ConversationBox.ReadOnly = true;
            this.ConversationBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConversationBox.Size = new System.Drawing.Size(393, 184);
            this.ConversationBox.TabIndex = 5;
            this.ConversationBox.TabStop = false;
            // 
            // MsgBox
            // 
            this.MsgBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MsgBox.Location = new System.Drawing.Point(12, 294);
            this.MsgBox.Name = "MsgBox";
            this.MsgBox.Size = new System.Drawing.Size(306, 20);
            this.MsgBox.TabIndex = 1;
            this.ControlHelp.SetToolTip(this.MsgBox, "Write something ...");
            this.MsgBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MsgBox_KeyDown);
            // 
            // SendButton
            // 
            this.SendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendButton.Location = new System.Drawing.Point(326, 292);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(79, 23);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Send";
            this.ControlHelp.SetToolTip(this.SendButton, "Send your message to others");
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // InformationPanel
            // 
            this.InformationPanel.Controls.Add(this.AddressLabel);
            this.InformationPanel.Controls.Add(this.NameLabel);
            this.InformationPanel.Controls.Add(this.AddressBox);
            this.InformationPanel.Controls.Add(this.NameBox);
            this.InformationPanel.Controls.Add(this.ConnectButton);
            this.InformationPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.InformationPanel.Location = new System.Drawing.Point(0, 0);
            this.InformationPanel.Name = "InformationPanel";
            this.InformationPanel.Size = new System.Drawing.Size(421, 86);
            this.InformationPanel.TabIndex = 0;
            // 
            // AddressLabel
            // 
            this.AddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressLabel.AutoSize = true;
            this.AddressLabel.Location = new System.Drawing.Point(12, 51);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(51, 13);
            this.AddressLabel.TabIndex = 9;
            this.AddressLabel.Text = "Address :";
            // 
            // NameLabel
            // 
            this.NameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(12, 12);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(41, 13);
            this.NameLabel.TabIndex = 8;
            this.NameLabel.Text = "Name :";
            // 
            // AddressBox
            // 
            this.AddressBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressBox.Location = new System.Drawing.Point(64, 51);
            this.AddressBox.Name = "AddressBox";
            this.AddressBox.Size = new System.Drawing.Size(252, 20);
            this.AddressBox.TabIndex = 1;
            this.AddressBox.Text = "192.168.56.1";
            this.ControlHelp.SetToolTip(this.AddressBox, "Write Server address");
            this.AddressBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddressBox_KeyDown);
            // 
            // NameBox
            // 
            this.NameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameBox.Location = new System.Drawing.Point(64, 12);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(252, 20);
            this.NameBox.TabIndex = 0;
            this.NameBox.Text = "Ahmad";
            this.ControlHelp.SetToolTip(this.NameBox, "Write Your Name");
            this.NameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddressBox_KeyDown);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectButton.Location = new System.Drawing.Point(324, 51);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(81, 23);
            this.ConnectButton.TabIndex = 2;
            this.ConnectButton.Text = "Connect";
            this.ControlHelp.SetToolTip(this.ConnectButton, "Establish connection to Server");
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 334);
            this.Controls.Add(this.InformationPanel);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.MsgBox);
            this.Controls.Add(this.ConversationBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(437, 373);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.InformationPanel.ResumeLayout(false);
            this.InformationPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox ConversationBox;
        private System.Windows.Forms.TextBox MsgBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Panel InformationPanel;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox AddressBox;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.ToolTip ControlHelp;
    }
}

