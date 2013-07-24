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
	public class AssemblyPathFixture
	{
		
		private static RemoteService _service;
		
		//constants
		private const String CLibrary = "NRobotRemote.Test.Keywords.dll";
		private const String CPort = "8271";
		private const String CDocFile = "";
		private const String CType = "NRobotRemote.Test.Keywords.StaticClass";
		private const String CUrl = "NRobotRemote/Test/Keywords/StaticClass";
		
		[Test]
		public void fixture_setup()
		{
			//copy NRobotRemote.Test.Keywords.dll to a temp directory
			string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
   			Directory.CreateDirectory(tempDirectory);
   			string asmpath = Path.Combine(tempDirectory,CLibrary);
   			File.Copy(CLibrary,asmpath);
   			
			//setup robot service
			_service = new RemoteService(asmpath,CType,CPort,CDocFile);
			_service.StartAsync();
		}
		
		[TestFixtureTearDown]
		public void fixture_teardown() 
		{
			//stop service
			_service.Stop();
		}
		
	}
}
