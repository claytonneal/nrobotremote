using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using System.Linq;

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
		/// Constructor from assembly and type
		/// </summary>
		public KeywordMap(String library, String type)
		{
			if (String.IsNullOrEmpty(library)) throw new ArgumentNullException("Unable to instanciate KeywordMap - no library specified");
			if (String.IsNullOrEmpty(type)) throw new ArgumentNullException("Unable to instanciate KeywordMap - no type specified");
			_library = Assembly.LoadFrom(library);
			_type = _library.GetType(type);
			if (_type==null) throw new Exception(String.Format("Type {0} was not found",type));
			_instance = Activator.CreateInstance(_type);
			_executor = new KeywordExecutor(this,_instance);
			BuildMap();
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
            log.Debug(String.Format("{0} keywords added to map",_keywords.Count));
            log.Debug("Keyword names are:");
            log.Debug(String.Join(",",_keywords.Select(x => x.Value.Name)));
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
		public string[] GetKeywordNames()
		{
			var names = _keywords.Select(x => x.Value.Name);
			return names.ToArray();
		}
		
		
		
	}
}
