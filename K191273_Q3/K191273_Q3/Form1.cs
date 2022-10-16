using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Windows.Forms;

namespace K191273_Q3
{
    public partial class Form1 : Form
    {
        List<Scripts> listofScripts = new List<Scripts>();
        public Form1()
        {
            InitializeComponent();
        }

        public String extractCategory(String foldername)
        {
            String category = "";
            category = foldername.Substring(foldername.LastIndexOf("\\") + 1);
            return category;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (int i in checkedListBox1.CheckedIndices)
            {
                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
            }
            Form1_Load(sender,e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var path = System.Configuration.ConfigurationManager.AppSettings["folderPath"];
            foreach (string f in Directory.GetDirectories(@path))
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
                    listofScripts.Add(new Scripts(item.Key, item.Value, category));
                }
                checkedListBox1.Items.Add(extractCategory(f));
            }

            dataGridView1.DataSource = listofScripts.Select(c => new { c.Script, c.Price, c.Category }).ToList();
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            List<Scripts> tempList = new List<Scripts>();
            foreach (String indexChecked in checkedListBox1.CheckedItems)
            {
                var searched_category = indexChecked;
                foreach (Scripts script in listofScripts)
                {
                    if (script.Category.ToLower() == searched_category.ToLower())
                    {
                        tempList.Add(new Scripts(script.Script, script.Price, script.Category));
                    }
                }
            }
            dataGridView1.DataSource = tempList.Select(c => new { c.Script, c.Price, c.Category }).ToList();
        }
    }

    public class Scripts
    {
        public String Script { get; set; }
        public String Price { get; set; }
        public String Category { get; set; }

        public Scripts(String Script, String Price, String Category)
        {
            this.Script = Script;
            this.Price = Price;
            this.Category = Category;
        }

    }
}
