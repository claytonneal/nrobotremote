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
	public class GetKeywordDocumentationFixture
	{
		
		private static RemoteService _service;
		private static IRemoteClient _client;
		
		//constants
		private const String CLibrary = "NRobotRemote.Test.Keywords.dll";
		private const String CPort = "8271";
		private const String CDocFile = "NRobotRemote.Test.Keywords.xml";
		private const String CType = "NRobotRemote.Test.Keywords.PublicClass";
		private const String CUrl = "NRobotRemote/Test/Keywords/PublicClass";
		
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
		public void nodocmethod()
		{
			string doc = _client.get_keyword_documentation("nodocmethod");
			Assert.IsTrue(String.IsNullOrEmpty(doc));
		}
		
		[Test]
		public void withdocmethod()
		{
			string doc = _client.get_keyword_documentation("withdocmethod");
			Assert.IsFalse(String.IsNullOrEmpty(doc));
		}
		
		[Test]
		[ExpectedException(typeof(XmlRpcFaultException))]
		public void unknownkeyword()
		{
			string doc = _client.get_keyword_documentation("some unknown keyword");
		}
		
		[Test]
		public void intro()
		{
			string doc = _client.get_keyword_documentation("__INTRO__");
			Assert.IsTrue(doc.Length > 0);
		}
		
		[Test]
		public void init()
		{
			string doc = _client.get_keyword_documentation("__INIT__");
			Assert.IsTrue(doc.Length == 0);
		}
		
	}
}
