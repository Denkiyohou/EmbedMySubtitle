using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Dialogs;
using Avalonia;
using System.Reactive.Subjects;

namespace EmbedMySubtitle.Services
{
    public class FileDialogService
    {
        public async Task<string?> OpenVideoFilePicker()
        {
            var options = new FilePickerOpenOptions()
            {
                FileTypeFilter = new[] { videoAll },
                Title = "Select Video"
            };
            return await OpenFilePickerAsync(options);
        }

        public async Task<string?> OpenSubtitleFilePicker()
        {
            var options = new FilePickerOpenOptions()
            {
                FileTypeFilter = new[] { subtitleAll },
                Title = "Select Subtitle"
            };
            return await OpenFilePickerAsync(options);
        }

        public async Task<string?> OpenFolderPicker()
        {
            return await OpenFolderPickerAsync();
        }

        private static async Task<string?> OpenFilePickerAsync(FilePickerOpenOptions openOptions)
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");


            var result = await provider.OpenFilePickerAsync(openOptions);
            
            return result.FirstOrDefault()?.Path.LocalPath;
        }

        private static async Task<string?> OpenFolderPickerAsync()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            var result = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                AllowMultiple = false
            });

            return result.FirstOrDefault()?.Path.LocalPath;
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
