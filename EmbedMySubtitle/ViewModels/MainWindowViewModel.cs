using ReactiveUI;
using System;
using System.IO;
using System.Windows.Input;
using EmbedMySubtitle.FileDialogServices;
using System.Threading.Tasks;
using System.Linq;
using EmbedMySubtitle.Services;

namespace EmbedMySubtitle.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private FileDialogService _fileDialogService;
        private VideoProcessService _videoProcessService;

        public MainWindowViewModel() 
        {
            _fileDialogService = new FileDialogService();
            _videoProcessService = new VideoProcessService();
            _videoProcessService.ProgressUpdated += OnProgressUpdated;

            SelectVideoCommand = ReactiveCommand.Create(SelectVideo);
            SelectSubtitleCommand = ReactiveCommand.Create(SelectSubtitle);
            SelectOutputPathCommand = ReactiveCommand.Create(SelectOutputPath);
            ProcessCommand = ReactiveCommand.Create(ProcessVideo);
        }

        private void OnProgressUpdated(double progress)
        {
            Progress = progress;
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

        private string? _outputFolderPath;
        public string? OutputFolderPath
        {
            get => _outputFolderPath;
            set => this.RaiseAndSetIfChanged(ref _outputFolderPath, value);
        }

        private double _progress;
        public double Progress
        {
            get => _progress;
            set => this.RaiseAndSetIfChanged(ref _progress, value);
        }

        public ICommand SelectVideoCommand { get; }
        public ICommand SelectSubtitleCommand { get; }
        public ICommand SelectOutputPathCommand { get; }
        public ICommand ProcessCommand { get; }

        public async void SelectVideo()
        {
            var result = await _fileDialogService.OpenVideoFilePicker();
            if (result != null)
            {
                VideoFilePath = result;
            }
        }

        public async void SelectSubtitle()
        {
            var result = await _fileDialogService.OpenSubtitleFilePicker();
            if(result != null)
            {
                SubtitleFilePath = result;
            }
        }

        public async void SelectOutputPath()
        {
            var result = await _fileDialogService.OpenFolderPicker();
            if (result != null)
            {
                OutputFolderPath = result;
            }
        }

        public async Task ProcessVideo()
        {
            await _videoProcessService.EmbedSubtitle(VideoFilePath, SubtitleFilePath, OutputFolderPath);                                             
        }

    }
}