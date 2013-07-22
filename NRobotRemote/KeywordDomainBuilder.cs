using System;
using System.Reflection;
using log4net;
using NRobotRemote.Domain;
using NRobotRemote.Config;

namespace NRobotRemote
{

	public class KeywordDomainBuilder
	{
		
		//log4net
		private static readonly ILog log = LogManager.GetLogger(typeof(KeywordDomainBuilder));
		
		/// <summary>
		/// Creates a new new keyword map on a new app domain
		/// </summary>
		public static KeywordMap CreateDomain(KeywordMapConfig config)
		{
			try
			{
				log.Debug(String.Format("Creating appDomain for type {0}",config.Type));
				//setup domain
				var kwdomainsetup = new AppDomainSetup();
				kwdomainsetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
				var kwdomain = AppDomain.CreateDomain(config.Type,null,kwdomainsetup);
	            //get remote builder instance      
				var remotebuilder = (KeywordMapBuilder) kwdomain.CreateInstanceAndUnwrap("NRobotRemote.Domain", typeof(KeywordMapBuilder).FullName);
				//call remote builder
				var map = remotebuilder.CreateMap(config);
				//log keyword names
				log.Debug(String.Format("Keyword names are, {0}",String.Join(",", map.GetKeywordNames().ToArray())));
				return map;
			}
			catch (Exception e)
			{
				log.Error(String.Format("Exception creating keyword domain, {0}",e.Message));
				throw new KeywordDomainException(e.Message);
			}

		}
		
		
	}
}
