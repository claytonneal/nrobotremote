using System;

namespace NRobotRemote
{
	/// <summary>
	/// Description of RemoteServiceConfig.
	/// </summary>
	public class RemoteServiceConfig
	{
		public RemoteServiceConfig()
		{
			docfile = null;
		}
		
		public String library {get; set;}
		public String type {get; set;}
		public int port {get; set;}
		public String docfile {get; set;}
		
		public override string ToString()
		{
			return string.Format("[RemoteService Library={0}, Type={1}, Port={2}, Docfile={3}]", library, type, port, docfile);
		}
		
	}

	
}
