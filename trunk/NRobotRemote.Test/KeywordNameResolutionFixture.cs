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
	public class KeywordNameResolutionFixture
	{
		private static RemoteService _service;
		private static IRemoteClient _client;
		
		//constants
		private const String CLibrary = "NRobotRemote.Test.Keywords.dll";
		private const String CPort = "8271";
		private const String CDocFile = "";
		private const String CType = "NRobotRemote.Test.Keywords.PublicClass";
		private const String CUrl = "PublicClass";
		
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
		public void match_uppercase()
		{
			String[] result = _client.get_keyword_arguments("stringreturnvalue".ToUpper());
		}
		
		[Test]
		public void match_lowercase()
		{
			String[] result = _client.get_keyword_arguments("stringreturnvalue".ToLower());
		}
		
		[Test]
		public void match_withspaces()
		{
			String[] result = _client.get_keyword_arguments("noreturnvalue noarguments".ToUpper());
		}
		
		[Test]
		[ExpectedException(typeof(XmlRpcFaultException))]
		public void unknownkeyword()
		{
			String[] result = _client.get_keyword_arguments("an unknown keyword".ToUpper());
		}
		
	}
}
