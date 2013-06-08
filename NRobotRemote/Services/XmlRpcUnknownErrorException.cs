using System;
using System.Runtime.Serialization;
using CookComputing.XmlRpc;

namespace NRobotRemote.Services
{
	/// <summary>
	/// Xml RPC exception for internal server error
	/// </summary>
	public class XmlRpcInternalErrorException : XmlRpcFaultException, ISerializable
	{
		
		public XmlRpcInternalErrorException(string message) : base((int)XmlRpcFaultCodes.UnHandledException, message)
		{
		}

		// This constructor is needed for serialization.
		protected XmlRpcInternalErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
	
}