using System;
using System.Runtime.Serialization;
using CookComputing.XmlRpc;

namespace NRobotRemote.Services
{
	/// <summary>
	/// XmlRpc exception for unknown keyword
	/// </summary>
	public class XmlRpcUnknownKeywordException : XmlRpcFaultException, ISerializable
	{
		
		public XmlRpcUnknownKeywordException(string message) : base((int)XmlRpcFaultCodes.UnRecognisedKeyword,message)
		{
		}
		
		protected XmlRpcUnknownKeywordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}