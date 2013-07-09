using System;
using System.IO;
using System.Net;﻿  ﻿
using CookComputing.XmlRpc;
using System.Reflection; 
using System.Xml;﻿  ﻿ 
using System.Xml.XPath; 
using System.Threading; 
using System.Diagnostics;
using log4net;
using NRobotRemote.Keywords;
using NRobotRemote.Doc;

namespace NRobotRemote.Services
{

	/// <summary>
﻿  	/// Class of XML-RPC methods for remote library (server)
﻿  	/// that conforms to RobotFramework remote library API
﻿  	/// </summary>
﻿ 	public class XmlRpcService : XmlRpcListenerService, IRemoteService
﻿  	{
﻿  ﻿  	
		//log4net
		private static readonly ILog log = LogManager.GetLogger(typeof(XmlRpcService));
		
		//properties
		private RemoteService _service;
		private const String CStopRemoteServer = "STOP REMOTE SERVER";
		private const String CIntro = "__INTRO__";
		private const String CInit = "__INIT__";
		
﻿  ﻿  ﻿
		public XmlRpcService(RemoteService service)
		{
			if (service==null) throw new Exception("No Service for XmlRpcService");
			 _service = service;﻿
		}
﻿  ﻿  
	﻿  ﻿  /// <summary>
	﻿  ﻿  /// Get a list of keywords available for use
	﻿  ﻿  /// </summary>
	﻿  ﻿  public string[] get_keyword_names()
	  ﻿  ﻿{
	﻿  ﻿  ﻿	try 
			{
				log.Debug("XmlRpc Method call - get_keyword_names");
				var kwnames =  _service._keywordmap.GetKeywordNames();
				kwnames.Add(CStopRemoteServer);
				var result = kwnames.ToArray();
				log.Debug("Method names are:");
				log.Debug(String.Join(",",result));
				return result;
			}
			catch (Exception e)
			{
				log.Error(String.Format("Exception in method - get_keyword_names : {0}",e.Message));
				throw new XmlRpcInternalErrorException(e.Message);
			}
	﻿  ﻿  }
﻿  ﻿  
	﻿  ﻿  /// <summary>
	﻿  ﻿  /// Run specified Robot Framework keyword from remote server.
	﻿  ﻿  /// </summary>
	﻿  ﻿  public XmlRpcStruct run_keyword(string keyword, object[] args)
	  ﻿  ﻿{
			log.Debug("XmlRpc Method call - run_keyword");
			XmlRpcStruct kr = new XmlRpcStruct();
			//check for stop remote server
			if (String.Equals(keyword,CStopRemoteServer,StringComparison.CurrentCultureIgnoreCase))
			{
				//start background thread to raise event
				Thread stopthread = new Thread(delayed_stop_remote_server);
				stopthread.IsBackground = true;
				stopthread.Start();
				log.Debug("Stop remote server thread started");
				//return success
				kr.Add("return",String.Empty);
				kr.Add("status","PASS");
				kr.Add("error",String.Empty);
				kr.Add("traceback",String.Empty);
				kr.Add("output",String.Empty);
			}
			else
			{
				try
				{
					var result = _service._keywordmap.Executor.ExecuteKeyword(keyword,args);
					kr.Add("return",result.@return);
					kr.Add("status",result.status);
					kr.Add("error",result.error);
					kr.Add("traceback",result.traceback);
					kr.Add("output",result.output);
				}
				catch (UnknownKeywordException e)
				{
					log.Error(String.Format("Exception in method - run_keyword : {0}",e.Message));
					throw new XmlRpcUnknownKeywordException(e.Message);
				}
				catch (Exception ee)
				{
					log.Error(String.Format("Exception in method - run_keyword : {0}",ee.Message));
					throw new XmlRpcInternalErrorException(ee.Message);
				}
			}
			return kr;
		}
		
		/// <summary>
	﻿  ﻿  /// Get list of arguments for specified Robot Framework keyword.
	﻿  ﻿  /// </summary>
	﻿  ﻿  public string[] get_keyword_arguments(string keyword)
	﻿  ﻿  {
			log.Debug("XmlRpc Method call - get_keyword_arguments");
			if (String.Equals(keyword,CStopRemoteServer,StringComparison.CurrentCultureIgnoreCase))
			{
				return null;
			}
			try
			{
				return _service._keywordmap.GetKeyword(keyword).ArgumentNames;
			}
			catch (UnknownKeywordException e)
			{
				log.Error(String.Format("Exception in method - get_keyword_arguments : {0}",e.Message));
				throw new XmlRpcUnknownKeywordException(e.Message);
			}
			catch (Exception ee)
			{
				log.Error(String.Format("Exception in method - get_keyword_arguments : {0}",ee.Message));
				throw new XmlRpcInternalErrorException(ee.Message);
			}
		
			
	﻿  ﻿  }﻿  ﻿  
			
	﻿  ﻿  /// <summary>
	﻿  ﻿  /// Get documentation for specified Robot Framework keyword.
	﻿  ﻿  /// Done by reading the .NET compiler generated XML documentation
	﻿  ﻿  /// for the loaded class library.
	﻿  ﻿  /// </summary>
	﻿  ﻿  /// <param name="keyword">The keyword to get documentation for.</param>
	﻿  ﻿  /// <returns>A documentation string for the given keyword.</returns>
	﻿  ﻿  public string get_keyword_documentation(string keyword)
	﻿  ﻿  {
		﻿  ﻿ 	log.Debug("XmlRpc Method call - get_keyword_documentation");
			//check for stop_remote_server
			if (String.Equals(keyword,CStopRemoteServer,StringComparison.CurrentCultureIgnoreCase))
			{
				return "Raises event to stop the remote server in the server host";
			}
			if(_service._keyworddoc!=null)
			{
				try
				{
					//check for INTRO 
					if (String.Equals(keyword,CIntro,StringComparison.CurrentCultureIgnoreCase))
					{
						return _service._keyworddoc.GetLibraryDoc();
					}
					//check for init
					if (String.Equals(keyword,CInit,StringComparison.CurrentCultureIgnoreCase))
					{
						return String.Empty;    
					}
					//get keyword documentation
					var kwd = _service._keywordmap.GetKeyword(keyword);
					var doc =  _service._keyworddoc.GetMethodDoc(kwd.Method);
					return doc;
				}
				catch (UnknownKeywordException e)
				{
					log.Error(String.Format("Exception in method - get_keyword_documentation : {0}",e.Message));
					throw new XmlRpcUnknownKeywordException(e.Message);
				}
				catch (Exception ee)
				{
					log.Error(String.Format("Exception in method - get_keyword_documentation : {0}",ee.Message));
					throw new XmlRpcInternalErrorException(ee.Message);
				}
				
			}
			else
			{
				return String.Empty;
			}
	﻿  ﻿  }

	﻿  	/// <summary>
		/// Raises event in RemoteService that a stop request was received
		/// </summary>
		public void stop_remote_server()
		{
			log.Debug("XmlRpc Method call - stop_remote_server");
			_service.OnStopRequested(EventArgs.Empty);
		}
	
		/// <summary>
		/// If stop remote server executed as keyword, need to delay to allow return value
		/// </summary>
		public void delayed_stop_remote_server()
		{
			System.Threading.Thread.Sleep(4000);
			stop_remote_server();
		}
	
}

}