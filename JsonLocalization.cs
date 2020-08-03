using System.Collections.Generic;

namespace I18N
{
    internal class JsonLocalization
    {
        public string Key { get; set; }
        public Dictionary<string, string> LocalizedValues { get; set; }
    }
}