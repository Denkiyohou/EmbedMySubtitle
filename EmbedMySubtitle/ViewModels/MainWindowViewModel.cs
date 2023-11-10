using ReactiveUI;
using System;
using System.IO;
using System.Windows.Input;
using EmbedMySubtitle.Services;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Text.RegularExpressions;

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

        private double _progress;
        public double Progress
        {
            get => _progress;
            set => this.RaiseAndSetIfChanged(ref _progress, value);
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

        public async Task<bool> ProcessVideo()
        {
            try
            {
                var process = new Process();
                process.StartInfo = new ProcessStartInfo("ffmpeg", $"-i \"{VideoFilePath}\" " +
                    $"-vf \"subtitles={SubtitleFilePath}\" \"{OutputFolderPath}\"\\output.mp4")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                };

                process.Start();

                StreamReader reader = process.StandardOutput;

                while (!process.HasExited)
                {
                    var line = await reader.ReadLineAsync();
                    if(line != null)
                    {
                        var progressMatch = Regex.Match(line, @"time=(?<hours>\d+):(?<minutes>\d+):(?<seconds>\d+).(?<milliseconds>\d+)");
                        if(progressMatch.Success)
                        {
                            var hours = int.Parse(progressMatch.Groups["hours"].Value);
                            var minutes = int.Parse(progressMatch.Groups["minutes"].Value);
                            var seconds = int.Parse(progressMatch.Groups["seconds"].Value);
                            var milliseconds = int.Parse(progressMatch.Groups["milliseconds"].Value);

                            var totalmilliseconds = (hours * 3600 + minutes * 60 + seconds) * 1000 + milliseconds;

                            // TO-DO 匹配整个视频的时常并用totalmillisecond除以视频的长度
                        }
                    }
                }

                
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex );
                return false;
            }
        }

    }
}