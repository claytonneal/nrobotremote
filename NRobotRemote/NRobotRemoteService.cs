using System;
using log4net;
using System.Security.Principal;
using NRobotRemote.Domain;
using NRobotRemote.Services;

namespace NRobotRemote
{
	/// <summary>
	/// The overall NRobotRemote service
	/// </summary>
	public class NRobotRemoteService
	{
		
		private static readonly ILog Log = LogManager.GetLogger(typeof(NRobotRemoteService));
        private HttpService _httpservice;
	    private KeywordManager _keywordManager;
	    private XmlRpcService _rpcService;
	    private NRobotRemoteServiceConfig _config;

		public NRobotRemoteService(NRobotRemoteServiceConfig config) 
        {
            if (config == null) throw new Exception("No configuration specified");
		    _config = config;
            _keywordManager = new KeywordManager();
            _rpcService = new XmlRpcService(_keywordManager);
            _httpservice = new HttpService(_rpcService, _keywordManager, config.Port);
            LoadKeywords();
        }
		

		
		/// <summary>
		/// Loads the keyword libraries
		/// </summary>
		private void LoadKeywords()
		{
			Log.Debug("Loading keywords");
			try
			{
				foreach(var libraryconfig in _config.AssemblyConfigs)
				{
                    _keywordManager.AddLibrary(libraryconfig.Value);
				}
			}
			catch (Exception e)
			{
				Log.Error(String.Format("Unable to load all configured keywords, {0}",e.Message));
				throw;
			}
			
		}
		
		/// <summary>
		/// Starts HTTP service
		/// </summary>
		public void StartAsync()
		{
			//check permissions
			if (!IsAdministrator())
			{
				Log.Error("Service not started as administrator");
				throw new UnauthorizedAccessException("Service not started as administrator");
			}
			_httpservice.StartAsync();
            Log.Debug("HTTP listener started");
		}
		
		/// <summary>
		/// Stops the service sync
		/// </summary>
		public void Stop()
		{
			_httpservice.Stop();
            Log.Debug("HTTP listener stopped");
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
