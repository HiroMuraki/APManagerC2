namespace APMControl {
    public static class APM {
        public const string UserProfileFolderName = "UserProfile";
        public const string UserStorageFileName = @"UserProfile\Storage.db";
        public const string RuntimeStorageFileName = @"UserProfile\__Storage__.db";
        public const string UserDataFileName = @"UserProfile\UserData";
        public const string UserAvatarFileName = @"UserProfile\Avatar";
        public const string DataAvatarsFolderName = "DataAvatars";
        public const string UserPasswordRegular = @"^[\u0021-\u007E]{4,16}$";
    }
}
