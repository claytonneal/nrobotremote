using NUnit.Framework;
using NRobotRemote;
using System;
using System.IO;

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
		private const String CUrl = "PublicClass";
		
		[Test]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void nolibrary()
		{
			var server = new RemoteService(null,CType,CPort,null);
		}
		
		[Test]
		[ExpectedException(typeof(System.IO.FileNotFoundException))]
		public void librarynotfound()
		{
			var server = new RemoteService("c:\\randomlibrary.dll",CType,CPort,null);
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
		
		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void config_noport()
		{
			var config = new RemoteServiceConfig();
			config.AddKeywordConfig(new KeywordMapConfig() {Library = CLibrary, Type = CType} );
			var server = new RemoteService(config);	
		}
		
		[Test]
		[ExpectedException(typeof(System.Exception))]
		public void config_notype()
		{
			var config = new RemoteServiceConfig();
			config.AddKeywordConfig(new KeywordMapConfig() {Library = CLibrary, Type = CType} );
			config.AddKeywordConfig(new KeywordMapConfig() {Library = CLibrary, Type = "Unknown.Type"} );
			config.port = int.Parse(CPort);
			var server = new RemoteService(config);
		}
		
		[Test]
		[ExpectedException(typeof(System.IO.FileNotFoundException))]
		public void config_nolibrary()
		{
			var config = new RemoteServiceConfig();
			config.AddKeywordConfig(new KeywordMapConfig() {Library = CLibrary, Type = CType} );
			config.AddKeywordConfig(new KeywordMapConfig() {Library = "c:\\unknown.dll", Type = "sometype"} );
			config.port = int.Parse(CPort);
			var server = new RemoteService(config);
		}
		
		[Test]
		[ExpectedException(typeof(System.Exception))]
		public void config_unknowndoc()
		{
			var config = new RemoteServiceConfig();
			config.AddKeywordConfig(new KeywordMapConfig() {Library = CLibrary, Type = CType, DocFile="unkown.xml"} );
			config.port = int.Parse(CPort);
			var server = new RemoteService(config);
		}
		
		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void config_sametype()
		{
			var config = new RemoteServiceConfig();
			config.AddKeywordConfig(new KeywordMapConfig() {Library = CLibrary, Type = CType} );
			config.AddKeywordConfig(new KeywordMapConfig() {Library = CLibrary, Type = CType} );
			config.port = int.Parse(CPort);
			var server = new RemoteService(config);
		}
		
		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void cmdline_oneitem()
		{
			var cmdline = new String[] {"NRobotRemote.Test.Keywords.dll:NRobotRemote.Test.Keywords.PublicClass"};
			var config = new RemoteServiceConfig(cmdline);
			var server = new RemoteService(config);	
		}
		
		
		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void cmdline_noport()
		{
			var cmdline = new String[] {"-k","NRobotRemote.Test.Keywords.dll:NRobotRemote.Test.Keywords.PublicClass"};
			var config = new RemoteServiceConfig(cmdline);
			var server = new RemoteService(config);	
		}
		
		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void cmdline_nolibraries()
		{
			var cmdline = new String[] {"-p","8271"};
			var config = new RemoteServiceConfig(cmdline);
			var server = new RemoteService(config);	
		}
		
		[Test]
		public void cmdline_valid_nodoc()
		{
			var cmdline = new String[] {"-k","NRobotRemote.Test.Keywords.dll:NRobotRemote.Test.Keywords.PublicClass","-p","8271"};
			var config = new RemoteServiceConfig(cmdline);
			var server = new RemoteService(config);	
		}
		
		[Test]
		public void cmdline_valid_withdoc()
		{
			var cmdline = new String[] {"-k","NRobotRemote.Test.Keywords.dll:NRobotRemote.Test.Keywords.PublicClass:NRobotRemote.Test.Keywords.xml","-p","8271"};
			var config = new RemoteServiceConfig(cmdline);
			var server = new RemoteService(config);	
		}
		
		[Test]
		public void cmdline_valid_multiple()
		{
			var cmdline = new String[] {"-k","NRobotRemote.Test.Keywords.dll:NRobotRemote.Test.Keywords.FirstClass:NRobotRemote.Test.Keywords.xml","NRobotRemote.Test.Keywords.dll:NRobotRemote.Test.Keywords.SecondClass:NRobotRemote.Test.Keywords.xml","-p","8271"};
			var config = new RemoteServiceConfig(cmdline);
			var server = new RemoteService(config);	
		}
		
	}
}
