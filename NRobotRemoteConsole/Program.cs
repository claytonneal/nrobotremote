using System;
using System.IO;
using System.Net;﻿  ﻿ 
using System.Diagnostics;
using System.Configuration;
using System.Reflection;
using log4net;
using NRobotRemote;
using System.Threading;

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
		//log
		Console.Title = "NRobotRemote";
		log.Info(String.Format("NRobotRemote v{0}",Assembly.GetExecutingAssembly().GetName().Version));
		try
		{
			//get options
			log.Debug("Parsing command line arguments");
			var config = new RemoteServiceConfig(args);
	        	
	        //start service
			RemoteService srv = new RemoteService(config);
			srv.StopRequested += OnStopHandler;
			srv.StartAsync();
				
			//wait
			System.Threading.Thread.Sleep(Timeout.Infinite);
			return 0;
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