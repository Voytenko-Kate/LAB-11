using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WpfDatabase.Model;

namespace WpfDatabase
{
    class ExportProvider<T>
    {
        protected IEnumerable<T> data;
        public ExportProvider(IEnumerable<T> data)
        {
            this.data = data;
        }
        public virtual void ExportTo(string filename)
        {
            StreamWriter output = new StreamWriter(File.OpenWrite(filename));
            foreach (var value in data)
                output.WriteLine(value);
            output.Close();
        }
    }
}
