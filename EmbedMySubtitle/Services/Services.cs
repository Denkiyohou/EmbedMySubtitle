using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace EmbedMySubtitle.Services
{
    public class FileDialogService
    {   
        public static async Task<string?> OpenVideoFilePicker()
        {
            var options = new FilePickerOpenOptions()
            {
                FileTypeFilter = new[] { videoAll },
                Title = "Select Video"
            };
            return await OpenFilePickerAsync(options);
        }

        public static async Task<string?> OpenSubtitleFilePicker()
        {
            var options = new FilePickerOpenOptions()
            {
                FileTypeFilter = new[] { subtitleAll },
                Title = "Select Subtitle"
            };
            return await OpenFilePickerAsync(options);
        }

        private static async Task<string?> OpenFilePickerAsync(FilePickerOpenOptions openOptions)
        {
            var window = GetWindow();

            if (window is null)
            {
                return null;
            }

            var result = await window.StorageProvider.OpenFilePickerAsync(openOptions);

            return result.FirstOrDefault()?.Path.LocalPath;
        }

        private static Window? GetWindow()
        {
            var lifetime = Avalonia.Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            return lifetime?.MainWindow;
        }

        public static FilePickerFileType videoAll { get; } = new("All Videos")
        {
            Patterns = new[] { "*.mp4", "*.mkv" }
        };

        public static FilePickerFileType subtitleAll { get; } = new("All Subtitles")
        {
            Patterns = new[] { "*.ass" }
        };
    }
}
