using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProgram
{
    class GraphData
    {
        private long ownerId;
        private Dictionary<long, List<long>> rel;

        public GraphData(long ownerId, Dictionary<long,List<long>> rel)
        {
            this.ownerId = ownerId;
            this.rel = rel;
        }

        public long OwnerId
        {
            get { return this.ownerId; }
        }

        public Dictionary<long,List<long>> Reatonships
        {
            get { return this.rel; }
            //set { this.rel = value; }
        }
    }
}
