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
        public async Task EmbedSubtitle(string videoFilePath, string subtitleFilePath, string outputFolderPath, double progress = 0.0)
        {
            IMediaInfo inputFile = await FFmpeg.GetMediaInfo(videoFilePath);
            string outputFilePath = outputFolderPath + "\\output.mp4";

            IVideoStream videoStream = inputFile.VideoStreams.First().AddSubtitles(subtitleFilePath);

            var conversion = FFmpeg.Conversions.New()
                                               .AddStream(videoStream)
                                               .SetOutput(outputFilePath);

            conversion.OnProgress += (sender, args) =>
            {
                progress = args.Duration.TotalSeconds / args.TotalLength.TotalSeconds * 100;
                Debug.WriteLine($"[{args.Duration} / {args.TotalLength}] {progress}%");
            };

            await conversion.Start();
        }

        public async Task compressVideo(string videoFilePath, string outputFolderPath, double progress = 0.0)
        {
            // TO-Do: Implement compressVideo()
        }
    }
}
