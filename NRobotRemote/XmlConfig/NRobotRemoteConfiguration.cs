using System.Configuration;

namespace NRobotRemote.XmlConfig
{
	/// <summary>
	/// Configuration section handler
	/// </summary>
	public class NRobotRemoteConfiguration : ConfigurationSection
	{
		
		private const string CConfigSection = "NRobotRemoteConfiguration";
		
		public static NRobotRemoteConfiguration GetConfig()
		{
			return (NRobotRemoteConfiguration)ConfigurationManager.GetSection(CConfigSection) ?? new NRobotRemoteConfiguration();
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
