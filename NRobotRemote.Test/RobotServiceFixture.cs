using NUnit.Framework;
using NRobotRemote;
using System;

namespace NRobotRemote.Test
{
	[TestFixture]
	public class RobotServiceFixture
	{
		
		//constants
		private const String CLibrary = "NRobotRemote.Test.Keywords.dll";
		private const String CPort = "8271";
		private const String CDocFile = "";
		private const String CType = "NRobotRemote.Test.Keywords.PublicClass";
		
		[Test]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void nolibrary()
		{
			var server = new RemoteService(null,CType,CPort,null);
		}
		
		[Test]
		[ExpectedException(typeof(System.Exception))]
		public void librarynotfound()
		{
			var server = new RemoteService("c:\randomlibrary.dll",CType,CPort,null);
		}
		
		[Test]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void noport()
		{
			var server = new RemoteService(CLibrary,CType,null,null);
		}
		
		[Test]
		[ExpectedException(typeof(System.FormatException))]
		public void nonnumericport()
		{
			var server = new RemoteService(CLibrary,CType,"notanumber",null);
		}
		
		[Test]
		public void nodocfile()
		{
			var server = new RemoteService(CLibrary,CType,CPort,null);
		}
		
		[Test]
		[ExpectedException(typeof(System.Exception))]
		public void docfilenotfound()
		{
			var server = new RemoteService(CLibrary,CType,CPort,"c:\randomdocfile.xml");
		}
		
		[Test]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void notype()
		{
			var server = new RemoteService(CLibrary,null,CPort,null);
		}
		
		[Test]
		[ExpectedException(typeof(System.Exception))]
		public void unknowntype()
		{
			var server = new RemoteService(CLibrary,"Not a valid type",CPort,null);
		}
		
		[Test]
		public void startandstop()
		{
			var server = new RemoteService(CLibrary,CType,CPort,null);
			server.StartAsync();
			server.Stop();
			server.StartAsync();
			server.Stop();
		}
		
		[Test]
		public void multipleservers()
		{
			var server1 = new RemoteService(CLibrary,CType,CPort,null);
			var server2 = new RemoteService(CLibrary,CType,"8272",null);
			server1.StartAsync();
			server2.StartAsync();
			server1.Stop();
			server2.Stop();
		}
		
		[Test]
		[ExpectedException(typeof(NRobotRemote.Keywords.DuplicateKeywordException))]
		public void duplicatekeywords()
		{
			var server = new RemoteService(CLibrary,"NRobotRemote.Test.Keywords.DuplicateMethod",CPort,null);
		}
		
		
	}
}
