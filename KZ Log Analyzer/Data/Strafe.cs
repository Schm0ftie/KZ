using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Strafe(int nr, float sync, float gain, float lost, float maxSpeed, float airTime)
        {
            Nr = nr;
            Sync = sync;
            Gain = gain;
            Lost = lost;
            MaxSpeed = maxSpeed;
            AirTime = airTime;
        }

    }
}
