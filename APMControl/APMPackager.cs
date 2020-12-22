using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using static APMControl.APM;

namespace APMControl {
    public static class APMPackager {
        private static string PackageFolder = "UserPackage";
        private static object _packagerLocker = new object();
        public static async Task MakePackageAsync(string outputFileName) {
            lock (_packagerLocker) {
                if (Directory.Exists(PackageFolder)) {
                    Directory.Delete(PackageFolder, true);
                }
                Directory.CreateDirectory(PackageFolder);
                Directory.CreateDirectory($@"{PackageFolder}\{UserProfileFolderName}");
                Directory.CreateDirectory($@"{PackageFolder}\{DataAvatarsFolderName}");

                if (File.Exists(UserAvatarFileName)) {
                    File.Copy($@"{UserAvatarFileName}", $@"{PackageFolder}\{UserAvatarFileName}");
                }
                if (File.Exists(UserDataFileName)) {
                    File.Copy($@"{UserDataFileName}", $@"{PackageFolder}\{UserDataFileName}");
                }
                if (File.Exists(UserStorageFileName)) {
                    File.Copy($@"{UserStorageFileName}", $@"{PackageFolder}\{UserStorageFileName}");
                }

                foreach (string file in Directory.GetFiles(DataAvatarsFolderName)) {
                    string fileName = Path.GetFileName(file);
                    File.Copy(file, $@"{PackageFolder}\{DataAvatarsFolderName}\{fileName}");
                }

                ZipFile.CreateFromDirectory(PackageFolder, outputFileName, CompressionLevel.NoCompression, false);
            }
            await Task.Delay(TimeSpan.FromMilliseconds(100)); //延迟100毫秒
            Directory.Delete(PackageFolder, true);
        }
    }
}
