using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KZLogAnalyzer.Data
{
    public class Map
    {
        public string Name { get; set; }

        public List<Jump> Jumps { get; set; }

        public List<KZRun> Times { get; set; }
    }
}
