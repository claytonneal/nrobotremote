using System;
using NUnit.Framework;
using NRobotRemote;
using CookComputing.XmlRpc;
using System.Linq;
using System.Diagnostics;

namespace NRobotRemote.Test
{

	[TestFixture]
	public class NonPublicConstructorFixture
	{
		
		private static RemoteService _service;
		private static IRemoteClient _client;
		
		//constants
		private const String CLibrary = "NRobotRemote.Test.Keywords.dll";
		private const String CPort = "8271";
		private const String CDocFile = "";
		private const String CType = "NRobotRemote.Test.Keywords.NonPublicConstructor";
		private const String CUrl = "NRobotRemote/Test/Keywords/NonPublicConstructor";
		
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
		public void publicstatic()
		{
			String[] result = _client.get_keyword_names();
            Assert.IsTrue(result.ToList().Contains("publicstatic".ToUpper()));
		}
		
		[Test]
		public void publicinstance()
		{
			String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("publicinstance".ToUpper()));
		}
		
		
	}
}
