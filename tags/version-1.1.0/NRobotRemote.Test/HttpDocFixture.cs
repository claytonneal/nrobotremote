using System;
using NUnit.Framework;
using NRobotRemote;
using CookComputing.XmlRpc;
using System.Linq;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Text;

namespace NRobotRemote.Test
{

	[TestFixture]
	public class HttpDocFixture
	{

		
		private static RemoteService _service;
		
		//constants
		private const String CLibrary = "NRobotRemote.Test.Keywords.dll";
		private const String CFirstType = "NRobotRemote.Test.Keywords.FirstClass";
		private const String CSecondType = "NRobotRemote.Test.Keywords.SecondClass";
		private const String CDocFile = "NRobotRemote.Test.Keywords.xml";
		
		[TestFixtureSetUp]
		public void fixture_setup()
		{
			//setup robot service
			RemoteServiceConfig config = new RemoteServiceConfig();
			config.port = 8271;
			config.AddKeywordConfig(new KeywordMapConfig() {Library = CLibrary, Type = CFirstType, DocFile = CDocFile});
			config.AddKeywordConfig(new KeywordMapConfig() {Library = CLibrary, Type = CSecondType});
			_service = new RemoteService(config);
			_service.StartAsync();
			
		}
		
		[TestFixtureTearDown]
		public void fixture_teardown() 
		{
			//stop service
			_service.Stop();
		}
		
		[Test]
		public void get_doc_baseurl()
		{
			//do request
			Uri addr = new Uri("http://127.0.0.1:8271");
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(addr);
			req.Method = "GET";
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
			//do response
			Assert.IsTrue(resp.StatusCode == HttpStatusCode.OK);
			var encoding = ASCIIEncoding.ASCII;
        	var reader = new System.IO.StreamReader(resp.GetResponseStream(), encoding);
            string responseText = reader.ReadToEnd();
            Trace.WriteLine(responseText);
            //assert content
        	Assert.IsTrue(responseText.Length > 0);
        	Assert.IsTrue(responseText.Contains("NRobotRemote"));
        	Assert.IsTrue(responseText.Contains("NRobotRemote.Test.Keywords.FirstClass"));
        	Assert.IsTrue(responseText.Contains("NRobotRemote.Test.Keywords.SecondClass"));
        	Assert.IsTrue(responseText.Contains("ADD"));
        	Assert.IsTrue(responseText.Contains("SUBTRACT"));
        	Assert.IsTrue(responseText.Contains("Adds two integers"));
        	Assert.IsFalse(responseText.Contains("Subtracts two integers"));

		}
		

	}
}
