using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Text;

namespace DevOps.ManagementServices.APIManagement
{
    public static class Extensions
    {
        public static IConfigurationRoot BuildConfiguraion(this ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                                .SetBasePath(context.FunctionAppDirectory)
                                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                                .AddEnvironmentVariables()
                                .Build();
            return config;
        }
    }
}
