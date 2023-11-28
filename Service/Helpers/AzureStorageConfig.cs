namespace Service.Helpers
{
    public sealed class AzureStorageConfig
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public int TokenExpirationMinutes { get; set; }
    }
}
