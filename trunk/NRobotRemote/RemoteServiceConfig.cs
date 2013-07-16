using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace NRobotRemote
{
	/// <summary>
	/// Configuration for Remote Service
	/// </summary>
	public class RemoteServiceConfig
	{
		
		//log4net
		private static readonly ILog log = LogManager.GetLogger(typeof(RemoteService));
		
		//cmd line tokens
		private const string CTokenKeyword = "-k";
		private const string CTokenPort = "-p";
		
		/// <summary>
		/// Parameterless constructor
		/// </summary>
		public RemoteServiceConfig()
		{
			_mapconfigs = new Dictionary<String,KeywordMapConfig>();
			port = 0;
		}
		
		/// <summary>
		/// Creates configuration for single library and type
		/// </summary>
		public RemoteServiceConfig(String library, String type, String portnum, String docfile = null)
		{
			_mapconfigs = new Dictionary<String,KeywordMapConfig>();
			this.port = int.Parse(portnum);
			_mapconfigs.Add(type, new KeywordMapConfig() {Library = library, Type = type, DocFile = docfile});
		}
		
		/// <summary>
		/// Creates configuration from cmd line options
		/// </summary>
		public RemoteServiceConfig(string[] args)
		{
			if (args.Length < 2) throw new ArgumentException("No enough config parameters");
			log.Debug(String.Format("Creating configuration from string : {0}",String.Join(" ",args)));
			_mapconfigs = new Dictionary<String,KeywordMapConfig>();
			try
			{
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
				if ((portindex==-1)||(portindex==argcount)) throw new ArgumentException("No port specified");
				if ((keyindex==-1)||(keyindex==argcount)) throw new ArgumentException("No keyword libraries specified");
				//process port
				this.port = int.Parse(args[portindex+1]);
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
			catch (Exception e)
			{
				log.Error(e.ToString());
				throw;
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
			if (config==null) throw new ArgumentNullException("Config specified is null");
			if (String.IsNullOrEmpty(config.Type)) throw new ArgumentNullException("Config has not Type defined");
			_mapconfigs.Add(config.Type,config);
		}
		
		
		
	}
	
	/// <summary>
	/// Configuration for Keyword Map
	/// </summary>
	public class KeywordMapConfig
	{
	
		private const char CSeparator = ':';
		
		public KeywordMapConfig()
		{
			DocFile = null;
		}
		
		public KeywordMapConfig(String config)
		{
			if (String.IsNullOrEmpty(config)) throw new Exception("No config specified");
			string[] parts = config.Split(CSeparator);
			if (parts.Length < 2) throw new Exception("Config doesnt contain enough items");
			Library = parts[0];
			Type = parts[1];
			if (parts.Length>=3) DocFile = parts[2];
		}
		
		
		public String Library;
		public String Type;
		public String DocFile;
		
	}

	
}
