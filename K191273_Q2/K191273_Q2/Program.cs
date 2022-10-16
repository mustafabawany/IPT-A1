using System;
using System.IO;
using System.Collections.Generic;
using AngleSharp;
using System.Xml.Serialization;

namespace K191273_Q2
{

    public class Scripts
    {
        public String Script { get; set; }
        public String Price { get; set; }

    }
    class Program
    {
        public static async void Serialize(String html)
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var doc = await context.OpenAsync(req => req.Content(html));

            var tables = doc.QuerySelectorAll("div");

            foreach (var values in tables)
            {
                List<Scripts> listOfScripts = new List<Scripts>();
                var table_class = values.GetAttribute("class");
                var category = "";
                if (table_class == "table-responsive")
                {
                    // PARSING CATEGORY 

                    var categories = values.QuerySelectorAll("h4");

                    foreach (var indv_category in categories)
                    {
                        category = indv_category.InnerHtml;

                        if (category.Contains("&amp;") || category.Contains("\\") || category.Contains("/") || category.Contains("."))
                        {
                            category = category.Replace("&amp;", "&");
                            category = category.Replace("/", "&");
                            category = category.Replace("\\", "&");
                            category = category.Replace(".", " ");
                        }

                        category = category.Trim();

                        // PARSING SCRIPT AND PRICE 

                        var rows = values.QuerySelectorAll("tr");

                        Dictionary<String, String> DictionaryOfScripts = new Dictionary<String, String>();

                        var currentScript = "";
                        var currentPrice = "";
                        foreach (var row in rows)
                        {
                            var row_class = row.GetAttribute("class");

                            if (row_class == "red-text-td" || row_class == "green-text-td")
                            {
                                var columns = row.QuerySelectorAll("td");
                                int i = 0;

                                foreach (var col in columns)
                                {
                                    if (i == 0)
                                    {
                                        currentScript = col.InnerHtml;
                                    }
                                    else if (i == 5)
                                    {
                                        currentPrice = col.InnerHtml;
                                    }
                                    i = i + 1;
                                }
                            }
                            if (!DictionaryOfScripts.ContainsKey(currentScript) && currentScript != "")
                            {
                                currentScript = currentScript.Trim();
                                currentPrice = currentPrice.Trim();
                                if (currentScript.Contains("&amp;"))
                                {
                                    currentScript = currentScript.Replace("&amp;", "&");
                                }
                                DictionaryOfScripts.Add(currentScript, currentPrice);
                            }
                        }
                        foreach (KeyValuePair<String, String> individualScript in DictionaryOfScripts)
                        {
                            listOfScripts.Add(new Scripts());
                            int length = listOfScripts.Count;
                            listOfScripts[length - 1].Script = individualScript.Key;
                            listOfScripts[length - 1].Price = individualScript.Value;
                        }
                    }

                    string appConfig_Dir = System.Configuration.ConfigurationManager.AppSettings["Path"];

                    Directory.CreateDirectory(appConfig_Dir + category);

                    String current = DateTime.Now.ToString("dd MMM yy");
                    String xmlName = "Summary" + current.Replace(" ", "");
                    XmlSerializer serializer = new XmlSerializer(listOfScripts.GetType());
                    using (StreamWriter writer = new StreamWriter(@appConfig_Dir + category + "\\" + xmlName + ".xml"))
                    {
                        serializer.Serialize(writer, listOfScripts);
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            String html = File.ReadAllText(@"D:\\index.html");
            Serialize(html);
            Console.WriteLine("Files have been created.");
            Console.ReadKey();
        }
    }
}
