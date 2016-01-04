using System;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using NRobotRemote.Config;

namespace NRobotRemote.Domain
{
	
	/// <summary>
	/// Options to use when building keyword map
	/// </summary>
	public enum BuildMapOptions
	{
		OnlyStatic,
		StaticAndInstance
	}
	
	
	/// <summary>
	/// Builds a keyword map instance
	/// </summary>
	public class KeywordMapBuilder : MarshalByRefObject
	{
		
		public KeywordMapBuilder()
		{
		}
		
		//fields
		private static KeywordMapConfig _config;
		
		public KeywordMap CreateMap(KeywordMapConfig config)
		{
			try
			{
				//check
				_config = config;
				if (String.IsNullOrEmpty(config.Library)) throw new ArgumentNullException("Unable to instanciate KeywordMap - no library specified");
				if (String.IsNullOrEmpty(config.Type)) throw new ArgumentNullException("Unable to instanciate KeywordMap - no type specified");
				var result = new KeywordMap();
				result._config = config;
				//load assembly
				AppDomain.CurrentDomain.AssemblyResolve += KeywordAssemblyResolveHandler;
				if (File.Exists(config.Library))
				{
					//load from path
					result._library = Assembly.LoadFrom(result._config.Library);
				}
				else
				{
					//load from assembly name
					result._library = Assembly.Load(result._config.Library);
				}
				result._type = result._library.GetType(result._config.Type);
				if (result._type==null) throw new Exception(String.Format("Type {0} was not found",result._config.Type));
				//create instance
				try
				{
					//if can create instance build map of instance and static methods
					result._instance = Activator.CreateInstance(result._type);
					result._executor = new KeywordExecutor(result,result._instance);
					result.BuildMap(BuildMapOptions.StaticAndInstance);
				}
				catch
				{
					//if can't create instance create map of only static methods
					result._instance = null;
					result._executor = new KeywordExecutor(result,null);
					result.BuildMap(BuildMapOptions.OnlyStatic);
				}
				
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
		
		/// <summary>
		/// Handles AssemblyResolve event
		/// This is needed if keyword assembly has dependencies
		/// We attempt to load assembly from same directory as the keyword assembly
		/// </summary>
		public static Assembly KeywordAssemblyResolveHandler(object source, ResolveEventArgs e)
		{
			try
			{
				Assembly result = null;
				//check if library specified includes a path
				if (_config.Library.Contains("\\"))
				{
					var libpath = Path.GetDirectoryName(_config.Library);
					if (!String.IsNullOrEmpty(libpath))
					{
						var asmname = new AssemblyName(e.Name);
						var asmpath = Path.Combine(libpath,asmname.Name);
						result= Assembly.LoadFrom(asmpath);
					}
				}
				return result;
			}
			catch
			{
				return null;
			}
			
		}
		
		
	}
}
