using CookComputing.XmlRpc;
using System;

namespace NRobotRemote
{
	/// <summary>
	/// Description of IRobotService.
	/// </summary>
	public interface IRemoteService
	{
		[XmlRpcMethod]
		string[] get_keyword_names();
		[XmlRpcMethod]
		XmlRpcStruct run_keyword(string keyword, object[] args);
		[XmlRpcMethod]
		string[] get_keyword_arguments(string keyword);
		[XmlRpcMethod]
		string get_keyword_documentation(string keyword);
	}
}
