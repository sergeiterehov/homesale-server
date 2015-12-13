using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace homesale
{
    public class Assoc : XElement
    {
        Func<Assoc, string> ToStringCallback = null;

        public Assoc(string Name = "Assoc") : base(XName.Get(Name))
        {

        }

        public Assoc(XElement xelement) : base(xelement)
        {

        }

        public Assoc Add(string Name, dynamic Value)
        {
            base.Add(new XElement(XName.Get(Name), Value));

            return this;
        }

        public Assoc FromXml(IEnumerable<XElement> xml)
        {
            RemoveAll();
            foreach (var e in xml)
            {
                Add(e);
            }
            return this;
        }

        public XElement ToXml(string Name)
        {
            this.Name = XName.Get(Name);
            return this;
        }

        public List<Assoc> List()
        {
            var result = new List<Assoc>();

            foreach (var item in this.Elements())
            {
                result.Add(new Assoc(item));
            }

            return result;
        }

        public Assoc this[string Key]
        {
            get
            {
                return this.Get(Key);
            }
        }

        public object this[string Key, Type GetType]
        {
            get
            {
                return Convert.ChangeType(this.Get<object>(Key), GetType);
            }
        }

        public T Get<T>(string Key)
        {
            return (T)Convert.ChangeType(this.Element(XName.Get(Key)).Value, typeof(T));
        }

        public Assoc Get(string Key)
        {
            return new Assoc(Key).FromXml(this.Element(XName.Get(Key)).Elements());
        }

        public bool Has(string Key)
        {
            return this.Element(XName.Get(Key)) != null;
        }

        public void Clear()
        {
            this.Elements().Remove();
        }

        public List<Assoc> GetList(Func<Assoc, string> callbackToString = null)
        {
            var result = new List<Assoc>();

            foreach (var item in this.Elements())
            {
                result.Add(new Assoc(item).ToString(callbackToString));
            }

            return result;
        }

        public Assoc ToString(Func<Assoc, string> callbackToString)
        {
            this.ToStringCallback = callbackToString;
            return this;
        }
        public override string ToString()
        {
            return null == this.ToStringCallback ? base.ToString() : ToStringCallback(this);
        }

        public string GetInnerString()
        {
            string result = "";

            foreach (var item in this.Elements())
            {
                result += item.ToString();
            }

            return result;
        }

        public string InnerString
        {
            get
            {
                return this.GetInnerString();
            }
        }
    }
}
