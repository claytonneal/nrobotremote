using System;

namespace NRobotRemote.Keywords
{
	/// <summary>
	/// Description of KeywordNameParser.
	/// </summary>
	public class KeywordNameParser
	{
		public KeywordNameParser() { }
		
		/// <summary>
		/// Takes a method name and turns it into a friendlier keyword
		/// </summary>
		public static String ToFriendlyName(String methodname)
		{
			var result = methodname.Replace("_"," ");
			result = result.ToUpper();
			return result;
		}
		
		/// <summary>
		/// Takes a keyword name and turns it into a potential method name
		/// </summary>
		public static String ToMethodName(String keyword)
		{
			var result = keyword.ToLower();
			result = result.Replace(" ","_");
			return result;
		}
	}
}
