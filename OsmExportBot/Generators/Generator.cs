using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmExportBot.Generators
{
    public abstract class Generator
    {
        public abstract string FileExtension { get; set; }

        public abstract string Generate(string coords, string filename, int colour);

        public string GenerateFileName(string rule)
        {
            return string.Format("{6}_{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}{7}",
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                DateTime.Now.Hour,
                DateTime.Now.Minute,
                DateTime.Now.Second,
                rule,
                this.FileExtension
                );
        }
    }
}
