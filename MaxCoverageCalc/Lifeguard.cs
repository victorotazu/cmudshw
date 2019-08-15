using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxCoverageCalc
{
    class Lifeguard
    {
        int id;
        int startTimeUnit;
        int endTimeUnit;

        public Lifeguard(int id)
        {
            this.id = id;
        }

        public int Id { get => id; set => id = value; }
        public int StartTimeUnit { get => startTimeUnit; set => startTimeUnit = value; }
        public int EndTimeUnit { get => endTimeUnit; set => endTimeUnit = value; }
        
    }
}
