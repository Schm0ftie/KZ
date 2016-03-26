using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.ComponentModel;

namespace KZLogAnalyzer.Data
{
    public enum JumpType { LongJump, Bhop, MultiBhop, DropBhop, WeirdJump, LadderJump, CountJump }

    public class Jump 
    {

        public string PlayerName { get; set; }

        public float Pre { get; set; }
        public float Height { get; set; }


        private float _Sync;
        public float Sync { get
            {
                float result = 0;
                foreach(Strafe oStrafe in Strafes)
                {
                    result += oStrafe.Sync;
                }
                return Strafes.Count > 0 ? result /= Strafes.Count : _Sync;
            }
            set
            {
                _Sync = value;
            }
        }

        public float Distance { get; set; }

        public JumpType JumpType { get; }

        private float _Max;
        public float Max {
            get
            {
                float result = 0;
                foreach(Strafe oStrafe in Strafes)
                {
                    if(oStrafe.MaxSpeed > result)
                        result = oStrafe.MaxSpeed;
                }
                return result > 0 ? result : _Max;
            }
            set
            {
                _Max = value;
            }

        }

        private int _StrafeCount;
        public int StrafeCount
        {
            get
            {
                return Strafes.Count > 0 ? Strafes.Count : _StrafeCount;
            }
            set
            {
                _StrafeCount = value;

            }
        }

        public List<Strafe> Strafes { get; set; }

        public float JumpOfEdge { get; set; }

        public bool CrouchJump { get; set; }

        public bool Forward { get; set; }

        public float Block { get; set; }

        public int Bhops { get; set; }

        public Jump(JumpType jumpType)
        {
            //[KZ] schm0ftie jumped 261.4541 units with a LongJump [3 Strafes | 275.825 Pre | 339 Max | Height 65.9 | 87% Sync | JumpOff Edge 7.585 | CrouchJump: yes | -Forward: yes] [240 block]
            JumpType = jumpType;
            Strafes = new List<Strafe>();
           
        }

        /// <summary>
        /// Converts Jump to XElement
        /// </summary>
        /// <param name="oJump"></param>
        /// <returns></returns>
        public XElement ToXElement()
        {
            XElement oXElement = new XElement(
                this.JumpType.ToString(),
                new XAttribute("PlayerName", this.PlayerName),
                new XAttribute("Distance", this.Distance),
                new XAttribute("Pre", this.Pre),
                new XAttribute("Max", this.Max),
                new XAttribute("StrafeCount", this.StrafeCount),
                new XAttribute("Height", this.Height),
                new XAttribute("Sync", this.Sync),
                new XAttribute("JumpOfEdge", this.JumpOfEdge),
                new XAttribute("CrouchJump", this.CrouchJump),
                new XAttribute("Forward", this.Forward),
                new XAttribute("Bhops", this.Bhops),
                new XAttribute("Block", this.Block));
            oXElement.Add(XmlHandler.ToXElement(this.Strafes));

            return oXElement;
        }

    }
}
