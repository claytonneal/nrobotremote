using System;
using NRobotRemote.Exceptions;

namespace NRobotRemote.Test.Keywords
{

	public class ExceptionClass
	{
		public ExceptionClass()
		{
		}
		
		public void NormalFail()
		{
			throw new Exception("Just a normal error");
		}
		
		public void ContinuableFail()
		{
			throw new ContinuableKeywordException("A continuable failure");
		}
		
		public void FatalFail()
		{
			throw new FatalKeywordException("A fatal failure");
		}
		
	}
}
