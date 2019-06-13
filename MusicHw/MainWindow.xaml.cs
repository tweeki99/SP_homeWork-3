using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;

namespace MusicHw
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();
            Closing += WindowClosing;
            ChooseMusic();
        }
        public void WindowClosing(object sender, CancelEventArgs e)
        {
            var saveFile = new Thread(new ThreadStart(SaveFileIfWindowDisposed));
            saveFile.IsBackground = false;
            saveFile.Start();
        }
        private void SaveFileIfWindowDisposed()
        {
                FileStream fileStream = new FileStream("SavedFile", FileMode.Create);
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Text);
        }

        private void ChooseMusic()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                mediaPlayer.Open(new Uri(openFileDialog.FileName));

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            timer.Start();
        }

        void TimerTick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
                lblStatus.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            else
                lblStatus.Content = "No file selected...";
        }

        private void BtnPlayClick(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }

        private void BtnPauseClick(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void BtnStopClick(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

        private void BtnChangeClick(object sender, RoutedEventArgs e)
        {
            ChooseMusic();
        }
    }
}
