using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KSP_DL
{
    public partial class UncryptKey : Form
    {
        private const string SupportedVersion = "Kerbal Space Program 1.12.5.3190 (lastest)";
        private const string RepositoryOwner = "Yel0w08";
        private const string RepositoryName = "storage.archive.data";
        private const string RepositoryCommit = "6a4e68875160458baf09aa36fe9ac79fa07ada91";
        private const string RepositoryVersionPath = "Games/Kerbal Space Program/Kerbal Space Program 1.12.5.3190";

        private static readonly string[] ArchiveParts =
        {
            "Kerbal Space Program.7z.001",
            "Kerbal Space Program.7z.002",
            "Kerbal Space Program.7z.003",
            "Kerbal Space Program.7z.004",
            "Kerbal Space Program.7z.005",
        };

        private static readonly HttpClient HttpClient = new();

        private readonly string defaultDownloadFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "KSP-DL_tmp"
        );

        private readonly Dictionary<string, DownloadPreset> downloadPresets;
        private CancellationTokenSource? downloadCancellationTokenSource;
        private bool isDownloadInProgress;
        private bool isClosingAfterCleanup;

        public UncryptKey()
        {
            InitializeComponent();

            downloadPresets = new Dictionary<string, DownloadPreset>(StringComparer.OrdinalIgnoreCase)
            {
                [".7z"] = new DownloadPreset("7zip_Archive", ArchiveParts),
                ["SFX"] = new DownloadPreset(
                    "Self_Extracting",
                    ArchiveParts.Concat(new[] { "Kerbal Space Program.exe" }).ToArray()
                ),
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = 1000;
            ProgressBar.Value = 0;
            KSP_Version.SelectedIndex = 0;
            TypeOfFile.SelectedIndex = 0;
            FormClosing += UncryptKey_FormClosing;
            UpdateStartupWarning();
        }

        private async void GetButton_Click(object sender, EventArgs e)
        {
            var selectedFileType = TypeOfFile.SelectedItem?.ToString();
            if (string.Equals(selectedFileType, "TMP", StringComparison.OrdinalIgnoreCase))
            {
                await ClearTemporaryFolderAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(CDKeyInput.Text))
            {
                MessageBox.Show("Please enter a decryption key.");
                return;
            }

            await StartDownloadAsync();
        }

        private async Task StartDownloadAsync()
        {
            var selectedVersion = KSP_Version.SelectedItem?.ToString();
            var selectedFileType = TypeOfFile.SelectedItem?.ToString();
            var cdKey = CDKeyInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(selectedVersion))
            {
                MessageBox.Show("Please select a KSP version.");
                return;
            }

            if (!string.Equals(selectedVersion, SupportedVersion, StringComparison.Ordinal))
            {
                MessageBox.Show("Unsupported KSP version.");
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedFileType))
            {
                MessageBox.Show("Please select a file type.");
                return;
            }

            if (!downloadPresets.TryGetValue(selectedFileType, out var preset))
            {
                MessageBox.Show("Unknown file type.");
                return;
            }

            var downloadFolder = defaultDownloadFolder;

            try
            {
                downloadCancellationTokenSource = new CancellationTokenSource();
                isDownloadInProgress = true;
                LockControls($"Preparing {selectedFileType} download...");
                Directory.CreateDirectory(downloadFolder);

                await DownloadPresetAsync(
                    preset,
                    downloadFolder,
                    cdKey,
                    downloadCancellationTokenSource.Token
                );

                if (string.Equals(selectedFileType, "SFX", StringComparison.OrdinalIgnoreCase))
                {
                    StartSfxExecutable(downloadFolder, cdKey);
                }
                else if (string.Equals(selectedFileType, ".7z", StringComparison.OrdinalIgnoreCase))
                {
                    ExtractSevenZipArchive(downloadFolder, cdKey);
                }

                MessageBox.Show(
                    $"Download complete.\nFiles saved in:\n{downloadFolder}",
                    "Done",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Download failed:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                isDownloadInProgress = false;
                downloadCancellationTokenSource?.Dispose();
                downloadCancellationTokenSource = null;
                UnlockControls();
            }
        }

        private async Task DownloadPresetAsync(
            DownloadPreset preset,
            string destinationFolder,
            string cdKey,
            CancellationToken cancellationToken
        )
        {
            var totalFiles = preset.Files.Length;

            for (var index = 0; index < totalFiles; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var fileName = preset.Files[index];
                var sourceUrl = BuildGithubMediaUrl(preset.RepositoryFolder, fileName);
                var destinationPath = Path.Combine(destinationFolder, fileName);

                SetStatus($"Downloading {fileName} ({index + 1}/{totalFiles})");

                await DownloadFileAsync(
                    sourceUrl,
                    destinationPath,
                    fileProgress =>
                    {
                        var globalProgress = ((index + fileProgress) / totalFiles) * 1000d;
                        SetProgress((int)Math.Round(globalProgress));
                    },
                    cancellationToken
                );
            }

            SetStatus("Download complete");
            SetProgress(ProgressBar.Maximum);
        }

        private static string BuildGithubMediaUrl(string repositoryFolder, string fileName)
        {
            var relativePath = $"{RepositoryVersionPath}/{repositoryFolder}/{fileName}";
            var escapedSegments = relativePath
                .Split('/')
                .Select(Uri.EscapeDataString);

            return $"https://media.githubusercontent.com/media/{RepositoryOwner}/{RepositoryName}/{RepositoryCommit}/{string.Join("/", escapedSegments)}";
        }

        private static string BuildPasswordArgument(string cdKey)
        {
            return $"-p{cdKey}";
        }

        private static async Task DownloadFileAsync(
            string sourceUrl,
            string destinationPath,
            Action<double> reportProgress,
            CancellationToken cancellationToken
        )
        {
            using var response = await HttpClient.GetAsync(
                sourceUrl,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken
            );
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength;

            await using var input = await response.Content.ReadAsStreamAsync(cancellationToken);
            await using var output = new FileStream(
                destinationPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None
            );

            var buffer = new byte[81920];
            long totalBytesRead = 0;
            int bytesRead;

            while (
                (bytesRead = await input.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken)) > 0
            )
            {
                await output.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
                totalBytesRead += bytesRead;

                if (totalBytes is > 0)
                {
                    reportProgress(totalBytesRead / (double)totalBytes.Value);
                }
            }

            reportProgress(1);
        }

        private void LockControls(string status)
        {
            GetButton.Enabled = false;
            KSP_Version.Enabled = false;
            TypeOfFile.Enabled = false;
            CDKeyInput.Enabled = false;
            SetStatus(status);
            SetProgress(0);
        }

        private void UnlockControls()
        {
            GetButton.Enabled = true;
            KSP_Version.Enabled = true;
            TypeOfFile.Enabled = true;
            CDKeyInput.Enabled = true;
            GetButton.Font = new System.Drawing.Font(GetButton.Font.FontFamily, 15);
            GetButton.Text = "Get";
        }

        private void SetStatus(string status)
        {
            if (InvokeRequired)
            {
                Invoke(() => SetStatus(status));
                return;
            }

            GetButton.Font = new System.Drawing.Font(GetButton.Font.FontFamily, 8);
            GetButton.Text = status;
        }

        private void SetProgress(int value)
        {
            var boundedValue = Math.Max(ProgressBar.Minimum, Math.Min(ProgressBar.Maximum, value));

            if (InvokeRequired)
            {
                Invoke(() => SetProgress(boundedValue));
                return;
            }

            ProgressBar.Value = boundedValue;
        }

        private void UpdateStartupWarning()
        {
            if (HasTemporaryFiles())
            {
                Warning.ForeColor = System.Drawing.Color.DarkOrange;
                Warning.Text = $"Warning: temporary files were detected in {Path.GetFileName(defaultDownloadFolder)}";
                return;
            }

            Warning.ForeColor = System.Drawing.Color.Red;
            Warning.Text = "Warning: you need a key to uncrypt the game";
        }

        private bool HasTemporaryFiles()
        {
            if (!Directory.Exists(defaultDownloadFolder))
            {
                return false;
            }

            return Directory.EnumerateFileSystemEntries(defaultDownloadFolder).Any();
        }

        private async void UncryptKey_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (isClosingAfterCleanup || !isDownloadInProgress)
            {
                return;
            }

            e.Cancel = true;
            LockControls("Cancelling download...");
            downloadCancellationTokenSource?.Cancel();

            await CleanupTemporaryFilesAsync();

            isClosingAfterCleanup = true;
            Close();
        }

        private async Task CleanupTemporaryFilesAsync()
        {
            try
            {
                await WaitForDownloadShutdownAsync();

                if (Directory.Exists(defaultDownloadFolder))
                {
                    Directory.Delete(defaultDownloadFolder, true);
                }
                SetProgress(ProgressBar.Maximum);
                TypeOfFile.Text = "SFX";
            }
            catch
            {
            }
        }

        private async Task ClearTemporaryFolderAsync()
        {
            try
            {
                LockControls("Clearing temp files...");
                await CleanupTemporaryFilesAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to clear temporary files:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                UnlockControls();
            }
        }

        private async Task WaitForDownloadShutdownAsync()
        {
            const int maxAttempts = 50;

            for (var attempt = 0; attempt < maxAttempts && isDownloadInProgress; attempt++)
            {
                await Task.Delay(100);
            }
        }

        private static void StartSfxExecutable(string downloadFolder, string cdKey)
        {
            var sfxPath = Path.Combine(downloadFolder, "Kerbal Space Program.exe");

            if (!File.Exists(sfxPath))
            {
                throw new FileNotFoundException("The SFX executable was not found after download.", sfxPath);
            }

            Process.Start(
                new ProcessStartInfo
                {
                    FileName = sfxPath,
                    Arguments = BuildPasswordArgument(cdKey),
                    UseShellExecute = true,
                    WorkingDirectory = downloadFolder,
                }
            );
        }

        private static void ExtractSevenZipArchive(string downloadFolder, string cdKey)
        {
            var archivePath = Path.Combine(downloadFolder, "Kerbal Space Program.7z.001");
            if (!File.Exists(archivePath))
            {
                throw new FileNotFoundException("The first archive part was not found after download.", archivePath);
            }

            var sevenZipPath = FindSevenZipExecutable();
            if (string.IsNullOrWhiteSpace(sevenZipPath))
            {
                throw new FileNotFoundException(
                    "7z.exe was not found. Install 7-Zip or add 7z.exe to PATH to extract the archive automatically."
                );
            }

            var outputFolder = Path.Combine(downloadFolder, "Extracted");
            Directory.CreateDirectory(outputFolder);

            var process = Process.Start(
                new ProcessStartInfo
                {
                    FileName = sevenZipPath,
                    Arguments = $"x \"{archivePath}\" {BuildPasswordArgument(cdKey)} -o\"{outputFolder}\" -y",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = downloadFolder,
                }
            );

            if (process == null)
            {
                throw new Win32Exception("Failed to start 7z.exe.");
            }

            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"7z extraction failed with exit code {process.ExitCode}.");
            }
        }

        private static string? FindSevenZipExecutable()
        {
            var candidates = new[]
            {
                "7z.exe",
                "7zz.exe",
                @"C:\Program Files\7-Zip\7z.exe",
                @"C:\Program Files (x86)\7-Zip\7z.exe",
            };

            foreach (var candidate in candidates)
            {
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            return null;
        }

        private void KSPVersionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FileTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TypeOfFile.SelectedItem?.ToString() == "TMP")
            {
          
                GetButton.Text = "Clear";
   

            }
            else
            {
         
            }
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private sealed record DownloadPreset(string RepositoryFolder, string[] Files);
    }
}
