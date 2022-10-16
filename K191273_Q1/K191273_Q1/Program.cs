using System;
using System.IO;
using System.Net;
using System.Text;

namespace K191273_Q1
{
    class Program
    {
        static void Main(string[] args)
        {
            String websiteLink = args[0];
            String dirPath = args[1];
            WebClient client = new WebClient();
            try 
            {
                client.DownloadFile(websiteLink, @dirPath + "\\index.html");
                Console.WriteLine(dirPath + " - will contain a file named index.html");
            }
            catch (System.Net.WebException)
            {
                Console.WriteLine("Directory Not Found");
            }
            client.Dispose();
        }
    }
}
