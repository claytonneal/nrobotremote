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
		private KeywordMap _keywordmap;
		private LibraryDoc _keydoc;
﻿  ﻿  ﻿
		public XmlRpcService(KeywordMap map, LibraryDoc doc = null)
		{
			if (map==null) throw new Exception("No Keyword Map instance");
			 _keywordmap = map;
			 _keydoc = doc;﻿
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
				string[] result =  _keywordmap.GetKeywordNames();
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
			try
			{
				log.Debug("XmlRpc Method call - run_keyword");
				var result = _keywordmap.Executor.ExecuteKeyword(keyword,args);
				XmlRpcStruct kr = new XmlRpcStruct();
				kr.Add("return",result.@return);
				kr.Add("status",result.status);
				kr.Add("error",result.error);
				kr.Add("traceback",result.traceback);
				kr.Add("output",result.output);
				return kr;
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
		
		/// <summary>
	﻿  ﻿  /// Get list of arguments for specified Robot Framework keyword.
	﻿  ﻿  /// </summary>
	﻿  ﻿  public string[] get_keyword_arguments(string keyword)
	﻿  ﻿  {
			try
			{
				log.Debug("XmlRpc Method call - get_keyword_arguments");
				return _keywordmap.GetKeyword(keyword).ArgumentNames;
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
			if(_keydoc!=null)
			{
				var kwd = _keywordmap.GetKeyword(keyword);
				var doc =  _keydoc.GetMethodDoc(kwd.Method);
				return doc;
			}
			else
			{
				return String.Empty;
			}
	﻿  ﻿  }

﻿  
}

}