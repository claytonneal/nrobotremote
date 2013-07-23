using System;
using NUnit.Framework;
using CookComputing.XmlRpc;
using System.Linq;
using System.Diagnostics;
using NRobotRemote;

namespace NRobotRemote.Test
{
	
	[TestFixture]
	public class GetKeywordNamesFixture
	{
		
		private static RemoteService _service;
		private static IRemoteClient _client;
		
		//constants
		private const String CLibrary = "NRobotRemote.Test.Keywords.dll";
		private const String CPort = "8271";
		private const String CDocFile = "";
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
		public void noreturnvalue_noarguments()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsTrue(result.ToList().Contains("noreturnvalue noarguments".ToUpper()));
		}
		
		[Test]
		public void noreturnvalue_stringarguments()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsTrue(result.ToList().Contains("noreturnvalue stringarguments".ToUpper()));
		}
		
		[Test]
		public void noreturnvalue_stringarrayarguments()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("noreturnvalue stringarrayarguments".ToUpper()));
		}
		
		[Test]
		public void noreturnvalue_stringandarrayarguments()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("noreturnvalue stringandarrayarguments".ToUpper()));
		}
		
		[Test]
		public void stringreturnvalue()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsTrue(result.ToList().Contains("stringreturnvalue".ToUpper()));
		}
		
		[Test]
		public void stringarrayreturnvalue()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsTrue(result.ToList().Contains("stringarrayreturnvalue".ToUpper()));
		}
		
		[Test]
		public void noreturnvalue()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsTrue(result.ToList().Contains("noreturnvalue".ToUpper()));
		}
		
		
		[Test]
		public void objectreturnvalue()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("objectreturnvalue".ToUpper()));
		}
		
		[Test]
		public void intparameter()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("intparameter".ToUpper()));
		}
		
		[Test]
		public void objectparameter()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("objectparameter".ToUpper()));
		}
		
		[Test]
		public void privatemethod()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("privatemethod".ToUpper()));
		}
		
		[Test]
		public void internalmethod()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("internalmethod".ToUpper()));
		}
		
		[Test]
		public void protectedmethod()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("protectedmethod".ToUpper()));
		}
		
		[Test]
		public void publicstaticmethod()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsTrue(result.ToList().Contains("publicstaticmethod".ToUpper()));
		}
		
		[Test]
		public void privatestaticmethod()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("privatestaticmethod".ToUpper()));
		}
		
		[Test]
		public void internalstaticmethod()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("internalstaticmethod".ToUpper()));
		}
		
		[Test]
		public void protectedstaticmethod()
		{
            String[] result = _client.get_keyword_names();
            Assert.IsFalse(result.ToList().Contains("protectedstaticmethod".ToUpper()));
		}
		
		
		[Test]
		public void obsolete_keyword()
		{
			String[] result = _client.get_keyword_names();
			Assert.IsFalse(result.ToList().Contains("obsolete keyword".ToUpper()));
		}
		
	}
}
