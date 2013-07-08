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
        internal HTTPService _httpservice;
        internal XmlRpcService _xmlrpcservice;
        internal KeywordMap _keywordmap;
        internal LibraryDoc _keyworddoc;
        internal RemoteServiceConfig _config;
        
        /// <summary>
        /// Constructor with direct arguments
        /// </summary>
        public RemoteService(String Library, String Type, String Port, String Docfile = null) : this(new RemoteServiceConfig {library = Library, type = Type, port = int.Parse(Port), docfile = Docfile } )
        {
        }
        
        
		/// <summary>
		/// Creates a new instance of the robot service
		/// </summary>
		public RemoteService(RemoteServiceConfig config)
		{
			//check
			if (config==null) throw new ArgumentException("No configuration specified");
			if (String.IsNullOrEmpty(config.library)) throw new ArgumentNullException("No library specified");
			if (String.IsNullOrEmpty(config.type)) throw new ArgumentNullException("No Type specified");
			if ((!String.IsNullOrEmpty(config.docfile))&&(!File.Exists(config.docfile))) throw new Exception("Documentation file not found");
			if (!File.Exists(config.library)) throw new Exception("Library file not found");
			_config = config;
			log.Info(_config.ToString());
			//setup keyword map
			_keywordmap = new KeywordMap(this);
			//setup documentator
			if ((!String.IsNullOrEmpty(_config.docfile))&&(File.Exists(_config.docfile)))
			{
				_keyworddoc = new LibraryDoc(this);
			}
			else
			{
				_keyworddoc = null;
			}
			_xmlrpcservice = new XmlRpcService(this);
			_httpservice = new HTTPService(this);
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
