using System;
using System.Linq;
using NRobotRemote.Exceptions;

namespace NRobotRemote.Domain
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
	/// Keyword error types
	/// </summary>
	public enum KeywordErrorTypes
	{
		NoError,
		Normal,
		Continuable,
		Fatal
	}
	
	/// <summary>
	/// Result of keyword execution
	/// </summary>
	[Serializable]
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
		public KeywordErrorTypes errortype {get; set;}
		
		/// <summary>
		/// Output result to string
		/// </summary>
		public override string ToString()
		{
			//build return value string
			String strReturn = @return.ToString();
			if (@return.GetType().IsArray && @return.GetType().GetElementType().Equals(typeof(System.String)))
			{
				var retarr = (String[])@return;
				strReturn = "[" + String.Join(",",retarr) + "]";
			}
			return string.Format("[KeywordResult Status={0}, Output={1}, Return={2}, Error={3}, Traceback={4}, Duration={5}, ErrorType={6}]", status, output, strReturn, error, traceback, duration,errortype);
		}
		
		/// <summary>
		/// Captures result properties from an exception
		/// </summary>
		public void CaptureException(Exception e)
		{
			this.traceback = e.StackTrace;
	﻿  ﻿  ﻿  ﻿  this.error = e.Message;
			this.status = KeywordStatus.FAIL;
			this.@return = "";
			
			//check exception type
			errortype = KeywordErrorTypes.Normal;
			if (e is ContinuableKeywordException)
			{
				errortype = KeywordErrorTypes.Continuable;
			}
			if (e is FatalKeywordException)
			{
				errortype = KeywordErrorTypes.Fatal;
			}
			
			
		}
		

	}
}
