using System;
using System.Collections.Generic;


namespace AppSecInc.LicensesCollector
{
    public class ExternalsDictionary : Dictionary<String, External>
    {
        public ExternalsDictionary()
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
