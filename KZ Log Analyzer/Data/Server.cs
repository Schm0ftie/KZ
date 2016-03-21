using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
