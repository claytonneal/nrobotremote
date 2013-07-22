using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using NRobotRemote.Domain;
using log4net;

namespace NRobotRemote
{
	/// <summary>
	/// Collection of keyword maps
	/// </summary>
	public class KeywordMapCollection : Collection<KeywordMap>
	{
		
		//log4net
		private static readonly ILog log = LogManager.GetLogger(typeof(KeywordMapCollection));
		
		/// <summary>
		/// Gets keyword map for type name
		/// </summary>
		public KeywordMap GetMap(String Typename)
		{
			if (String.IsNullOrEmpty(Typename)) throw new Exception("No Type name specified");
			var match = this.FirstOrDefault(m => m.KeywordClassType.FullName.Equals(Typename,StringComparison.CurrentCultureIgnoreCase));
			if (match==null) throw new Exception(String.Format("No keyword map found for typename={0}",Typename));
			return match;
		}
		
		/// <summary>
		/// Checks if keyword map exists for type name
		/// </summary>
		/// <param name="Typename"></param>
		/// <returns></returns>
		public bool ContainsMap(String Typename)
		{
			if (String.IsNullOrEmpty(Typename)) return false;
			var match = this.FirstOrDefault(m => m.KeywordClassType.FullName.Equals(Typename,StringComparison.CurrentCultureIgnoreCase));
			return !(match==null);
		}
		
		/// <summary>
		/// Attempts to unload all keyword map domains
		/// </summary>
		public void UnLoadMaps()
		{
			log.Debug("Unloading all keyword appDomains");
			int mapcount = this.Count;
			for(int counter = (mapcount-1); counter>=0; counter--)
			{
				var kwmap = this[counter];
				try
				{
					AppDomain kwdomain = kwmap.GetDomain();
					AppDomain.Unload(kwdomain);
					this.RemoveAt(counter);
				}
				catch
				{
					log.Error(String.Format("Unable to unload appdomain for keyword map {0}",kwmap.KeywordClassType.FullName));
				}
			}
		}
		
	}
}
