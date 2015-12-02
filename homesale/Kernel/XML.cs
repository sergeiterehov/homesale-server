﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale
{
    class XML
    {
        private Dictionary<string, dynamic> _attr = new Dictionary<string, dynamic>();
        private string _xml = "";
        private string _name;
        private bool _closed;

        public XML(string Name, bool Closed = false)
        {
            this._name = Name;
            this._closed = Closed;
        }

        public XML attr(string Attribute, dynamic Value)
        {
            this._attr[Attribute] = Value.ToString();
            return this;
        }
        public dynamic attr(string Attribute)
        {
            return this._attr[Attribute];
        }

        public XML xml(dynamic Inner)
        {
            this._xml = Inner.ToString();
            return this;
        }
        public string xml()
        {
            return this._xml;
        }

        public XML append(dynamic Inner)
        {
            this._xml += Inner.ToString();
            return this;
        }
        public XML appendTo(XML Parent)
        {
            Parent.append(this);
            return this;
        }

        public override string ToString()
        {
            string attrs = "";
            if (this._attr.Count > 0)
                foreach(KeyValuePair<string, dynamic> item in this._attr)
                {
                    attrs += item.Key + String.Format("\"{0}\"", item.Value);
                }
            if (this._closed)
            {
                return String.Format("<{0}{1} />", this._name, attrs);
            }
            else
            {
                return String.Format("<{0}{1}>{2}</{0}>", this._name, attrs, this._xml);
            }
        }
    }
}