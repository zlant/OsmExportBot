using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot
{
    static class UserState
    {
        public static ConcurrentDictionary<long, string> NewRule { get; set; }
            = new ConcurrentDictionary<long, string>();

        public static ConcurrentDictionary<long, string> CurrentRule { get; set; }
            = new ConcurrentDictionary<long, string>();
    }
}
