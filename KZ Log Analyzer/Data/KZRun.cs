using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KZLogAnalyzer.Data
{
    public enum KZTime { Pro, TP}

    public class KZRun
    {
       public KZTime Typ { get; set; }
        
       public int TeleportCount { get; set; }
        
       public string Time { get; set; } 
    }
}
