using System;

namespace NRobotRemote.Test.Keywords
{
	/// <summary>
	/// Class with private default constructor
	/// </summary>
	public class NonPublicConstructor
	{
		private NonPublicConstructor()
		{
		}
		
		public NonPublicConstructor(string arg)
		{
		}
		
		public static void publicstatic() { }
		public void publicinstance() { }
		
	}
}
