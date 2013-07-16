using System;
using System.IO;
using System.Net;﻿  ﻿ 
using System.Diagnostics;
using System.Configuration;
using System.Reflection;
using log4net;
using NRobotRemote;
using System.Threading;
using CommandLine;

namespace NRobotRemoteConsole
{
﻿  
﻿  public class Program
﻿  {
﻿  ﻿  
	//log4net
	private static readonly ILog log = LogManager.GetLogger(typeof(Program));
﻿  ﻿  
	/// <summary>
	/// Entry point
	/// </summary>
﻿  ﻿  public static int Main(string[] args)
﻿  ﻿  {
﻿  ﻿  ﻿  
		try
		{
			//log
			Console.Title = "NRobotRemote";
			log.Info(String.Format("NRobotRemote v{0}",Assembly.GetExecutingAssembly().GetName().Version));
	        
			//get options
			log.Debug("Parsing command line arguments");
	        var options = new Options();
	        if (CommandLine.Parser.Default.ParseArguments(args,options)) 
	        {
	        	
	        	//start service
				RemoteService srv = new RemoteService(options.library,options.type,options.port,options.docfile);
				srv.StopRequested += OnStopHandler;
				srv.StartAsync();
				
				//wait
				System.Threading.Thread.Sleep(Timeout.Infinite);
				return 0;
	        }
		}
		catch (Exception e)
		{
			log.Error(e.ToString());
		}
		
		//abnormal exit
		System.Threading.Thread.Sleep(6000);
		return 1;

﻿  ﻿  }

	/// <summary>
	/// Event handler for stop_remote_server
	/// </summary>
	public static void OnStopHandler(object sender, EventArgs e)
	{
		log.Debug("Stop request was received - closing down");
		Environment.Exit(0);
	}
	
﻿  ﻿ 
﻿  }

}
﻿  
﻿