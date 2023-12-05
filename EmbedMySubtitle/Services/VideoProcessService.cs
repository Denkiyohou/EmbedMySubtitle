using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using System.Diagnostics;

namespace EmbedMySubtitle.Services
{
    public class VideoProcessService
    {
        public event Action<double>? ProgressUpdated;

        public async Task EmbedSubtitle(string? videoFilePath, string? subtitleFilePath, string? outputFolderPath)
        {
            IMediaInfo inputFile = await FFmpeg.GetMediaInfo(videoFilePath);
            string outputFilePath = outputFolderPath + "\\output.mp4";

            IVideoStream videoStream = inputFile.VideoStreams.First().AddSubtitles(subtitleFilePath);

            var conversion = FFmpeg.Conversions.New()
                                               .AddStream(videoStream)
                                               .SetOutput(outputFilePath);

            conversion.OnProgress += (sender, args) =>
            {
                double progress = args.Duration.TotalSeconds / args.TotalLength.TotalSeconds * 100;
                ProgressUpdated?.Invoke(progress);
                Debug.WriteLine($"[{args.Duration} / {args.TotalLength}] {progress}%");
            };

            await conversion.Start();
        }

        public async Task compressVideo(string? videoFilePath, string? outputFolderPath, double progress = 0.0)
        {
            // TO-DO: Implement compressVideo()
        }
    }
}
