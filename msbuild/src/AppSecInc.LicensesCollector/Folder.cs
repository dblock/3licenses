using System;

namespace AppSecInc.LicensesCollector
{
    class Folder
    {
        public String Src { get; set; }
        public String Name { get; set; }

        public String Replace(String value)
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                return value
                    .Replace(Src, Name == null ? "" : Name)
                    .Replace(Src.Replace('/', '-'), Name == null ? "" : Name);
            }
        }
    }
}
