using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using static APMControl.APM;

namespace APMControl {
    public static class APMPackager {
        private static readonly string PackageFolder = "UserPackage";
        private static readonly object _packagerLocker = new object();

        public static async Task PackAsync(string outputFileName) {
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

        public static async Task UnpackAsync(string fileName) {
            if (Directory.Exists(PackageFolder)) {
                Directory.Delete(PackageFolder, true);
            }
            await Task.Run(() => {
                lock (_packagerLocker) {
                    ZipFile.ExtractToDirectory(fileName, PackageFolder);
                }
            });

            lock (_packagerLocker) {
                if (Directory.Exists(UserProfileFolderName)) {
                    Directory.Delete(UserProfileFolderName, true);
                }
                Directory.CreateDirectory(UserProfileFolderName);
                if (Directory.Exists(DataAvatarsFolderName)) {
                    Directory.Delete(DataAvatarsFolderName, true);
                }
                Directory.CreateDirectory(DataAvatarsFolderName);

                //复制用户数据
                File.Copy($@"{PackageFolder}\{UserDataFileName}", $@"{UserDataFileName}");
                File.Copy($@"{PackageFolder}\{UserAvatarFileName}", $@"{UserAvatarFileName}");
                File.Copy($@"{PackageFolder}\{UserStorageFileName}", $@"{UserStorageFileName}");

                //复制头像文件
                foreach (var file in Directory.GetFiles($@"{PackageFolder}\{DataAvatarsFolderName}")) {
                    string avatarFileName = Path.GetFileName(file);
                    File.Copy(file, $@"{DataAvatarsFolderName}\{avatarFileName}");
                }

                Directory.Delete(PackageFolder, true);
            }
        }
    }
}
