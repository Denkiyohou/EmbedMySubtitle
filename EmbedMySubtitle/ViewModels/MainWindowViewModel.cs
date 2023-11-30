using ReactiveUI;
using System;
using System.IO;
using System.Windows.Input;
using EmbedMySubtitle.FileDialogServices;
using System.Threading.Tasks;
using System.Diagnostics;
using Xabe.FFmpeg;
using System.Linq;

namespace EmbedMySubtitle.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() 
        {
            SelectVideoCommand = ReactiveCommand.Create(SelectVideo);
            SelectSubtitleCommand = ReactiveCommand.Create(SelectSubtitle);
            SelectOutputPathCommand = ReactiveCommand.Create(SelectOutputPath);
            ProcessCommand = ReactiveCommand.Create(ProcessVideo);
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

        public async Task ProcessVideo()
        {
            IMediaInfo inputFile = await FFmpeg.GetMediaInfo(VideoFilePath);
            string outputFilePath = OutputFolderPath + "\\output.mp4";

            IVideoStream videoStream = inputFile.VideoStreams.First().AddSubtitles(SubtitleFilePath);

            var conversion = FFmpeg.Conversions.New()
                                               .AddStream(videoStream)
                                               .SetOutput(outputFilePath);

            conversion.OnProgress += (sender, args) =>
            {
                Progress = args.Duration.TotalSeconds / args.TotalLength.TotalSeconds * 100;
                Debug.WriteLine($"[{args.Duration} / {args.TotalLength}] {Progress}%");
            };

            await conversion.Start();                                                      
        }

    }
}