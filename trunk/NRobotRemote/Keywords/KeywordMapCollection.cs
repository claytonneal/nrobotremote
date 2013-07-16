using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace NRobotRemote.Keywords
{
	/// <summary>
	/// Collection of keyword maps
	/// </summary>
	public class KeywordMapCollection : Collection<KeywordMap>
	{
		
		/// <summary>
		/// Gets keyword map for type name
		/// </summary>
		public KeywordMap GetMap(String Typename)
		{
			if (String.IsNullOrEmpty(Typename)) throw new Exception("No Typename specified");
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
		
	
		
	}
}
