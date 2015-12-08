using System;
using System.Xml.Linq;
using System.Collections.Generic;

namespace homesale
{
    abstract class Request
    {
        static public bool has(string FieldName)
        {
            var XML = App.Main.ME().Query.XML;
            return XML.Element(XName.Get("call")).Element(XName.Get(FieldName)) != null;
        }

        static public FieldType input<FieldType>(string FieldName)
        {
            if (has(FieldName))
            {
                var XML = App.Main.ME().Query.XML;
                return (FieldType) Convert.ChangeType(XML.Element(XName.Get("call")).Element(XName.Get(FieldName)).Value, typeof(FieldType));
            }
            else
            {
                return (dynamic)null;
            }
        }
        static public object input(string FieldName)
        {
            return input<object>(FieldName);
        }
    }
}
