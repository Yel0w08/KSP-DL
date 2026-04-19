namespace KSP_DL
{
    partial class UncryptKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UncryptKey));
            GetButton = new Button();
            KSP_Version = new ComboBox();
            TypeOfFile = new ComboBox();
            Warning = new Label();
            ProgressBar = new ProgressBar();
            CDKeyInput = new TextBox();
            PathToDonload = new FolderBrowserDialog();
            SuspendLayout();
            // 
            // GetButton
            // 
            GetButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            GetButton.Font = new Font("Segoe UI", 15F);
            GetButton.Location = new Point(12, 393);
            GetButton.Name = "GetButton";
            GetButton.Size = new Size(360, 56);
            GetButton.TabIndex = 0;
            GetButton.Text = "Get";
            GetButton.UseVisualStyleBackColor = true;
            GetButton.Click += GetButton_Click;
            // 
            // KSP_Version
            // 
            KSP_Version.DisplayMember = "1";
            KSP_Version.FormattingEnabled = true;
            KSP_Version.IntegralHeight = false;
            KSP_Version.Items.AddRange(new object[] { "Kerbal Space Program 1.12.5.3190 (lastest)" });
            KSP_Version.Location = new Point(12, 364);
            KSP_Version.Name = "KSP_Version";
            KSP_Version.Size = new Size(304, 23);
            KSP_Version.TabIndex = 2;
            KSP_Version.Tag = "";
            KSP_Version.SelectedIndexChanged += KSPVersionComboBox_SelectedIndexChanged;
            // 
            // TypeOfFile
            // 
            TypeOfFile.FormattingEnabled = true;
            TypeOfFile.Items.AddRange(new object[] { "SFX", ".7z" });
            TypeOfFile.Location = new Point(322, 364);
            TypeOfFile.Name = "TypeOfFile";
            TypeOfFile.Size = new Size(50, 23);
            TypeOfFile.TabIndex = 3;
            TypeOfFile.SelectedIndexChanged += FileTypeComboBox_SelectedIndexChanged;
            // 
            // Warning
            // 
            Warning.AutoSize = true;
            Warning.ForeColor = Color.Red;
            Warning.Location = new Point(12, 9);
            Warning.Name = "Warning";
            Warning.Size = new Size(248, 15);
            Warning.TabIndex = 4;
            Warning.Text = "Warning: you need a key to uncrypt the game";
            Warning.Click += WarningLabel_Click;
            // 
            // ProgressBar
            // 
            ProgressBar.ForeColor = Color.FromArgb(255, 128, 0);
            ProgressBar.Location = new Point(12, 335);
            ProgressBar.Maximum = 1000;
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new Size(361, 23);
            ProgressBar.Step = 100;
            ProgressBar.TabIndex = 5;
            ProgressBar.UseWaitCursor = true;
            ProgressBar.Click += progressBar_Click;
            // 
            // CDKeyInput
            // 
            CDKeyInput.CharacterCasing = CharacterCasing.Upper;
            CDKeyInput.Cursor = Cursors.IBeam;
            CDKeyInput.ForeColor = Color.Black;
            CDKeyInput.HideSelection = false;
            CDKeyInput.Location = new Point(13, 38);
            CDKeyInput.MaxLength = 32;
            CDKeyInput.Name = "CDKeyInput";
            CDKeyInput.Size = new Size(360, 23);
            CDKeyInput.TabIndex = 6;
            CDKeyInput.TextChanged += KeyInput_TextChanged;
            // 
            // UncryptKey
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(384, 461);
            Controls.Add(CDKeyInput);
            Controls.Add(ProgressBar);
            Controls.Add(Warning);
            Controls.Add(TypeOfFile);
            Controls.Add(KSP_Version);
            Controls.Add(GetButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "UncryptKey";
            Text = "KSP-DL";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button GetButton;
        private ComboBox KSP_Version;
        private ComboBox TypeOfFile;
        private Label Warning;
        private ProgressBar ProgressBar;
        private TextBox CDKeyInput;
        private FolderBrowserDialog PathToDonload;
    }
}
