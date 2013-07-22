using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using NRobotRemote.Doc;
using System.Text;

namespace NRobotRemote.Keywords
{
	/// <summary>
	/// Description of KeywordMap.
	/// </summary>
	public class KeywordMap
	{
		
		//log4net
		private static readonly ILog log = LogManager.GetLogger(typeof(KeywordMap));
		
		//private fields
		private Dictionary<String,Keyword> _keywords;
		private Assembly _library;
		private Type _type;
		private Object _instance;
		private KeywordExecutor _executor;
		private XDocument _docxml;
		private KeywordMapConfig _config;
		
		/// <summary>
		/// Keyword executor for the map
		/// </summary>
		public KeywordExecutor Executor
		{
			get
			{
				return _executor;
			}
		}
		
		/// <summary>
		/// Get Type of the keyword class
		/// </summary>
		public Type KeywordClassType
		{
			get
			{
				return _type;
			}
		}
		
		
		/// <summary>
		/// Constructor from assembly and type
		/// </summary>
		public KeywordMap(KeywordMapConfig config)
		{
			//check
			if (String.IsNullOrEmpty(config.Library)) throw new ArgumentNullException("Unable to instanciate KeywordMap - no library specified");
			if (String.IsNullOrEmpty(config.Type)) throw new ArgumentNullException("Unable to instanciate KeywordMap - no type specified");
			_config = config;
			//build map
			_library = Assembly.LoadFrom(_config.Library);
			_type = _library.GetType(_config.Type);
			if (_type==null) throw new Exception(String.Format("Type {0} was not found",_config.Type));
			_instance = Activator.CreateInstance(_type);
			_executor = new KeywordExecutor(this,_instance);
			BuildMap();
			//load doc
			_docxml = null;
			if (!String.IsNullOrEmpty(_config.DocFile)) 
			{
				if (File.Exists(_config.DocFile)) 
				{
					_docxml = XDocument.Load(_config.DocFile);
					log.Debug(String.Format("XML Documentation file loaded : {0}",Path.GetFileName(_config.DocFile)));
				}
				else
				{
					throw new Exception(String.Format("Xml documentation file not found : {0}",_config.DocFile));
				}
			}
		}
		
		/// <summary>
		/// Builds the keyword map
		/// </summary>
		private void BuildMap()
		{
			log.Debug("Building keyword map");
			_keywords = new Dictionary<String,Keyword>();
            var methods = _type.GetMethods().Where((mi) => mi.DeclaringType != typeof(object));
            if (methods != null)
           	{
            	foreach (MethodInfo method in methods)
                {
            		try
            		{
            			var keyword = KeywordFactory.CreateFromMethod(method);
            			if (keyword!=null) 
            			{
            				if (_keywords.ContainsKey(keyword.Name))
            				{
            					throw new DuplicateKeywordException(String.Format("{0} keyword is duplicated",keyword.Name));
            				}
            				_keywords.Add(keyword.Name,keyword);
            			}
            		}
            		catch (Exception e)
            		{
            			log.Error(String.Format("Exception building keyword map : {0}",e.Message));
            			throw e;
            		}
                }
			}
            if (_keywords.Count()==0) throw new Exception("No keywords found");
            log.Debug(String.Format("{0} keywords added to map from type {1}",_keywords.Count,_type.FullName));
            log.Debug("Keyword names are: " + String.Join(",",_keywords.Select(x => x.Value.Name)));
		}
		
		/// <summary>
		/// Gets a keyword based on its name, exception if not found in map
		/// </summary>
		public Keyword GetKeyword(string name)
		{
            //filter on name
            name = KeywordNameParser.ToFriendlyName(name);
            if (_keywords.ContainsKey(name))
            {
            	return _keywords[name];
            }
            else
            {
            	var match = _keywords.Where(x => x.Key.Equals(name));
            	if ((match==null)||(match.Count()==0)) throw new UnknownKeywordException(String.Format("Keyword not in map {0}",name));
            	return match.FirstOrDefault().Value;
            }
		}
		
		/// <summary>
		/// Returns true if map contains keyword with specified name
		/// </summary>
		public Boolean Contains(string name)
		{
			try 
			{
				GetKeyword(name);
				return true;
			}
			catch
			{
				return false;
			}
		}
		
		/// <summary>
		/// Gets all keyword names from the map
		/// </summary>
		public List<String> GetKeywordNames()
		{
			var names = _keywords.Select(x => x.Value.Name);
			return names.ToList();
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
				var doc = _type.GetXmlDocumentation(_docxml);
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
			html.Append("<h3>Keywords From : " + _type.FullName + "</h3>");
			html.Append("<table style=\"text-align: left; width: 90%;\" border=\"1\" cellpadding=\"1\" cellspacing=\"0\">");
			html.Append("<thead><tr style=\" background-color: rgb(153, 153, 153)\">");
			html.Append("<th>Keyword</th><th>Arguments</th><th>Description</th></tr>");
			html.Append("</thead><tbody>");
			var names = GetKeywordNames().ToArray();
			Array.Sort(names);
			foreach(String name in names)
			{
				html.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",name,
				                          String.Join(",",GetKeyword(name).ArgumentNames),
				                          GetMethodDoc(GetKeyword(name).Method)));
			}
			html.Append("</tbody></table>");
			return html.ToString();
		}
		
	}
}
