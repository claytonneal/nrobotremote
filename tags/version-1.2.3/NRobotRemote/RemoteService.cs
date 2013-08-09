using System;
using System.IO;
using log4net;
using System.Security.Principal;
using System.Collections.Generic;
using NRobotRemote.Domain;
using NRobotRemote.Config;

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
        internal KeywordMapCollection _keywordmaps;
        internal RemoteServiceConfig _config;
        
        /// <summary>
        /// Event called when keyword stop_remote_server is executed
        /// Event is raised on background thread
        /// </summary>
        public event EventHandler StopRequested;
        
        
        /// <summary>
        /// Constructor for single library
        /// </summary>
        public RemoteService(String Library, String Type, String Port, String Docfile = null) : this(new RemoteServiceConfig(Library,Type,Port,Docfile))
        {
        }
        
		/// <summary>
		/// Creates a new instance of the robot service
		/// </summary>
		public RemoteService(RemoteServiceConfig config)
		{
			//check config
			if (config==null) throw new ArgumentException("No configuration specified");
			config.VerifyConfig();
			_config = config;
			//setup services
			_xmlrpcservice = new XmlRpcService(this);
			_httpservice = new HTTPService(this);
			//setup collection
			_keywordmaps = new KeywordMapCollection();
		}
		
		/// <summary>
		/// Loads the keyword libraries into app domains
		/// </summary>
		private void LoadKeywords()
		{
			//build keyword maps
			log.Debug("Building keyword maps");
			var configs = _config.GetConfigs();
			try
			{
				foreach(KeywordMapConfig mapconfig in configs)
				{
					_keywordmaps.Add(KeywordDomainBuilder.CreateDomain(mapconfig));
				}
			}
			catch (Exception e)
			{
				log.Error(String.Format("Unable to build keyword maps, {0}",e.Message));
				UnLoadKeywords();
				throw e;
			}
			
		}
		
		/// <summary>
		/// Unloads all keyword app domains
		/// </summary>
		private void UnLoadKeywords()
		{
			_keywordmaps.UnLoadMaps();
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
			LoadKeywords();
			_httpservice.StartAsync();
		}
		
		/// <summary>
		/// Stops the service sync
		/// </summary>
		public void Stop()
		{
			_httpservice.Stop();
			UnLoadKeywords();
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

		/// <summary>
		/// Raises StopRequested event
		/// </summary>
		internal void OnStopRequested(EventArgs e)
		{
			EventHandler handler = StopRequested;
			if (handler!= null)
			{
				handler(this, e);
			}
		}
		
	}
}
