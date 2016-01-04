using System;
using CookComputing.XmlRpc;
using NRobotRemote.Domain;

namespace NRobotRemote
{
	/// <summary>
	/// Xml Rpc keyword execution result builder
	/// </summary>
	public class XmlRpcResultBuilder
	{
		
		/// <summary>
		/// Converts keyword result to RF XmlRpc Structure
		/// </summary>
		public static XmlRpcStruct ToXmlRpcResult(KeywordResult kwresult)
		{
			var result = new XmlRpcStruct();
			//add status
			result.Add("status",kwresult.status.ToString());
			//add error, traceback, output
			result.Add("error",kwresult.error);
			result.Add("traceback",kwresult.traceback);
			result.Add("output",kwresult.output);
			//add return
			if (kwresult.@return.GetType().Equals(typeof(System.Int64)))
			{
				//64bit int has to be returned as string
				result.Add("return",kwresult.@return.ToString());
			}
			else
			{
				result.Add("return",kwresult.@return);
			}
			//check error type
			if (kwresult.status==KeywordStatus.FAIL)
			{
				if (kwresult.errortype==KeywordErrorTypes.Continuable)
				{
					//continuable error
					result.Add("continuable",true);
				}
				if (kwresult.errortype==KeywordErrorTypes.Fatal)
				{
					//fatal error
					result.Add("fatal",true);
				}
			}
			return result;
		}
		
	}
}
