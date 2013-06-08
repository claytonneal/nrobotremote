using System;
using CookComputing.XmlRpc;

namespace NRobotRemote
{
	/// <summary>
	/// Interface to define a client proxy for robot remote 
	/// </summary>
	public interface IRemoteClient : IRemoteService, IXmlRpcProxy
	{
		
	}
}
