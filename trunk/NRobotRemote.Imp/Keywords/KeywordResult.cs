using System;

namespace NRobotRemote.Keywords
{
	/// <summary>
	/// Description of KeywordResult.
	/// </summary>
	public class KeywordResult
	{
		public KeywordResult() 
		{ 
			status = "FAIL";
			output = "";
			@return = "";
			error = "";
			traceback = "";
		}
		
		public String status {get; set; }
		public String output {get; set; }
		public String @return {get; set; }
		public String error {get; set;}
		public String traceback {get; set;}
		public double duration {get; set; }
		
		public override string ToString()
		{
			return string.Format("[KeywordResult Status={0}, Output={1}, Return={2}, Error={3}, Traceback={4}, Duration={5}]", status, output, @return, error, traceback, duration);
		}

	}
}
