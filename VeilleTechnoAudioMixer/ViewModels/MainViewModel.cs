using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VeilleTechnoAudioMixer.Commands;

namespace VeilleTechnoAudioMixer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private float currentVolume;
        private double slidervalue;
        private string filename;
        public string fileName
        {
            get => filename;
            set
            {
                filename = value;
                OnPropertyChanged();
            }
        }

        public double sliderValue
        {
            get => slidervalue;
            set
            {
                slidervalue = value;
                OnPropertyChanged();
                Slider_ValueChanged(slidervalue);
            }
        }
        public string path;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        public ObservableCollection<string> msg { get; private set; }
        public DelegateCommand<object> Play { get; private set; }
        public DelegateCommand<object> OpenFile { get; private set; }


        public MainViewModel()
        {
            msg = new ObservableCollection<string>()
            {
                "hello",
                "goodbye"
            };


            Play = new DelegateCommand<object>(PlayMusic);
            OpenFile = new DelegateCommand<object>(Openfile);

        }


        public void PlayMusic(object wav)
        {

            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null && path != null)
            {
                audioFile = new AudioFileReader(path);
                outputDevice.Init(audioFile);
            }
            if (path != null)
            {
                audioFile.Volume = currentVolume;
                outputDevice.Play();
            }

        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            outputDevice.Dispose();
            outputDevice = null;
            audioFile.Dispose();
            audioFile = null;
        }

        public void Openfile(object file)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            path = openFileDialog.FileName;
            fileName = openFileDialog.SafeFileName;
        }
        private void Slider_ValueChanged(double slidervalue)
        {
            var converted = Convert.ToSingle(slidervalue);
            Trace.WriteLine(converted);
            if (audioFile != null)
            {
                audioFile.Volume = converted;
            }
            currentVolume = converted;
        }
    }
}
