using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Dynamic;
using System.ComponentModel;

namespace CommonLib.Data.Xml
{
	public class DynamicXElement : DynamicObject
	{
		public DynamicXElement() { }

		public DynamicXElement(XElement element) {
			this.XElement = element;
		}

		public DynamicXElement(String name) {
			this.XElement = new XElement(name);
		}

		public XElement XElement { get; protected set; }

		private Dictionary<string, Type> _elementTypes = new Dictionary<string,Type>();
		public Dictionary<string, Type> ElementTypes { get { return _elementTypes; } }

		public override bool TrySetMember(SetMemberBinder binder, object value) {
			var setElement = this.XElement.Element(binder.Name);
			if (setElement != null)
				setElement.SetValue(value);
			else {
				if (value.GetType() == typeof(DynamicXElement))
					this.XElement.Add(new XElement(binder.Name));
				else
					this.XElement.Add(new XElement(binder.Name, value));
			}
			return true;
		}

		public override bool TryConvert(ConvertBinder binder, out object result) {
			result = this.ConvertType(binder.Type, this.XElement.Value);
			return true;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result) {
			var getElement = this.XElement.Element(binder.Name);
			if (getElement != null) {
				if (!_elementTypes.ContainsKey(binder.Name)) {
					result = new DynamicXElement(getElement);
				}
				else {
					result = this.ConvertType(_elementTypes[binder.Name], getElement.Value);
				}
				return true;
			}
			else {
				result = null;
				return false;
			}
		}

		protected object ConvertType(Type t, string data) {
			return TypeDescriptor.GetConverter(t).ConvertFromString(data);
		}
	}
}
