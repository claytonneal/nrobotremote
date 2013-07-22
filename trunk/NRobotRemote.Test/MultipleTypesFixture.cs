using System;
using NUnit.Framework;
using NRobotRemote;
using CookComputing.XmlRpc;
using System.Linq;
using System.Diagnostics;
using NRobotRemote.Config;

namespace NRobotRemote.Test
{
	[TestFixture]
	public class MultipleTypesFixture
	{
		
		private static RemoteService _service;
		private static IRemoteClient _firstclient;
		private static IRemoteClient _secondclient;
		
		//constants
		private const String CLibrary = "NRobotRemote.Test.Keywords.dll";
		private const String CFirstType = "NRobotRemote.Test.Keywords.FirstClass";
		private const String CSecondType = "NRobotRemote.Test.Keywords.SecondClass";
		
		[TestFixtureSetUp]
		public void fixture_setup()
		{
			//setup robot service
			RemoteServiceConfig config = new RemoteServiceConfig();
			config.port = 8271;
			config.AddKeywordConfig(new KeywordMapConfig() {Library = CLibrary, Type = CFirstType});
			config.AddKeywordConfig(new KeywordMapConfig() {Library = CLibrary, Type = CSecondType});
			_service = new RemoteService(config);
			_service.StartAsync();
			//setup client proxies
			_firstclient = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
			_secondclient = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
			_firstclient.Url = "http://127.0.0.1:8271/NRobotRemote/Test/Keywords/FirstClass";
			_secondclient.Url = "http://127.0.0.1:8271/NRobotRemote/Test/Keywords/SecondClass";
			
		}
		
		[TestFixtureTearDown]
		public void fixture_teardown() 
		{
			//stop service
			_service.Stop();
		}
		
		[Test]
		public void first_positive()
		{
			XmlRpcStruct result = _firstclient.run_keyword("add", new object[]{"1","2"});
            Assert.IsTrue(result["return"].GetType().Equals(typeof(System.Int32)));
            Assert.IsTrue((System.Int32)result["return"]==3);
		}
		
		[Test]
		[ExpectedException(typeof(XmlRpcFaultException))]
		public void first_unknownkeyword()
		{
			XmlRpcStruct result = _firstclient.run_keyword("subtract", new object[]{"1","2"});
		}
		
		[Test]
		public void second_positive()
		{
			XmlRpcStruct result = _secondclient.run_keyword("subtract", new object[]{"3","1"});
            Assert.IsTrue(result["return"].GetType().Equals(typeof(System.Int32)));
            Assert.IsTrue((System.Int32)result["return"]==2);
		}
		
		[Test]
		[ExpectedException(typeof(XmlRpcFaultException))]
		public void second_unknownkeyword()
		{
			XmlRpcStruct result = _secondclient.run_keyword("add", new object[]{"1","2"});
		}
		
		[Test]
		[ExpectedException(typeof(XmlRpcServerException))]
		public void invalidurl_unknowntype()
		{
			var _wrongclient = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
			_wrongclient.Url = "http://127.0.0.1:8271/UnknownClass";
			XmlRpcStruct result = _wrongclient.run_keyword("add", new object[]{"1","2"});
		}
		
		[Test]
		[ExpectedException(typeof(XmlRpcServerException))]
		public void invalidurl_notype()
		{
			var _wrongclient = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
			_wrongclient.Url = "http://127.0.0.1:8271";
			XmlRpcStruct result = _wrongclient.run_keyword("add", new object[]{"1","2"});
		}
		
		
		
		
	}
}
