using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace KSP_DL
{
    public partial class UncryptKey : Form
    {
        public string pathToDownloadFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "TMP_KSP-DL"
        );

        public UncryptKey()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void GetButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CDKeyInput.Text))
            {
                MessageBox.Show("Please enter a Decryption key.");
                return;
            }
            SelectKerbal();
        }

        private bool SelectKerbal()
        {
            var selectedVersion = KSP_Version.SelectedItem?.ToString();
            var selectedFileType = TypeOfFile.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedVersion))
            {
                MessageBox.Show("Please select a KSP version.");
                return false;
            }

            if (selectedVersion != "Kerbal Space Program 1.12.5.3190 (lastest)")
            {
                MessageBox.Show("Unsupported KSP version.");
                return false;
            }

            if (string.IsNullOrEmpty(selectedFileType))
            {
                MessageBox.Show("Please select a file type.");
                return false;
            }

            switch (selectedFileType)
            {
                case "SFX":
                case ".7z":
                    DownloadKsp(selectedVersion, selectedFileType);
                    break;

                default:
                    MessageBox.Show("Unknown file type.");
                    break;
            }

            return true;
        }

        private void DownloadKsp(string version, string fileType)
        {
            LockControls();
            string defaultPath = $"C:\\Users\\{Environment.UserName}\\KSP-DL_tmp";
            string TMP_Path = defaultPath;

          /*  DialogResult result = MessageBox.Show(
                $"Select temporary folder? Default is {defaultPath}",
                "Select custom path",
                MessageBoxButtons.YesNo
            );

            if (result == DialogResult.Yes)
            {
                PathToDonload.ShowDialog();
                TMP_Path = PathToDonload.SelectedPath;
            }
           
            MessageBox.Show($"Downloading to {TMP_Path}");*/
            GetButton.Font = new System.Drawing.Font(GetButton.Font.FontFamily, 8);
            if (version == "Kerbal Space Program 1.12.5.3190 (lastest)")
            {

                switch (fileType)
                {
                    case "SFX":
                        GetButton.Text = $"Downloading {version} {fileType}";
                      
                        break;
                    case ".7z":
                        GetButton.Text = $"Downloading {version} {fileType}";
                        break;



                }

            }
            else if (version == "") { }
        }

        private void LockControls()
        {
            GetButton.Enabled = false;
            GetButton.Text = "Processing...";
            KSP_Version.Enabled = false;
            TypeOfFile.Enabled = false;
            CDKeyInput.Enabled = false;
        }

        private void KSPVersionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void FileTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void WarningLabel_Click(object sender, EventArgs e)
        {
        }

        private void KeyInput_TextChanged(object sender, EventArgs e)
        {
        }

        private void progressBar_Click(object sender, EventArgs e)
        {
        }
    }
}
