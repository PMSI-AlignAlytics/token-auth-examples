using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace WebApp
{
    public class AppConfig : IAppConfig
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string Color { get; set; }
    }
}