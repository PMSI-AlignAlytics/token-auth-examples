using System;
using System.Linq;
using Owin;

namespace WebApp
{
    public interface IAppConfig
    {
        string Name { get; set; }
        string Key { get; set; }
        string DisplayName { get; set; }
        string Color { get; set; }
    }
}