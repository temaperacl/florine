using System;
using System.Collections.Generic;
using System.Text;

namespace Florine
{
    class ItemType
    {
        public class IndividualItem
        {
            public Dictionary<string, String> Info = new Dictionary<string, String>();
            public Dictionary<string, Int32> Values = new Dictionary<string, Int32>();
        };
        public List<string> Items = new List<string>();
        private Dictionary<string, Type> _typeLookup = new Dictionary<string, Type>();

        private Dictionary<string, IndividualItem> _contents = new Dictionary<string, IndividualItem>();
        public IndividualItem this[string Name]
        {
            get
            {
                return _contents[Name];
            }
            set
            {
                _contents[Name] = value;
            }
        }
        public static ItemType FromCSV(System.IO.StreamReader sr)
        {
            ItemType dataStore = new ItemType();
            string Header = sr.ReadLine();
            List<string> columns = new List<string>();
            foreach (string s in Header.Split(new char[1] { ',' }))
            {
                string element = s.Trim();
                char elType = element[0];
                string elName = element.Substring(1);
                columns.Add(elName);
                switch(elType)
                {
                    case '$':
                        dataStore._typeLookup[elName] = typeof(String);
                        break;
                    case ':':
                        dataStore._typeLookup[elName] = typeof(Int32);
                        break;
                    default:
                        dataStore._typeLookup[elName] = typeof(String);
                        break;
                }
            }

            while(!sr.EndOfStream)
            {
                IndividualItem i = new IndividualItem();
                string s = sr.ReadLine();
                string [] contents = s.Split(',');

                int idx = 0;
                for(int col = 0; col < columns.Count; ++col)
                {
                    if(idx >= contents.Length)
                    {
                        break;
                    }
                    string colName = columns[col];
                    Type colType = dataStore._typeLookup[colName];

                    string itemString = contents[idx].TrimStart(new char[] { ' ', '\t' });
                    if(itemString[0] == '"')
                    {
                        string curString = itemString.Substring(1);
                        itemString = "";
                        while(!curString.Trim().EndsWith("\""))
                        {
                            itemString += curString;
                            ++idx;
                            if(idx >= contents.Length)
                            {
                                break;
                            }
                            curString = contents[idx];
                        }
                        itemString += curString.Substring(0, curString.Length - 1);
                    }

                    if(colType == typeof(Int32))
                    {
                        i.Values[colName] = Int32.Parse(itemString.Trim());
                    } else
                    {
                        i.Info[colName] = itemString;
                    }
                    ++idx;
                }
                dataStore.Items.Add(i.Info[columns[0]]);
                dataStore[i.Info[columns[0]]] = i;
            }
            return dataStore;
        }


    }
}
