namespace KSP_DL
{
    partial class UncryptKey
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UncryptKey));
            GetButton = new Button();
            KSP_Version = new ComboBox();
            TypeOfFile = new ComboBox();
            Warning = new Label();
            progressBar = new ProgressBar();
            CDKeyInput = new TextBox();
            PathToDonload = new FolderBrowserDialog();
            SuspendLayout();
            // 
            // GetButton
            // 
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
            Warning.Size = new Size(251, 15);
            Warning.TabIndex = 4;
            Warning.Text = "Warning : you need a key to uncrypt the game";
            Warning.Click += WarningLabel_Click;
            // 
            // progressBar
            // 
            progressBar.ForeColor = Color.FromArgb(255, 128, 0);
            progressBar.Location = new Point(12, 335);
            progressBar.Maximum = 1000;
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(361, 23);
            progressBar.Step = 100;
            progressBar.TabIndex = 5;
            progressBar.UseWaitCursor = true;
            progressBar.Click += progressBar_Click;
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
            ClientSize = new Size(384, 461);
            Controls.Add(CDKeyInput);
            Controls.Add(progressBar);
            Controls.Add(Warning);
            Controls.Add(TypeOfFile);
            Controls.Add(KSP_Version);
            Controls.Add(GetButton);
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
        private ProgressBar progressBar;
        private TextBox CDKeyInput;
        private FolderBrowserDialog PathToDonload;
    }
}
