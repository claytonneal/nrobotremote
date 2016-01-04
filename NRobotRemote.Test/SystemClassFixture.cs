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
	public class SystemClassFixture
	{
		
		private static RemoteService _service;
		private static IRemoteClient _client;
		
		//constants
		private const String CLibrary = "mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
		private const String CPort = "8271";
		private const String CDocFile = "";
		private const String CType = "System.IO.File";
		private const String CUrl = "System/IO/File";
		
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
		public void cmd_exists()
		{
			XmlRpcStruct result = _client.run_keyword("exists", new object[1]{"C:\\Windows\\System32\\cmd.exe"});
			Assert.IsTrue((Boolean)result["return"]);
		}
		
	}
}
