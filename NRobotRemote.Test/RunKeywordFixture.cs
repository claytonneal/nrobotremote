using System;
using NUnit.Framework;
using NRobotRemote;
using CookComputing.XmlRpc;
using System.Linq;
using System.Diagnostics;

namespace NRobotRemote.Test
{
	[TestFixture]
	public class RunKeywordFixture
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
		[ExpectedException(typeof(XmlRpcNullParameterException))]
		public void nullargs()
		{
			_client.run_keyword("exec noerror",null);
		}
		
		[Test]
		public void returnstructure()
		{
			XmlRpcStruct result = _client.run_keyword("exec noerror",new object[0]);
			Assert.IsTrue(result.ContainsKey("return"));
			Assert.IsTrue(result.ContainsKey("status"));
			Assert.IsTrue(result.ContainsKey("error"));
			Assert.IsTrue(result.ContainsKey("traceback"));
			Assert.IsTrue(result.ContainsKey("output"));
		}
		
		[Test]
		public void exec_noerror()
		{
			XmlRpcStruct result = _client.run_keyword("exec noerror",new object[0]);
			Assert.IsTrue(result["status"].ToString()=="PASS");
			Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
			Assert.IsTrue(String.IsNullOrEmpty(result["error"].ToString()));
			Assert.IsTrue(String.IsNullOrEmpty(result["traceback"].ToString()));
			Assert.IsTrue(String.IsNullOrEmpty(result["output"].ToString()));
		}
		
		[Test]
		public void exec_exception()
		{
			XmlRpcStruct result = _client.run_keyword("exec exception",new object[0]);
			Assert.IsTrue(result["status"].ToString()=="FAIL");
			Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
			Assert.IsFalse(String.IsNullOrEmpty(result["error"].ToString()));
			Assert.IsFalse(String.IsNullOrEmpty(result["traceback"].ToString()));
			Assert.IsTrue(String.IsNullOrEmpty(result["output"].ToString()));
		}
		
		[Test]
		public void exec_stringparam()
		{
			XmlRpcStruct result = _client.run_keyword("exec stringparam",new string[] {"arg1","arg2"} );
			Assert.IsTrue(result["status"].ToString()=="PASS");
			Assert.IsTrue(result["return"].ToString()=="arg1arg2");
			Assert.IsTrue(String.IsNullOrEmpty(result["error"].ToString()));
			Assert.IsTrue(String.IsNullOrEmpty(result["traceback"].ToString()));
			Assert.IsTrue(String.IsNullOrEmpty(result["output"].ToString()));
		}
		
		[Test]
		[ExpectedException(typeof(XmlRpcFaultException))]
		public void exec_stringparam_noparam()
		{
			XmlRpcStruct result = _client.run_keyword("exec stringparam",new string[] {""} );
			Assert.IsTrue(result["status"].ToString()=="FAIL");
			Assert.IsTrue(result["return"].ToString()=="");
			Assert.IsTrue(String.IsNullOrEmpty(result["error"].ToString()));
			Assert.IsTrue(String.IsNullOrEmpty(result["traceback"].ToString()));
			Assert.IsTrue(String.IsNullOrEmpty(result["output"].ToString()));
		}
		
        [Test]
        [ExpectedException(typeof(XmlRpcFaultException))]
		public void exec_stringparam_extraparam()
		{
			XmlRpcStruct result = _client.run_keyword("exec stringparam",new string[] {"1","2","3"} );
			Assert.IsTrue(result["status"].ToString()=="FAIL");
			Assert.IsTrue(result["return"].ToString()=="");
			Assert.IsTrue(String.IsNullOrEmpty(result["error"].ToString()));
			Assert.IsTrue(String.IsNullOrEmpty(result["traceback"].ToString()));
			Assert.IsTrue(String.IsNullOrEmpty(result["output"].ToString()));
		}
		
		

        [Test]
        public void exec_withtrace_single()
        {
            XmlRpcStruct result = _client.run_keyword("exec withtrace", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(result["return"].ToString() == "");
            Assert.IsTrue(String.IsNullOrEmpty(result["error"].ToString()));
            Assert.IsTrue(String.IsNullOrEmpty(result["traceback"].ToString()));
            Assert.IsTrue(result["output"].ToString() == "Hello");
        }

        [Test]
        public void exec_withtrace_double()
        {
            XmlRpcStruct result1 = _client.run_keyword("exec withtrace", new object[0]);
            XmlRpcStruct result2 = _client.run_keyword("exec withtrace", new object[0]);
            Assert.IsTrue(result1["output"].ToString() == "Hello");
            Assert.IsTrue(result2["output"].ToString() == "Hello");
        }

        [Test]
        public void exec_returnsnull()
        {
            XmlRpcStruct result = _client.run_keyword("exec returnsnull", new object[0]);
            Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
        }
		
		[Test]
        public void exec_returnfalse()
        {
            XmlRpcStruct result = _client.run_keyword("exec returnfalse", new object[0]);
            Assert.IsTrue(result["return"].GetType().Equals(typeof(System.Boolean)));
            Assert.IsFalse((Boolean)result["return"]);
        }
        
        [Test]
        public void exec_returntrue()
        {
            XmlRpcStruct result = _client.run_keyword("exec returntrue", new object[0]);
            Assert.IsTrue(result["return"].GetType().Equals(typeof(System.Boolean)));
            Assert.IsTrue((Boolean)result["return"]);
        }
		
	}
}
