using System;
using System.Collections.Generic;


namespace AppSecInc.LicensesCollector
{
    class Externals : Dictionary<String, External>
    {
        public Externals()
        {

        }

        public void AddConfiguredExternal(External external)
        {
            if (ContainsKey(external.Src))
            {
                throw new Exception("Duplicate external '" + external.Src + "'");
            }

            Add(external.Src, external);
        }
    }
}
