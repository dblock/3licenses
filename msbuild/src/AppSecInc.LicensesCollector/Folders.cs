using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppSecInc.LicensesCollector
{
    class Folders : List<Folder>
    {
        public Folders()
        {

        }

        public void AddConfiguredFolder(Folder folder)
        {
            Add(folder);
        }
    }
}
