using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Text;
using NRobotRemote.Config;

namespace NRobotRemote.Domain
{
	/// <summary>
	/// Description of KeywordMap.
	/// </summary>
	public class KeywordMap : MarshalByRefObject
	{
			
		//fields
		private Dictionary<String,Keyword> _keywords;
		public String _doc;
		internal Assembly _library;
		internal Type _type;
		internal Object _instance;
		internal KeywordExecutor _executor;
		internal KeywordMapConfig _config;
		
		
		
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
		/// Default constructor
		/// </summary>
		public KeywordMap()
		{
			_doc = String.Empty;
		}
		
		/// <summary>
		/// Builds the keyword map
		/// </summary>
		internal void BuildMap(BuildMapOptions options)
		{
			_keywords = new Dictionary<String,Keyword>();
            var methods = _type.GetMethods().Where((mi) => mi.DeclaringType != typeof(object));
            if (methods != null)
           	{
            	foreach (MethodInfo method in methods)
                {
            		try
            		{
            			var keyword = KeywordFactory.CreateFromMethod(method, options);
            			if (keyword!=null) 
            			{
            				if (_keywords.ContainsKey(keyword.Name))
            				{
            					throw new Exception(String.Format("{0} keyword is duplicated",keyword.Name));
            				}
            				_keywords.Add(keyword.Name,keyword);
            			}
            		}
            		catch (Exception e)
            		{
            			throw e;
            		}
                }
			}
            if (_keywords.Count()==0) throw new Exception("No keywords found");
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
		/// Gets all keywords from the map
		/// </summary>
		public List<Keyword> GetKeywords()
		{
			var keywords = _keywords.Select(x => x.Value);
			return keywords.ToList();
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
				                          GetKeyword(name)._doc));
			}
			html.Append("</tbody></table>");
			return html.ToString();
		}
		
		/// <summary>
		/// Executes a keyword within the map
		/// </summary>
		public KeywordResult ExecuteKeyword(string name, object[] args)
		{
			return _executor.ExecuteKeyword(name,args);
		}
		
		/// <summary>
		/// Gets the app domain of the keyword map
		/// </summary>
		public AppDomain GetDomain()
		{
			return AppDomain.CurrentDomain;
		}
		
		/// <summary>
		/// Gets the argument names of a keyword in the map
		/// </summary>
		public string[] GetKeywordArguments(string keyword)
		{
			return GetKeyword(keyword).ArgumentNames;
		}
		
		/// <summary>
		/// Gets documentation for a keyword
		/// </summary>
		public string GetKeywordDoc(string keyword)
		{
			var kwd = GetKeyword(keyword);
			return kwd._doc;
				
		}
		
	}
}
