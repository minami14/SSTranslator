using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using Google.Cloud.TextToSpeech.V1;
using Google.Cloud.Translation.V2;
using Google.Cloud.Vision.V1;
using SSTranslator.Properties;

namespace SSTranslator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string DefaultFolder =
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\VRChat";

        private readonly ImageAnnotatorClient _imageAnnotator = ImageAnnotatorClient.Create();
        private readonly TranslationClient _translation = TranslationClient.Create();
        private readonly TextToSpeechClient _speech = TextToSpeechClient.Create();
        private readonly FileSystemWatcher _watcher;

        private LanguageCode _language;
        private bool _play;

        public MainWindow()
        {
            InitializeComponent();

            var lang = Settings.Default.Language;
            ComboBoxLanguage.ItemsSource = LanguageCode.GetLanguageList();
            _language = LanguageCode.GetLanguageCode(lang);
            ComboBoxLanguage.Text = _language.Name;
            ComboBoxLanguage.SelectionChanged += (s, e) =>
            {
                _language = LanguageCode.GetLanguageCode((s as ComboBox).SelectedItem.ToString());
                Settings.Default.Language = _language.Name;
                Settings.Default.Save();
            };

            ButtonSelectFolder.Click += ButtonSelectFolder_Click;
            _play = Settings.Default.Play;
            CheckBoxPlayVoice.IsChecked = _play;
            CheckBoxPlayVoice.Checked += (s, e) =>
            {
                _play = true;
                Settings.Default.Play = true;
                Settings.Default.Save();
            };

            CheckBoxPlayVoice.Unchecked += (s, e) =>
            {
                _play = false;
                Settings.Default.Play = false;
                Settings.Default.Save();
            };

            var path = Settings.Default.TargetFolder;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = DefaultFolder;
            }

            if (!Directory.Exists(path))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }

            TextBoxTargetFolder.Text = path;
            _watcher = new FileSystemWatcher(path);
            _watcher.Created += Watcher_Created;
            _watcher.EnableRaisingEvents = true;
        }

        private void ButtonSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.Description = "Select a folder.";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var target = dlg.SelectedPath;
                _watcher.Path = target;
                Dispatcher.BeginInvoke(new Action(() => { TextBoxTargetFolder.Text = target; }));
                Settings.Default.TargetFolder = target;
                Settings.Default.Save();
            }
        }

        private object _translateLock = new object();

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            var image = Google.Cloud.Vision.V1.Image.FromFile(e.FullPath);
            var text = _imageAnnotator.DetectDocumentText(image);
            var translated = _translation.TranslateText(text.Text, _language.TranslateCode);
            lock (_translateLock)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TextBoxDetected.Text = text.Text;
                    TextBoxTranslated.Text = translated.TranslatedText;
                }));

                if (_play)
                {
                    var audio = TextToSpeech(translated.TranslatedText);
                    var player = new SoundPlayer(new MemoryStream(audio));
                    player.Play();
                    player.Dispose();
                }
            }
        }

        private byte[] TextToSpeech(string text)
        {
            var input = new SynthesisInput
            {
                Text = text
            };

            var voice = new VoiceSelectionParams
            {
                LanguageCode = _language.SpeechCode,
                Name = _language.SpeechName
            };

            var config = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Linear16
            };

            return _speech.SynthesizeSpeech(new SynthesizeSpeechRequest
            {
                Input = input,
                Voice = voice,
                AudioConfig = config
            }).AudioContent.ToByteArray();
        }
    }
}
