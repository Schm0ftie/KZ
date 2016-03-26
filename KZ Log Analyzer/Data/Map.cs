using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KZLogAnalyzer.Data
{
    public class Map
    {
        public string Name
        {
            get; set;
        }

        public string WorkshopPreFix { get; set; }

        public string FullName
        {
            get
            {
                return WorkshopPreFix + Name;
            }
        }


        public List<Jump> Jumps { get; set; }

        public List<KZRun> Times { get; set; }

        public Map(){
            Jumps = new List<Jump>();
            Times = new List<KZRun>();
        }

        public XElement ToXElement()
        {
            XElement oXElement = new XElement("Map",
                    new XAttribute("Name", this.Name),
                    new XAttribute("WorkshopPreFix", this.WorkshopPreFix),
                    new XElement("Times", XmlHandler.ToXElement(Times)),
                    new XElement("Jumps", XmlHandler.ToXElement(Jumps)));
            return oXElement;
        }
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Map m = obj as Map;
            if ((System.Object)m == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (FullName == m.FullName);
        }
    }
}
