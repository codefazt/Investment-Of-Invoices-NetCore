using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TuFactoringModels;

namespace TuFactoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitialConfigurations.SetPort();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseUrls("http://0.0.0.0:" + InitialConfigurations.GetPort())
                .UseStartup<Startup>();
    }
}
