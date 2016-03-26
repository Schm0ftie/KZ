using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KZLogAnalyzer.Data;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace KZLogAnalyzer
{
    class XmlHandler
    {
        /// <summary>
        /// Saves a List<Jump> to path in XML format
        /// </summary>
        /// <param name="oJumpList"></param>
        /// <param name="path"></param>
        public void Save(List<Server> oServers, string path)
        {
            FileInfo oFileInfo = new FileInfo(path);
            XDocument oXDocument = new XDocument();
            XElement oXElementRoot = new XElement("Data");
            foreach (Server oServer in oServers)
            {
               oXElementRoot.Add(oServer.ToXElement());
            }
            oXDocument.Add(oXElementRoot);
            oXDocument.Save(oFileInfo.FullName);

        }



        /// <summary>
        /// COnverts List of Strafe to List of XElement
        /// </summary>
        /// <param name="oJumpStrafe"></param>
        /// <returns></returns>
        public static List<XElement> ToXElement(List<Strafe> oJumpStrafe)
        {
            List<XElement> oXElementList = new List<XElement>();
            foreach (Strafe oStrafe in oJumpStrafe)
            {
                oXElementList.Add(oStrafe.ToXElement());
            }
            return oXElementList;
        }

        public static List<XElement> ToXElement(List<KZRun> oKZRuns)
        {
            List<XElement> oXElementList = new List<XElement>();
            foreach (KZRun oKZRun in oKZRuns)
            {
                oXElementList.Add(oKZRun.ToXElement());
            }
            return oXElementList;
        }

        public static List<XElement> ToXElement(List<Jump> oJumps)
        {
            List<XElement> oXElementList = new List<XElement>();
            foreach (Jump oJump in oJumps)
            {
                oXElementList.Add(oJump.ToXElement());
            }
            return oXElementList;
        }

        public static List<XElement> ToXElement(List<Map> oMaps)
        {
            List<XElement> oXElementList = new List<XElement>();
            foreach (Map oMap in oMaps)
            {
                oXElementList.Add(oMap.ToXElement());
            }
            return oXElementList;
        }

        public static List<XElement> ToXElement(List<Server> oServers)
        {
            List<XElement> oXElementList = new List<XElement>();
            foreach (Server oServer in oServers)
            {
                oXElementList.Add(oServer.ToXElement());
            }
            return oXElementList;
        }


        public List<Server> Load(string path)
        {
            FileInfo oFileInfo = new FileInfo(path);
            if (!oFileInfo.Exists)
                return new List<Server>();

           XDocument xDoc = XDocument.Load(path);
            List<Server> oServerList = xDoc.Element("Data").Elements("Server").Select(s => new Server()
            {
                IP = s.Attribute("IP").Value,
                Name = s.Attribute("Name").Value,
                Tickrate = float.Parse(s.Attribute("Tickrate").Value, CultureInfo.InvariantCulture),
                KZTimerVersion = s.Attribute("KZTimerVersion").Value,
                Maps = s.Element("Maps").Elements().Select(m => new Map()
                {
                    Name = m.Attribute("Name").Value,
                    WorkshopPreFix = m.Attribute("WorkshopPreFix").Value,
                    Times = m.Element("Times").Elements().Select(r => new KZRun()
                    {
                        Typ = (KZTime)Enum.Parse(typeof(KZTime), r.Attribute("Typ").Value),
                        Runner = r.Attribute("Runner").Value,
                        TeleportCount = int.Parse(r.Attribute("TeleportCount").Value),
                        Time = r.Attribute("Time").Value
                    }).ToList(),

                    Jumps = m.Element("Jumps").Elements().Select(j => new Jump((JumpType)Enum.Parse(typeof(JumpType), j.Name.ToString()))
                    {
                        PlayerName = j.Attribute("PlayerName").Value,
                        Distance = float.Parse(j.Attribute("Distance").Value, CultureInfo.InvariantCulture),
                        Pre = float.Parse(j.Attribute("Pre").Value, CultureInfo.InvariantCulture),
                        Max = float.Parse(j.Attribute("Max").Value, CultureInfo.InvariantCulture),
                        StrafeCount = int.Parse(j.Attribute("StrafeCount").Value, CultureInfo.InvariantCulture),
                        Height = float.Parse(j.Attribute("Height").Value, CultureInfo.InvariantCulture),
                        Sync = float.Parse(j.Attribute("Sync").Value, CultureInfo.InvariantCulture),
                        JumpOfEdge = float.Parse(j.Attribute("JumpOfEdge").Value, CultureInfo.InvariantCulture),
                        CrouchJump = bool.Parse(j.Attribute("CrouchJump").Value),
                        Forward = bool.Parse(j.Attribute("Forward").Value),
                        Bhops = int.Parse(j.Attribute("Bhops").Value, CultureInfo.InvariantCulture),
                        Block = float.Parse(j.Attribute("Block").Value, CultureInfo.InvariantCulture),
                        Strafes = j.Elements().Select(st => new Strafe(
                            int.Parse(st.Attribute("Nr").Value),
                            float.Parse(st.Attribute("Sync").Value),
                            float.Parse(st.Attribute("Gain").Value),
                            float.Parse(st.Attribute("Lost").Value),
                            float.Parse(st.Attribute("MaxSpeed").Value),
                            float.Parse(st.Attribute("AirTime").Value)))
                        .ToList()
                    }).ToList()
                }).ToList()
            }).ToList();
            return oServerList;
        }
    }
}
