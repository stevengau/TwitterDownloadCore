﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace TwitterDownloadCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://0.0.0.0:80")
            .UseStartup<Startup>()
            .ConfigureLogging(logging => 
                logging.AddFilter("Default", LogLevel.Warning).SetMinimumLevel(LogLevel.Warning)
               .AddFilter<DebugLoggerProvider>("Microsoft", LogLevel.Warning));
            
        
    }
}
