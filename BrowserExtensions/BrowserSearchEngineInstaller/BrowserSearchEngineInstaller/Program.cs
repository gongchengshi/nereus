using System;

namespace BrowserSearchEngineInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var firefox = new Firefox();
                firefox.Install();
                // firefox.SetAsDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while installing search engine for Firefox\n" +
                    "Most likely, Firefox is not installed. See error:\n\n" + ex);
            }

            try
            {
                var chrome = new Chrome();
                chrome.Install();
                // chrome.SetAsDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while installing search engine for Chrome\n" +
                    "Most likely, Chrome is not installed. See error:\n\n" + ex);
            }
        }
    }

    public interface ISearchEngineInstall
    {
        void Install();
        void SetAsDefault();
    }
}
