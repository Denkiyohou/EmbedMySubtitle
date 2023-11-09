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
            SelectOutputPathCommand = ReactiveCommand.Create(SelectOutputPath);
        }

        private FileDialogService _dialogService = new FileDialogService();

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

        private string? _outputFolderPath;
        public string? OutputFolderPath
        {
            get => _outputFolderPath;
            set => this.RaiseAndSetIfChanged(ref _outputFolderPath, value);
        }

        public ICommand SelectVideoCommand { get; }
        public ICommand SelectSubtitleCommand { get; }
        public ICommand SelectOutputPathCommand { get; }
        public ICommand EmbedSubtitleCommand { get; }

        public async void SelectVideo()
        {
            var result = await _dialogService.OpenVideoFilePicker();
            if (result != null)
            {
                VideoFilePath = result;
            }
        }

        public async void SelectSubtitle()
        {
            var result = await _dialogService.OpenSubtitleFilePicker();
            if(result != null)
            {
                SubtitleFilePath = result;
            }
        }

        public async void SelectOutputPath()
        {
            var result = await _dialogService.OpenFolderPicker();

            if (result != null)
            {
                OutputFolderPath = result;
            }
        }


    }
}