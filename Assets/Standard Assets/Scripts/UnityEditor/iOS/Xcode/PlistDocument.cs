using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace UnityEditor.iOS.Xcode
{
	public class PlistDocument
	{
		public PlistElementDict root;

		public string version;

		private static Func<XText, string> __f__am_cache2;

		private static Dictionary<string, int> __f__switch_map0;

		public PlistDocument()
		{
			this.root = new PlistElementDict();
			this.version = "1.0";
		}

		internal static XDocument ParseXmlNoDtd(string text)
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.ProhibitDtd = false;
			xmlReaderSettings.XmlResolver = null;
			XmlReader reader = XmlReader.Create(new StringReader(text), xmlReaderSettings);
			return XDocument.Load(reader);
		}

		internal static string CleanDtdToString(XDocument doc)
		{
			if (doc.DocumentType != null)
			{
				XDocument xDocument = new XDocument(new XDeclaration("1.0", "utf-8", null), new object[]
				{
					new XDocumentType(doc.DocumentType.Name, doc.DocumentType.PublicId, doc.DocumentType.SystemId, null),
					new XElement(doc.Root.Name)
				});
				return string.Concat(new object[]
				{
					string.Empty,
					xDocument.Declaration,
					Environment.NewLine,
					xDocument.DocumentType,
					Environment.NewLine,
					doc.Root
				});
			}
			XDocument xDocument2 = new XDocument(new XDeclaration("1.0", "utf-8", null), new object[]
			{
				new XElement(doc.Root.Name)
			});
			return string.Concat(new object[]
			{
				string.Empty,
				xDocument2.Declaration,
				Environment.NewLine,
				doc.Root
			});
		}

		private static string GetText(XElement xml)
		{
			return string.Join(string.Empty, (from x in xml.Nodes().OfType<XText>()
			select x.Value).ToArray<string>());
		}

		private static PlistElement ReadElement(XElement xml)
		{
			string localName = xml.Name.LocalName;
			switch (localName)
			{
			case "dict":
			{
				List<XElement> list = xml.Elements().ToList<XElement>();
				PlistElementDict plistElementDict = new PlistElementDict();
				if (list.Count % 2 == 1)
				{
					throw new Exception("Malformed plist file");
				}
				for (int i = 0; i < list.Count - 1; i++)
				{
					if (list[i].Name != "key")
					{
						throw new Exception("Malformed plist file");
					}
					string key = PlistDocument.GetText(list[i]).Trim();
					PlistElement plistElement = PlistDocument.ReadElement(list[i + 1]);
					if (plistElement != null)
					{
						i++;
						plistElementDict[key] = plistElement;
					}
				}
				return plistElementDict;
			}
			case "array":
			{
				List<XElement> list2 = xml.Elements().ToList<XElement>();
				PlistElementArray plistElementArray = new PlistElementArray();
				foreach (XElement current in list2)
				{
					PlistElement plistElement2 = PlistDocument.ReadElement(current);
					if (plistElement2 != null)
					{
						plistElementArray.values.Add(plistElement2);
					}
				}
				return plistElementArray;
			}
			case "string":
				return new PlistElementString(PlistDocument.GetText(xml));
			case "integer":
			{
				int v;
				if (int.TryParse(PlistDocument.GetText(xml), out v))
				{
					return new PlistElementInteger(v);
				}
				return null;
			}
			case "true":
				return new PlistElementBoolean(true);
			case "false":
				return new PlistElementBoolean(false);
			}
			return null;
		}

		public void ReadFromFile(string path)
		{
			this.ReadFromString(File.ReadAllText(path));
		}

		public void ReadFromStream(TextReader tr)
		{
			this.ReadFromString(tr.ReadToEnd());
		}

		public void ReadFromString(string text)
		{
			XDocument xDocument = PlistDocument.ParseXmlNoDtd(text);
			this.version = (string)xDocument.Root.Attribute("version");
			XElement xml = xDocument.XPathSelectElement("plist/dict");
			PlistElement plistElement = PlistDocument.ReadElement(xml);
			if (plistElement == null)
			{
				throw new Exception("Error parsing plist file");
			}
			this.root = (plistElement as PlistElementDict);
			if (this.root == null)
			{
				throw new Exception("Malformed plist file");
			}
		}

		private static XElement WriteElement(PlistElement el)
		{
			if (el is PlistElementBoolean)
			{
				PlistElementBoolean plistElementBoolean = el as PlistElementBoolean;
				return new XElement((!plistElementBoolean.value) ? "false" : "true");
			}
			if (el is PlistElementInteger)
			{
				PlistElementInteger plistElementInteger = el as PlistElementInteger;
				return new XElement("integer", plistElementInteger.value.ToString());
			}
			if (el is PlistElementString)
			{
				PlistElementString plistElementString = el as PlistElementString;
				return new XElement("string", plistElementString.value);
			}
			if (el is PlistElementDict)
			{
				PlistElementDict plistElementDict = el as PlistElementDict;
				XElement xElement = new XElement("dict");
				foreach (KeyValuePair<string, PlistElement> current in plistElementDict.values)
				{
					XElement content = new XElement("key", current.Key);
					XElement xElement2 = PlistDocument.WriteElement(current.Value);
					if (xElement2 != null)
					{
						xElement.Add(content);
						xElement.Add(xElement2);
					}
				}
				return xElement;
			}
			if (el is PlistElementArray)
			{
				PlistElementArray plistElementArray = el as PlistElementArray;
				XElement xElement3 = new XElement("array");
				foreach (PlistElement current2 in plistElementArray.values)
				{
					XElement xElement4 = PlistDocument.WriteElement(current2);
					if (xElement4 != null)
					{
						xElement3.Add(xElement4);
					}
				}
				return xElement3;
			}
			return null;
		}

		public void WriteToFile(string path)
		{
			File.WriteAllText(path, this.WriteToString());
		}

		public void WriteToStream(TextWriter tw)
		{
			tw.Write(this.WriteToString());
		}

		public string WriteToString()
		{
			XElement content = PlistDocument.WriteElement(this.root);
			XElement xElement = new XElement("plist");
			xElement.Add(new XAttribute("version", this.version));
			xElement.Add(content);
			XDocument xDocument = new XDocument();
			xDocument.Add(xElement);
			return PlistDocument.CleanDtdToString(xDocument);
		}
	}
}
