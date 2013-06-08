using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using log4net;

namespace NRobotRemote.Doc
{
	/// <summary>
	/// Description of XmlDocumentation.
	/// </summary>
	public class LibraryDoc
	{
		
		//log4net
		private static readonly ILog log = LogManager.GetLogger(typeof(LibraryDoc));
		
		private String _docfile;
		private XDocument _docxml;
		
		public LibraryDoc(String docfile)
		{
			if (String.IsNullOrEmpty(docfile)) throw new ArgumentNullException("Documentation file is null");
			if (!File.Exists(docfile)) throw new FileNotFoundException("Documentation file not found");
			_docfile = docfile;
			_docxml = XDocument.Load(docfile);
			log.Debug("XML Documentation file loaded");
		}
		
		/// <summary>
		/// Gets xml documentation for specified method
		/// </summary>
		public String GetMethodDoc(MethodInfo method)
		{
			var doc = method.GetXmlDocumentation(_docxml);
			log.Debug(String.Format("Documentation for method {0} : {1}",method.Name,doc));
			return doc;
		}
		
	}
}
