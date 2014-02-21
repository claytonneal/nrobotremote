using System;

namespace NRobotRemote.Test.Keywords
{

	public class SecondClass
	{
		public SecondClass()
		{
		}
		
		/// <summary>
		/// Subtracts two integers
		/// </summary>
		public int Subtract(String arg1, String arg2)
		{
			return int.Parse(arg1) - int.Parse(arg2);
		}
	}
}
