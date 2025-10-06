using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneOf.Types;

namespace TaskManager.Configuration
{
    public static class StartUpChecks
    {
        public static void CheckEnvironmentsExistance()
        {
            string[] environemts = {
                        "AllowedHosts",
                        // database connection string
                        "MYSQL_CONNECTION_STRING",
                        // mailing settings
                        "MAIL_SERVER",
                        "MAIL_USERNAME",
                        "MAIL_PASSWORD",
                        "MAIL_SENDER_NAME",
                        "MAIL_PORT",
                        "MAIL_TLS",
                        // logging settings
                        "LOKI_URL",
                        "APP_NAME",
                        "LOG_LEVEL",
                        // redis settings
                        "REDIS_CONNECTION_STRING",
                        // MinIO Configuration
                        "MINIO_ENDPOINT",
                        "MINIO_ACCESS_KEY",
                        "MINIO_SECRET_KEY",
                        "MINIO_BUCKET_NAME",
                        "MINIO_USE_SSL",
                        "MINIO_PRESIGNED_URL_EXPIRY",
             };


            foreach (string variable in environemts)
            {
                string? value = Environment.GetEnvironmentVariable(variable);
                
                if (value is null)
                {
                    throw new Exception($"Environment variable {variable} is not set.");
                }
             }


            
        }
    }
}







