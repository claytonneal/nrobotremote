using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NRobotRemote.Config
{
	/// <summary>
	/// Configuration for Remote Service
	/// </summary>
	public class RemoteServiceConfig
	{
		

		//cmd line tokens
		private const string CTokenKeyword = "-k";
		private const string CTokenPort = "-p";
		private const int CNoPortSpecified = -1;
		
		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public RemoteServiceConfig()
		{
			_mapconfigs = new Dictionary<String,KeywordMapConfig>();
			port = CNoPortSpecified;
		}
		
		/// <summary>
		/// Creates configuration for single library and type
		/// </summary>
		public RemoteServiceConfig(String library, String type, String portnum, String docfile = null)
		{
			_mapconfigs = new Dictionary<String,KeywordMapConfig>();
			//check port
			if (String.IsNullOrEmpty(portnum)) throw new ConfigurationErrorsException("No port specified");
			int temport;
			if (!int.TryParse(portnum,out temport)) throw new ConfigurationErrorsException("Invalid port specified");
			this.port = temport;
			//check type
			if (String.IsNullOrEmpty(type)) throw new ConfigurationErrorsException("No type specified");
			_mapconfigs.Add(type, new KeywordMapConfig() {Library = library, Type = type, DocFile = docfile});
		}
		
		/// <summary>
		/// Creates configuration from cmd line options
		/// </summary>
		public RemoteServiceConfig(string[] args)
		{
			if (args.Length < 2) throw new ConfigurationErrorsException("No enough config parameters");
			_mapconfigs = new Dictionary<String,KeywordMapConfig>();
			port = CNoPortSpecified;
			int argcount = args.Length;
			//get indexes
			int portindex = -1;
			int keyindex = -1;
			for (int counter=0; counter < argcount; counter++)
			{
				if (args[counter].Equals(CTokenPort,StringComparison.CurrentCultureIgnoreCase))
				{
					portindex = counter;
				}
				if (args[counter].Equals(CTokenKeyword,StringComparison.CurrentCultureIgnoreCase))
				{
					keyindex = counter;
				}
			}
			if ((portindex==-1)||(portindex==argcount)) throw new ConfigurationErrorsException("No port specified");
			if ((keyindex==-1)||(keyindex==argcount)) throw new ConfigurationErrorsException("No keyword libraries specified");
			//process port
			int temport;
			if (!int.TryParse(args[portindex+1],out temport)) throw new ConfigurationErrorsException("Invalid port specified");
			this.port = temport;
			//process key libraries
			int startindex;
			int endindex;
			if (portindex > keyindex)
			{
				endindex = portindex-1;
				startindex = keyindex + 1;
			}
			else
			{
				endindex = argcount-1;
				startindex = keyindex + 1;
			}
			for (int counter=startindex; counter<=endindex; counter++)
			{
				var config = new KeywordMapConfig(args[counter]);
				this._mapconfigs.Add(config.Type,config);
			}
		}
		
		
		/// <summary>
		/// Port number for the service
		/// </summary>
		public int port {get; set;}
		
		/// <summary>
		/// Dictionary of keyword configs
		/// </summary>
		private Dictionary<String,KeywordMapConfig> _mapconfigs;
		
		/// <summary>
		/// list of keyword map configs
		/// </summary>
		/// <returns></returns>
		public List<KeywordMapConfig> GetConfigs()
		{
			return _mapconfigs.Select(d => d.Value).ToList();
		}
		
		/// <summary>
		/// Add a new keyword assembly/type config
		/// </summary>
		public void AddKeywordConfig(KeywordMapConfig config)
		{
			if (config==null) throw new ConfigurationErrorsException("Config specified is null");
			if (String.IsNullOrEmpty(config.Type)) throw new ConfigurationErrorsException("Config has not Type defined");
			_mapconfigs.Add(config.Type,config);
		}
		
		/// <summary>
		/// Verifies configuration is correct
		/// </summary>
		public void VerifyConfig()
		{
			if (GetConfigs().Count==0) throw new ConfigurationErrorsException("No keyword map configurations");
			if (port==CNoPortSpecified) throw new ConfigurationErrorsException("No port specified");
			foreach(KeywordMapConfig mapconfig in GetConfigs())
			{
				mapconfig.Verify();
			}
		}
		
	}
	
	
}
