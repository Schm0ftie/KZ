using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KZLogAnalyzer.Data
{
    public enum KZTime { PRO, TP}

    public class KZRun
    {
       public KZTime Typ { get; set; }

       public string Runner { get; set; }

       public int TeleportCount { get; set; }
        
       public string Time { get; set; }

        public XElement ToXElement()
        {
            XElement oXElement = new XElement("KZTime",
                    new XAttribute("Typ", this.Typ.ToString()),
                    new XAttribute("Runner", this.Runner),
                    new XAttribute("TeleportCount", this.TeleportCount),
                    new XAttribute("Time", this.Time));
            return oXElement;
        }
    }
}
