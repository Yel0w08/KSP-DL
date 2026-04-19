using Microsoft.VisualBasic.ApplicationServices;

namespace KSP_DL
{

    public partial class UncryptKey : Form
    {
    public string pathToDonwloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "TMP_KSP-DL");

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
                    DonwloadKSP(selectedVersion, selectedFileType);
                    break;

                case ".7z":
                    DonwloadKSP(selectedVersion, selectedFileType);
                    break;

                default:
                    MessageBox.Show("Unknown file type.");
                    break;
            }

            return true;
        }
        private void DonwloadKSP(string version, string fileType)
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            DialogResult dr = MessageBox.Show($"Select temporary folder ?, default is C:\\Users\\{userName}\\KSP-DL_tmp",
                                  "Select custom path", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    PathToDonload.ShowDialog();
                    break;
                case DialogResult.No:
                    break;
            }

            var downloadPath = PathToDonload.SelectedPath;
            LockEveryting();
        }
        private void LockEveryting()
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
