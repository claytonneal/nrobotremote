using System;
using CookComputing.XmlRpc;

namespace NRobotRemote.Keywords
{
	
	/// <summary>
	/// Keyword execution status
	/// </summary>
	public enum KeywordStatus
	{
		PASS,
		FAIL
	}
	
	/// <summary>
	/// Result of keyword execution
	/// </summary>
	public class KeywordResult
	{
		public KeywordResult() 
		{ 
			status = KeywordStatus.FAIL;
			output = "";
			@return = "";
			error = "";
			traceback = "";
		}
		
		public KeywordStatus status {get; set; }
		public String output {get; set; }
		public Object @return {get; set; }
		public String error {get; set;}
		public String traceback {get; set;}
		public double duration {get; set; }
		
		/// <summary>
		/// Output result to string
		/// </summary>
		public override string ToString()
		{
			return string.Format("[KeywordResult Status={0}, Output={1}, Return={2}, Error={3}, Traceback={4}, Duration={5}]", status, output, @return, error, traceback, duration);
		}
		
		/// <summary>
		/// Converts keyword result to RF XmlRpc Structure
		/// </summary>
		public XmlRpcStruct ToRobotXmlRpcStruct()
		{
			var result = new XmlRpcStruct();
			//add status
			result.Add("status",this.status.ToString());
			//add error, traceback, output
			result.Add("error",this.error);
			result.Add("traceback",this.traceback);
			result.Add("output",this.output);
			//add return
			if (this.@return.GetType().Equals(typeof(System.Int64)))
			{
				//64bit int has to be returned as string
				result.Add("return",this.@return.ToString());
			}
			else
			{
				result.Add("return",this.@return);
			}
			return result;
		}

	}
}
