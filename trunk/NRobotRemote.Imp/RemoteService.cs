using System;
using System.IO;
using log4net;
using NRobotRemote.Keywords;
using NRobotRemote.Services;
using NRobotRemote.Doc;
using System.Security.Principal;

namespace NRobotRemote
{
	/// <summary>
	/// Main class for robot service
	/// </summary>
	public class RemoteService
	{
		
		//log4net
		private static readonly ILog log = LogManager.GetLogger(typeof(RemoteService));
		
		//properties
        private HTTPService _httpservice;
        private XmlRpcService _xmlrpcservice;
        private KeywordMap _keywordmap;
        private LibraryDoc _keyworddoc;
		
		
		/// <summary>
		/// Creates a new instance of the robot service
		/// </summary>
		public RemoteService(String library, String type, String port, String docfile = null)
		{
			log.Info(string.Format("[RemoteService Library={0}, Type={1}, Docfile={2}, Port={3}]", library, type, docfile, port));
			//check
			if (String.IsNullOrEmpty(library)) throw new ArgumentNullException("No library specified");
			if (String.IsNullOrEmpty(port)) throw new ArgumentNullException("No Port specified");
			if (String.IsNullOrEmpty(type)) throw new ArgumentNullException("No Type specified");
			if ((!String.IsNullOrEmpty(docfile))&&(!File.Exists(docfile))) throw new Exception("Documentation file not found");
			if (!File.Exists(library)) throw new Exception("Library file not found");
			//setup keyword map
			_keywordmap = new KeywordMap(library,type);
			//setup documentator
			if ((!String.IsNullOrEmpty(docfile))&&(File.Exists(docfile)))
			{
				_keyworddoc = new LibraryDoc(docfile);
			}
			else
			{
				_keyworddoc = null;
			}
			_xmlrpcservice = new XmlRpcService(_keywordmap,_keyworddoc);
			_httpservice = new HTTPService(_xmlrpcservice,Convert.ToInt32(port));
		}
		
		
		
		/// <summary>
		/// Starts the service async
		/// </summary>
		public void StartAsync()
		{
			//check permissions
			if (!IsAdministrator())
			{
				log.Error("Service not started as administrator");
				throw new UnauthorizedAccessException("Service not started as administrator");
			}
			_httpservice.StartAsync();
		}
		
		/// <summary>
		/// Stops the service sync
		/// </summary>
		public void Stop()
		{
			_httpservice.Stop();
		}
		
		/// <summary>
		/// Checks if identity is admin
		/// </summary>
		private bool IsAdministrator()
    	{
	        var identity = WindowsIdentity.GetCurrent();
	        var principal = new WindowsPrincipal(identity);
	        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    	}

		
	}
}
