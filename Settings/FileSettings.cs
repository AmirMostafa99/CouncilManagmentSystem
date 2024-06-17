namespace CouncilsManagmentSystem.Settings
{
    public static class FileSettings
    {
        public const string FilesPath = "/TopicsFiles";
        public const string AllowedExtensions = ".pdf";
        public const int MaxFileSizeInMB = 100;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
    }
}
