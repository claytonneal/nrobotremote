using System;
using NUnit.Framework;
using NRobotRemote;
using CookComputing.XmlRpc;
using System.Linq;
using System.Diagnostics;

namespace NRobotRemote.Test
{

	[TestFixture]
	public class StopRemoteServerFixture
	{
		
		private static RemoteService _service;
		private static IRemoteClient _client;
		private static volatile bool _stoprequested;
		
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
			//add event handler
			_service.StopRequested += OnStopHandler;
		}
		
		[TestFixtureTearDown]
		public void fixture_teardown() 
		{
			//stop service
			_service.Stop();
		}
		
		[Test]
		public void stop_remote_server_method()
		{
			_stoprequested = false;
			_client.stop_remote_server();
			//wait upto 5 secs for it to be processed
			System.Threading.Thread.Sleep(5000);
			Assert.IsTrue(_stoprequested);
			
		}
		
		[Test]
		public void stop_remote_server_keyword()
		{
			_stoprequested = false;
			_client.run_keyword("STOP REMOTE SERVER",new object[0]);
			//wait upto 5 secs for it to be processed
			System.Threading.Thread.Sleep(6000);
			Assert.IsTrue(_stoprequested);
			
		}
		
		
		/// <summary>
		/// Event handler called on worker thread
		/// </summary>
		public void OnStopHandler(object sender, EventArgs e)
		{
			Trace.WriteLine("Stop remote server event handler called");
			_stoprequested = true;
		}
		
		
	}
}
