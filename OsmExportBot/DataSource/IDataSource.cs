using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot.DataSource
{
    interface IDataSource
    {
        Uri OverpassApiUri { get; }

        string BuildQuery(string name, float lat, float lon);

        string RunQuery(string query);
    }
}
