using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KZLogAnalyzer.Data
{
    /// <summary>
    /// Describes a kz server
    /// </summary>
    public class Server
    {
        public string IP { get; set; }

        public string Name { get; set; }

        public List<Map> Maps { get; set; }

        public float Tickrate { get; set; }

        public string KZTimerVersion { get; set; }

        public Server()
        {
            Maps = new List<Map>();
        }

        public XElement ToXElement()
        {
            XElement oXElement = new XElement("Server",
                    new XAttribute("IP", this.IP),
                    new XAttribute("Name", (this.Name == null) ? String.Empty : this.Name),
                    new XAttribute("Tickrate", this.Tickrate),
                    new XAttribute("KZTimerVersion", (this.KZTimerVersion == null) ? String.Empty : this.KZTimerVersion),
                    new XElement("Maps", XmlHandler.ToXElement(Maps)));
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
            Server s = obj as Server;
            if ((System.Object)s == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (IP == s.IP && Tickrate == s.Tickrate);
        }
    }
}
