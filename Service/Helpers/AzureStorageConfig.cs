﻿namespace Service.Helpers
{
    public class AzureStorageConfig
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public int TokenExpirationMinutes { get; set; }
    }
}
