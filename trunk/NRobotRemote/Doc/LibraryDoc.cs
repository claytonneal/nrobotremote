using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using log4net;
using System.Text;
using System.Linq;

namespace NRobotRemote.Doc
{
	/// <summary>
	/// Description of XmlDocumentation.
	/// </summary>
	public class LibraryDoc
	{
		
		//log4net
		private static readonly ILog log = LogManager.GetLogger(typeof(LibraryDoc));
		
		private RemoteService _service;
		private XDocument _docxml;
		
		public LibraryDoc(RemoteService service)
		{
			if (service==null) throw new Exception("No Service specified for LibraryDoc");
			_service = service;
			//load file
			_docxml = null;
			if (!String.IsNullOrEmpty(_service._config.docfile)) 
			{
				if (File.Exists(_service._config.docfile)) 
				{
					_docxml = XDocument.Load(_service._config.docfile);
					log.Debug("XML Documentation file loaded");
				}
			}
		}
		
		/// <summary>
		/// Gets xml documentation for specified method
		/// </summary>
		public String GetMethodDoc(MethodInfo method)
		{
			if (_docxml!=null)
			{
				var doc = method.GetXmlDocumentation(_docxml);
				log.Debug(String.Format("Documentation for method {0} : {1}",method.Name,doc));
				return doc;
			}
			else
			{
				log.Warn("No xml documentation file loaded, returning empty string");
				return String.Empty;
			}
		}
		
		/// <summary>
		/// Gets xml documentation of the library class type
		/// </summary>
		/// <returns></returns>
		public String GetLibraryDoc()
		{
			if (_docxml!=null)
			{
				var kwtype = _service._keywordmap.KeywordClassType;
				var doc = kwtype.GetXmlDocumentation(_docxml);
				log.Debug(String.Format("Documentation for library : {0}",doc));
				return doc;	
			}
			else
			{
				log.Warn("No xml documentation file loaded, returning empty string");
				return String.Empty;
			}
		}
			
		
		/// <summary>
		/// Gets a Html table for display of all keyword documentation
		/// </summary>
		public String GetHTMLDoc()
		{
			//setup
			StringBuilder html = new StringBuilder();
			html.Append("<table style=\"text-align: left; width: 90%;\" border=\"1\" cellpadding=\"1\" cellspacing=\"0\">");
			html.Append("<thead><tr style=\" background-color: rgb(153, 153, 153)\">");
			html.Append("<th>Keyword</th><th>Arguments</th><th>Description</th></tr>");
			html.Append("</thead><tbody>");
			var names = _service._keywordmap.GetKeywordNames().ToArray();
			Array.Sort(names);
			foreach(String name in names)
			{
				html.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",name,
				                          String.Join(",",_service._keywordmap.GetKeyword(name).ArgumentNames),
				                          GetMethodDoc(_service._keywordmap.GetKeyword(name).Method)));
			}
			html.Append("</tbody></table>");
			return html.ToString();
		}
		
		
	}
}
