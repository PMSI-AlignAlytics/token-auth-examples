using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApp;

namespace WebAppExe
{
    public class Program
    {

        static void Main(string[] args)
        {

            var port = 0;
            var appName = "";
            var key = "";

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
                }
            }


            var options = new StartOptions("http://localhost:" + port);
            options.Settings.Add("name", appName);
            options.Settings.Add("key", key);
            options.Settings.Add("devmode", "false");

            using (Microsoft.Owin.Hosting.WebApp.Start<WebApp.Startup>("http://localhost:" + port))
            {
                Console.WriteLine(string.Format("App [{0}] running on port [{1}]", appName, port));
                Console.ReadLine();
            }

            

        }

    }
}