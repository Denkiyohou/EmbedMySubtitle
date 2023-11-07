using ReactiveUI;
using System;
using System.IO;
using FFmpeg;
using System.Windows.Input;
using EmbedMySubtitle.Services;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmbedMySubtitle.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() 
        {
            SelectVideoCommand = ReactiveCommand.Create(SelectVideo);
            SelectSubtitleCommand = ReactiveCommand.Create(SelectSubtitle);
        }

        private string? _videoFilePath;
        public string? VideoFilePath
        {
            get => _videoFilePath;
            set => this.RaiseAndSetIfChanged(ref _videoFilePath, value);
        }

        private string? _subtitleFilePath;
        public string? SubtitleFilePath
        {
            get => _subtitleFilePath;
            set => this.RaiseAndSetIfChanged(ref _subtitleFilePath, value);
        }

        //private string? _outputFilePaht;
        //public string? OutputFilePaht
        //{
        //    get => _outputFilePaht;
        //    set => this.RaiseAndSetIfChanged(ref _outputFilePaht, value);
        //}

        public ICommand SelectVideoCommand { get; }
        public ICommand SelectSubtitleCommand { get; }
        //public ICommand SelectOutputPathCommand { get; }
        public ICommand EmbedSubtitleDCommand { get; }

        public async void SelectVideo()
        {
            var result = await FileDialogService.OpenVideoFilePicker();
            if (result != null)
            {
                _videoFilePath = result;
            }
        }

        public async void SelectSubtitle()
        {
            var result = await FileDialogService.OpenSubtitleFilePicker();
            if(result != null)
            {
                _subtitleFilePath = result;
            }
        }

    }
}