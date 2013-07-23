using System;
using System.Configuration;
using System.IO;
using System.Configuration;

namespace NRobotRemote.Config
{
	
	/// <summary>
	/// Configuration for Keyword Map
	/// </summary>
	[Serializable]
	public class KeywordMapConfig
	{
	
		private const char CSeparator = ':';
		
		public KeywordMapConfig()
		{
			DocFile = null;
		}
		
		/// <summary>
		/// Constructor from parameter string in format library:type:docfile
		/// </summary>
		public KeywordMapConfig(String config)
		{
			if (String.IsNullOrEmpty(config)) throw new ConfigurationErrorsException("No map config parameters specified");
			string[] parts = config.Split(CSeparator);
			if (parts.Length < 2) throw new ConfigurationErrorsException("Map config doesnt contain enough items");
			Library = parts[0];
			Type = parts[1];
			if (parts.Length>=3) DocFile = parts[2];
			Verify();
		}
		
		/// <summary>
		/// Verifies the keyword map config
		/// </summary>
		public void Verify()
		{
			if (String.IsNullOrEmpty(Library)) throw new ConfigurationErrorsException("No Library specified");
			if (String.IsNullOrEmpty(Type)) throw new ConfigurationErrorsException("No Type specified");
			if (!String.IsNullOrEmpty(DocFile))
			{
				if (!File.Exists(DocFile)) throw new ConfigurationErrorsException("Specified documentation file not found");
			}
		}
		
		//public fields
		public String Library;
		public String Type;
		public String DocFile;
		
	}
	
}
