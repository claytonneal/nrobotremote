using System;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using NRobotRemote.Config;

namespace NRobotRemote.Domain
{
	/// <summary>
	/// Builds a keyword map instance
	/// </summary>
	public class KeywordMapBuilder : MarshalByRefObject
	{
		
		public KeywordMapBuilder()
		{
		}
		
		public KeywordMap CreateMap(KeywordMapConfig config)
		{
			try
			{
				//check
				if (String.IsNullOrEmpty(config.Library)) throw new ArgumentNullException("Unable to instanciate KeywordMap - no library specified");
				if (String.IsNullOrEmpty(config.Type)) throw new ArgumentNullException("Unable to instanciate KeywordMap - no type specified");
				var result = new KeywordMap();
				result._config = config;
				//build map
				result._library = Assembly.LoadFrom(result._config.Library);
				result._type = result._library.GetType(result._config.Type);
				if (result._type==null) throw new Exception(String.Format("Type {0} was not found",result._config.Type));
				result._instance = Activator.CreateInstance(result._type);
				result._executor = new KeywordExecutor(result,result._instance);
				result.BuildMap();
				//load xml doc
				XDocument xmldoc = null;
				if (!String.IsNullOrEmpty(result._config.DocFile)) 
				{
					if (File.Exists(result._config.DocFile)) 
					{
						xmldoc = XDocument.Load(result._config.DocFile);
					}
					else
					{
						throw new Exception(String.Format("Xml documentation file not found : {0}",result._config.DocFile));
					}
				}
				//get doc from xml
				if (xmldoc!=null)
				{
					//library
					result._doc = result._type.GetXmlDocumentation(xmldoc);
					//keywords
					foreach(Keyword key in result.GetKeywords())
					{
						key._doc = key.Method.GetXmlDocumentation(xmldoc);
					}
				}
				return result;
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
		
		
	}
}
