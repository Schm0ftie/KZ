using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KZLogAnalyzer.Data
{
    public class Strafe
    {
        public int Nr { get; set; }
        public float Sync { get; set; }
        public float Gain { get; set; }
        public float Lost { get; set; }
        public float MaxSpeed { get; set; }
        public float AirTime { get; set; }

        public Strafe()
        {

        }

        public Strafe(int nr, float sync, float gain, float lost, float maxSpeed, float airTime)
        {
            Nr = nr;
            Sync = sync;
            Gain = gain;
            Lost = lost;
            MaxSpeed = maxSpeed;
            AirTime = airTime;
        }

        public XElement ToXElement()
        {
            XElement oXElement = new XElement("Strafe",
                    new XAttribute("Nr", this.Nr),
                    new XAttribute("Sync", this.Sync),
                    new XAttribute("Gain", this.Gain),
                    new XAttribute("Lost", this.Gain),
                    new XAttribute("MaxSpeed", this.MaxSpeed),
                    new XAttribute("AirTime", this.AirTime));
            return oXElement;
        }

    }
}
