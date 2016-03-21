using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KZLogAnalyzer.Data;
using System.Xml.Linq;
using System.IO;

namespace KZLogAnalyzer
{
    class XmlHandler
    {
        /// <summary>
        /// Saves a List<Jump> to path in XML format
        /// </summary>
        /// <param name="oJumpList"></param>
        /// <param name="path"></param>
        public void Save(List<Jump> oJumpList, string path)
        {
            FileInfo oFileInfo = new FileInfo(path);
            XDocument oXDocument = new XDocument();
            XElement oXElementRoot = new XElement("Jumps");
            foreach (Jump oJump in oJumpList)
            {
               oXElementRoot.Add(ToXElement(oJump));
            }
            oXDocument.Add(oXElementRoot);
            oXDocument.Save(oFileInfo.FullName);

        }

        /// <summary>
        /// Converts Jump to XElement
        /// </summary>
        /// <param name="oJump"></param>
        /// <returns></returns>
        private XElement ToXElement(Jump oJump)
        {
            XElement oXElement = new XElement(
                oJump.JumpType.ToString(),
                new XAttribute("Distance", oJump.Distance),
                new XAttribute("Pre", oJump.Pre),
                new XAttribute("Max", oJump.Max),
                new XAttribute("StrafeCount", oJump.StrafeCount),
                new XAttribute("Height", oJump.Height),
                new XAttribute("Sync", oJump.Sync),
                new XAttribute("JumpOfEdge", oJump.JumpOffEdge),
                new XAttribute("CrouchJump", oJump.CrouchJump),
                new XAttribute("Forward", oJump.Forward),
                new XAttribute("Bhops", oJump.Bhops),
                new XAttribute("Block", oJump.Block));
            oXElement.Add(ToXElement(oJump.Strafes));
            
            return oXElement;
        }

        /// <summary>
        /// COnverts List of Strafe to List of XElement
        /// </summary>
        /// <param name="oJumpStrafe"></param>
        /// <returns></returns>
        private List<XElement> ToXElement(List<Strafe> oJumpStrafe)
        {
            List<XElement> oXElementList = new List<XElement>();
            foreach (Strafe oStrafe in oJumpStrafe)
            {
                oXElementList.Add(ToXElement(oStrafe));
            }
            return oXElementList;
        }
        
        /// <summary>
        /// Converts a Strafe to XElement
        /// </summary>
        /// <param name="oStrafe"></param>
        /// <returns></returns>
        private XElement ToXElement(Strafe oStrafe)
        {
            XElement oXElement = new XElement("Strafe",
                    new XAttribute("Nr", oStrafe.Nr),
                    new XAttribute("Sync", oStrafe.Sync),
                    new XAttribute("Gain", oStrafe.Gain),
                    new XAttribute("Lost", oStrafe.Gain),
                    new XAttribute("MaxSpeed", oStrafe.MaxSpeed),
                    new XAttribute("AirTime", oStrafe.AirTime));
            return oXElement;
        } 
    }
}
