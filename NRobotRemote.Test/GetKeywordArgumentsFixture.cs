using System;
using NUnit.Framework;
using NRobotRemote.Services;
using CookComputing.XmlRpc;
using System.Linq;
using System.Diagnostics;
using NRobotRemote;

namespace NRobotRemote.Test
{

	[TestFixture]
	public class GetKeywordArgumentsFixture
	{
		private static RemoteService _service;
		private static IRemoteClient _client;
		
		//constants
		private const String CLibrary = "NRobotRemote.Test.Keywords.dll";
		private const String CPort = "8271";
		private const String CDocFile = "";
		private const String CType = "NRobotRemote.Test.Keywords.PublicClass";
		
		[TestFixtureSetUp]
		public void fixture_setup()
		{
			//setup robot service
			_service = new RemoteService(CLibrary,CType,CPort,CDocFile);
			_service.StartAsync();
			//setup client proxy
			_client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
			_client.Url = "http://127.0.0.1:" + CPort + "/";
		}
		
		[TestFixtureTearDown]
		public void fixture_teardown() 
		{
			//stop service
			_service.Stop();
		}
		
		[Test]
		public void stringarguments()
		{
			String[] result = _client.get_keyword_arguments("noreturnvalue stringarguments".ToUpper());
			Assert.IsTrue(result.ToList().Contains("arg1"));
			Assert.IsTrue(result.ToList().Contains("arg2"));
		}
		
		[Test]
        [ExpectedException(typeof(XmlRpcFaultException))]
		public void stringarrayarguments()
		{
			String[] result = _client.get_keyword_arguments("noreturnvalue stringarrayarguments".ToUpper());
			Assert.IsTrue(result.ToList().Contains("arg1"));
			Assert.IsTrue(result.ToList().Contains("arg2"));
		}
		
		[Test]
        [ExpectedException(typeof(XmlRpcFaultException))]
		public void stringandarrayarguments()
		{
			String[] result = _client.get_keyword_arguments("noreturnvalue stringandarrayarguments".ToUpper());
			Assert.IsTrue(result.ToList().Contains("arg1"));
			Assert.IsTrue(result.ToList().Contains("arg2"));
		}
		
		
	}
}
