using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Hosting.Services;
using Microsoft.Owin.Hosting.Starter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApp;

namespace WebAppExe
{

    public class Program
    {

        static void Main(string[] args)
        {

            var currentProcess = Process.GetCurrentProcess();

            var port = 0;
            var appName = "";
            var key = "";
            var color = "";

            if (args.Length == 0)
            {
                Console.WriteLine("you must specify a configuration file");
                System.Environment.Exit(0);
            }
            
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var directory = Path.GetDirectoryName(path);
            directory = directory.Replace("\\bin", string.Empty);
            var file = directory + "\\" + args[0];

            Console.WriteLine("Loading " + file + "...");

            if (!File.Exists(file))
            {
                Console.WriteLine("File not found.");
                System.Environment.Exit(0);
            }

            using (var s = new StreamReader(file))
            {
                using(var reader = new JsonTextReader(s))
                {

                    var token = JObject.ReadFrom(reader);
                    port = token["port"].Value<int>();
                    appName = token["name"].Value<string>();
                    key = token["symKey"].Value<string>();
                    color = token["color"].Value<string>();
                }
            }
            
            var options = new StartOptions("http://localhost:" + port);
            options.AppStartup = "WebApp.Startup";

            Action<Microsoft.Owin.Hosting.Services.ServiceProvider> delfun = (provider) => {
                var appConfig = new AppConfig {
                    Name = appName,
                    Key = key,
                    DisplayName = appName,
                    Color = color
                };
                provider.AddInstance<IAppConfig>(appConfig);
            };

            var services = Microsoft.Owin.Hosting.Services.ServicesFactory.Create(delfun);
            var starter = services.GetService<IHostingStarter>();

            using (starter.Start(options))
            {
                Console.WriteLine(string.Format("App [{0}] running on port [{1}]", appName, port));
                Console.WriteLine(string.Format("ProcessId: [{0}]", currentProcess.Id));
                Console.WriteLine("Press any key to quit");
                Console.ReadLine();
            }

        }

    }
}