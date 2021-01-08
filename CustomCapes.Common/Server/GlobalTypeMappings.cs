using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CustomCapes.Common.Server {

    public static class GlobalTypeMappings {

        private static readonly IDictionary<string, string> _mappings = new Dictionary<string, string> {
            {".png", "image/png"},
            {".jpeg", "image/jpeg"},
            {".jpg", "image/jpeg"},
            {".css", "text/css"},
            {".html", "text/html"}
        };

        public static IDictionary<string, string> Mappings => _mappings;
    }

}