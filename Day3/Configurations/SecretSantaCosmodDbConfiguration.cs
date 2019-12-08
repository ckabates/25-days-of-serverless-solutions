 using System;
using System.Collections.Generic;
using System.Text;

namespace Day3.Configurations
{
    public class SecretSantaCosmodDbConfiguration
    {
        public string ConnectionString { get; set; }

        public string DatabaseId { get; set; }

        public string ContainerId { get; set; }

        public string PartitionKey { get; set; }
    }
}
