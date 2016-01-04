using System;
using NUnit.Framework;
using NRobotRemote;
using CookComputing.XmlRpc;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace NRobotRemote.Test
{
	[TestFixture]
	public class ExcetptionTypesFixture
	{
		
		private static RemoteService _service;
		private static IRemoteClient _client;
		
		//constants
		private const String CLibrary = "NRobotRemote.Test.Keywords.dll";
		private const String CPort = "8271";
		private const String CDocFile = "";
		private const String CType = "NRobotRemote.Test.Keywords.ExceptionClass";
		private const String CUrl = "NRobotRemote/Test/Keywords/ExceptionClass";
		
		[TestFixtureSetUp]
		public void fixture_setup()
		{
			//setup robot service
			_service = new RemoteService(CLibrary,CType,CPort,CDocFile);
			_service.StartAsync();
			//setup client proxy
			_client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
			_client.Url = "http://127.0.0.1:" + CPort + "/" + CUrl;
		}
		
		[TestFixtureTearDown]
		public void fixture_teardown() 
		{
			//stop service
			_service.Stop();
		}
		
		[Test]
		public void NormalFailure()
		{
			XmlRpcStruct result = _client.run_keyword("NormalFail",new object[0]);
			//check continuable or fatal are not set
			Assert.IsFalse(result.ContainsKey("fatal"));
			Assert.IsFalse(result.ContainsKey("continuable"));
		}
		
		[Test]
		public void ContinuableFailure()
		{
			XmlRpcStruct result = _client.run_keyword("ContinuableFail",new object[0]);
			//check no fatal element
			Assert.IsFalse(result.ContainsKey("fatal"));
			//check there is a continuable element
			Assert.IsTrue(result.ContainsKey("continuable"));
			//check continuable is true
			Assert.IsTrue(Convert.ToBoolean(result["continuable"]));
		}
		
		[Test]
		public void FatalFailure()
		{
			XmlRpcStruct result = _client.run_keyword("FatalFail",new object[0]);
			//check no continuable element
			Assert.IsFalse(result.ContainsKey("continuable"));
			//check there is a fatal element
			Assert.IsTrue(result.ContainsKey("fatal"));
			//check fatal is true
			Assert.IsTrue(Convert.ToBoolean(result["fatal"]));
		}
		
	}
}
