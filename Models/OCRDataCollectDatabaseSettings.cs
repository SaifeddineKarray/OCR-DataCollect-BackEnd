﻿namespace WebAPI.Models
{
    public class OCRDataCollectDatabaseSettings : IOCRDataCollectDatabaseSettings
    {
        public string DataCollectionName { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
    }
}
