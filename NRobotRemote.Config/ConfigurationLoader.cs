using System;
using System.Configuration;

namespace NRobotRemote.Config
{

	public class ConfigurationLoader
	{
		/// <summary>
		/// Loads config file into standard config class
		/// </summary>
		public static RemoteServiceConfig GetConfiguration()
		{
			
			var xmlconfig = NRobotRemoteConfiguration.GetConfig();
			var result = new RemoteServiceConfig();
			//get port
			result.port = int.Parse(xmlconfig.Port.Number);
			//get keyword assemblies
			foreach(AssemblyElement xmlasm in xmlconfig.Assemblies)
			{
				result.AddKeywordConfig(new KeywordMapConfig() { Library = xmlasm.Name, Type = xmlasm.Type, DocFile = xmlasm.DocFile } );
			}
			return result;
		}
		
	}
}
