using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Xml;
using K191273_Qs4.Models;

namespace K191273_Qs4.Controllers
{
    public class MarketSummaryController : Controller
    {
        List<MarketSummary> listofScripts = new List<MarketSummary>();
        public String extractCategory(String foldername)
        {
            String category = "";
            category = foldername.Substring(foldername.LastIndexOf("\\") + 1);
            return category;
        }

        // GET: MarketSummary/All
        public ActionResult All()
        {
            var path = System.Web.Configuration.WebConfigurationManager.AppSettings["folderPath"].ToString();
            foreach (string f in Directory.GetDirectories(@"E:\K191273_MustafaBawany\Categories"))
            {
                var filename = Directory.GetFiles(f);

                Dictionary<String, String> DictionaryOfScripts = new Dictionary<string, string>();

                using (XmlReader reader = XmlReader.Create(@filename[0]))
                {
                    var currentScript = "";
                    var currentPrice = "";
                    var temp = "";
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "Script":
                                    currentScript = reader.ReadString().ToString();
                                    break;
                                case "Price":
                                    currentPrice = reader.ReadString().ToString();
                                    break;
                            }
                        }
                        if (!DictionaryOfScripts.ContainsKey(currentScript) && currentScript != "" && currentPrice != "" && currentPrice != temp)
                        {
                            temp = currentPrice;
                            DictionaryOfScripts.Add(currentScript, currentPrice);
                        }
                    }
                }
                foreach (KeyValuePair<String, String> item in DictionaryOfScripts)
                {
                    var category = extractCategory(f);
                    listofScripts.Add(new MarketSummary(item.Key, item.Value, category));
                }
            }

            return PartialView(listofScripts.ToList());
        }

        // GET MarketSummary/Categories/id
        public ActionResult Categories(String id)
        {
            if (listofScripts.Count() == 0)
            {
                All();
            }
            
            var searched_category = id;
            List<MarketSummary> tempList = new List<MarketSummary>();
            var temp = "";
            foreach (MarketSummary script in listofScripts)
            {
                temp = script.Category.Replace("&", "and");
                if (script.Category.ToLower() == searched_category.ToLower() || temp.ToLower() == searched_category.ToLower())
                {
                    tempList.Add(new MarketSummary(script.Script, script.Price, script.Category));
                }
            }

            return View(tempList.ToList());
        }
    }
}