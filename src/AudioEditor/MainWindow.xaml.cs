using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AudioEditor
{
    public partial class MainWindow : Window
    {
        private IWavePlayer? _waveOut;
        private AudioFileReader? _audioFile;
        private string? _currentFile;
        private bool _isPlaying = false;

        public MainWindow()
        {
            InitializeComponent();
            UpdateStatus("Ready");
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Audio files|*.wav;*.mp3|All files|*.*";
            if (dlg.ShowDialog() == true)
            {
                LoadFile(dlg.FileName);
            }
        }

        private void LoadFile(string path)
        {
            try
            {
                StopPlayback();
                _audioFile?.Dispose();
                _audioFile = new AudioFileReader(path);
                _waveOut = new WaveOutEvent();
                _waveOut.Init(_audioFile);
                _currentFile = path;
                FilePathText.Text = path;
                EndText.Text = Math.Round(_audioFile.TotalTime.TotalSeconds, 2).ToString();
                UpdateStatus($"Loaded {Path.GetFileName(path)}");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading file: {ex.Message}");
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            TogglePlayPause();
        }

        private void TogglePlayPause()
        {
            if (_waveOut == null || _audioFile == null)
            {
                UpdateStatus("No file loaded");
                return;
            }

            if (!_isPlaying)
            {
                _waveOut.Play();
                _isPlaying = true;
                PlayPauseButton.Content = "Pause";
                UpdateStatus("Playing");
            }
            else
            {
                _waveOut.Pause();
                _isPlaying = false;
                PlayPauseButton.Content = "Play";
                UpdateStatus("Paused");
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StopPlayback();
        }

        private void StopPlayback()
        {
            if (_waveOut != null)
            {
                _waveOut.Stop();
                _isPlaying = false;
                PlayPauseButton.Content = "Play";
                if (_audioFile != null)
                    _audioFile.Position = 0;
                UpdateStatus("Stopped");
            }
        }

        private async void TrimButton_Click(object sender, RoutedEventArgs e)
        {
            await TrimAndExportAsync();
        }

        private async Task TrimAndExportAsync()
        {
            if (string.IsNullOrEmpty(_currentFile) || _audioFile == null)
            {
                UpdateStatus("No file loaded");
                return;
            }

            if (!double.TryParse(StartText.Text, out double start) || !double.TryParse(EndText.Text, out double end))
            {
                UpdateStatus("Invalid start or end time");
                return;
            }

            if (start < 0) start = 0;
            if (end <= start)
            {
                UpdateStatus("End must be greater than start");
                return;
            }

            var saveDlg = new SaveFileDialog();
            saveDlg.Filter = "WAV file|*.wav|MP3 file|*.mp3";
            saveDlg.FileName = Path.GetFileNameWithoutExtension(_currentFile) + "_trim";
            if (saveDlg.ShowDialog() != true) return;

            UpdateStatus("Exporting...");
            await Task.Run(() =>
            {
                try
                {
                    TrimToFile(_currentFile, saveDlg.FileName, TimeSpan.FromSeconds(start), TimeSpan.FromSeconds(end));
                    Dispatcher.Invoke(() => UpdateStatus($"Exported to {saveDlg.FileName}"));
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => UpdateStatus($"Export error: {ex.Message}"));
                }
            });
        }

        private void TrimToFile(string sourcePath, string destPath, TimeSpan start, TimeSpan end)
        {
            using var reader = new AudioFileReader(sourcePath);
            var bytesPerSecond = reader.WaveFormat.AverageBytesPerSecond;
            var startPos = (long)(start.TotalSeconds * bytesPerSecond);
            var endPos = (long)(end.TotalSeconds * bytesPerSecond);
            startPos = (startPos / reader.WaveFormat.BlockAlign) * reader.WaveFormat.BlockAlign;
            endPos = (endPos / reader.WaveFormat.BlockAlign) * reader.WaveFormat.BlockAlign;

            reader.Position = startPos;
            var bytesRequired = endPos - startPos;

            if (Path.GetExtension(destPath).Equals(".wav", StringComparison.OrdinalIgnoreCase))
            {
                using var waveFile = new WaveFileWriter(destPath, reader.WaveFormat);
                var buffer = new byte[1024 * 4];
                while (bytesRequired > 0)
                {
                    var bytesToRead = (int)Math.Min(bytesRequired, buffer.Length);
                    var bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead == 0) break;
                    waveFile.Write(buffer, 0, bytesRead);
                    bytesRequired -= bytesRead;
                }
            }
            else
            {
                // For MP3 export we create a WAV temporary and let the user convert externally.
                var temp = Path.GetTempFileName() + ".wav";
                try
                {
                    using (var waveFile = new WaveFileWriter(temp, reader.WaveFormat))
                    {
                        var buffer = new byte[1024 * 4];
                        while (bytesRequired > 0)
                        {
                            var bytesToRead = (int)Math.Min(bytesRequired, buffer.Length);
                            var bytesRead = reader.Read(buffer, 0, bytesToRead);
                            if (bytesRead == 0) break;
                            waveFile.Write(buffer, 0, bytesRead);
                            bytesRequired -= bytesRead;
                        }
                    }
                    File.Move(temp, destPath);
                }
                finally
                {
                    if (File.Exists(temp)) File.Delete(temp);
                }
            }
        }

        private void UpdateStatus(string text)
        {
            StatusText.Text = text;
            // Set focus briefly to the status to help some screen readers announce updates
            try
            {
                StatusText.Focus();
            }
            catch { }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                TogglePlayPause();
                e.Handled = true;
            }
            else if (e.Key == Key.O && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                OpenButton_Click(this, new RoutedEventArgs());
                e.Handled = true;
            }
            else if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                StopButton_Click(this, new RoutedEventArgs());
                e.Handled = true;
            }
            else if (e.Key == Key.T && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                TrimButton_Click(this, new RoutedEventArgs());
                e.Handled = true;
            }
        }
    }
}
