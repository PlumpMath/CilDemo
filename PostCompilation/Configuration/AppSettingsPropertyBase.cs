using System.Configuration;
using System.IO;
using System.Reflection;

namespace PostCompilation.Configuration
{
    abstract class AppSettingsPropertyBase
    {
        public static AppSettingsSection AppSettings { get; private set; }



        static AppSettingsPropertyBase()
        {
            var thisFilename = Assembly.GetAssembly(typeof(AppSettingsPropertyBase)).Location;
            var otherFilename = Assembly.GetExecutingAssembly().Location;
            var filename = File.Exists(thisFilename) ? thisFilename : otherFilename;
            AppSettings = ConfigurationManager.OpenExeConfiguration(filename).AppSettings;
        }
    }
}
