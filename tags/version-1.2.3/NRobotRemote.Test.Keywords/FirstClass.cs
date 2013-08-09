using System;

namespace NRobotRemote.Test.Keywords
{
	public class FirstClass
	{
		public FirstClass()
		{
		}
		
		/// <summary>
		/// Adds two integers
		/// </summary>
		public int Add(String arg1, String arg2)
		{
			return int.Parse(arg1) + int.Parse(arg2);
		}
		
	}
}
