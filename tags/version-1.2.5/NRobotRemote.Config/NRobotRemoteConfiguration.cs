using System;
using System.Configuration;

namespace NRobotRemote.Config
{
	/// <summary>
	/// Configuration section handler
	/// </summary>
	public class NRobotRemoteConfiguration : ConfigurationSection
	{
		
		private const string cConfigSection = "NRobotRemoteConfiguration";
		
		public static NRobotRemoteConfiguration GetConfig()
		{
			return (NRobotRemoteConfiguration)ConfigurationManager.GetSection(cConfigSection) ?? new NRobotRemoteConfiguration();
		}
		
		[ConfigurationProperty("assemblies")]
		public AssemblyElementCollection Assemblies
		{
			get
			{
				return (AssemblyElementCollection)this["assemblies"] ?? new AssemblyElementCollection();
			}
		}
		
		[ConfigurationProperty("port")]
		public PortElement Port
		{
			get
			{
				return (PortElement)this["port"] ?? new PortElement();
			}
		}
		
	}
}
