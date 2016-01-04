using System;
using System.Net;﻿  ﻿
using CookComputing.XmlRpc;
using log4net;
using NRobotRemote.Domain;
using NRobotRemote.Helpers;

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
		private static readonly ILog Log = LogManager.GetLogger(typeof(XmlRpcService));
		
		//constants
		private const String CIntro = "__INTRO__";
		private const String CInit = "__INIT__";

        //properties
	    private KeywordManager _keywordManager;

        //constructor
	    public XmlRpcService(KeywordManager keywordManager)
	    {
	        _keywordManager = keywordManager;
	    }


        /// <summary>
        /// Process xmlrpc request
        /// </summary>
        public override void ProcessRequest(HttpListenerContext requestContext)
        {
            base.ProcessRequest(requestContext);
        }


#region XmlRpcMethods

        /// <summary>
	﻿  ﻿  /// Get a list of keywords available for use
	﻿  ﻿  /// </summary>
	﻿  ﻿  public string[] get_keyword_names()
	  ﻿  ﻿{
	﻿  ﻿  ﻿	try 
			{
                Log.Debug("XmlRpc Method call - get_keyword_names");
			    return _keywordManager.GetAllKeywordNames();
			}
			catch (Exception e)
			{
				Log.Error(String.Format("Exception in method - get_keyword_names : {0}",e.Message));
				throw new XmlRpcFaultException(1,e.Message);
			}
	﻿  ﻿  }
﻿  ﻿  
	﻿  ﻿  /// <summary>
	﻿  ﻿  /// Run specified Robot Framework keyword
	﻿  ﻿  /// </summary>
	﻿  ﻿  public XmlRpcStruct run_keyword(string keyword, object[] args)
	  ﻿  ﻿{
			Log.Debug(String.Format("XmlRpc Method call - run_keyword {0}",keyword));
			XmlRpcStruct kr = new XmlRpcStruct();
			try
			{
			    var result = _keywordManager.RunKeyword(keyword, args);
				Log.Debug(result.ToString());
				kr = XmlRpcResultBuilder.ToXmlRpcResult(result);
			}
			catch (Exception e)
			{
				Log.Error(String.Format("Exception in method - run_keyword : {0}",e));
				throw new XmlRpcFaultException(1,e.Message);
			}
			return kr;
		}
		
		/// <summary>
	﻿  ﻿  /// Get list of arguments for specified Robot Framework keyword.
	﻿  ﻿  /// </summary>
	﻿  ﻿  public string[] get_keyword_arguments(string friendlyname)
	﻿  ﻿  {
            Log.Debug(String.Format("XmlRpc Method call - get_keyword_arguments {0}", friendlyname));
			try
			{
                var keyword = _keywordManager.GetKeyword(friendlyname);
			    return keyword.ArgumentNames;
			}
			catch (Exception e)
			{
				Log.Error(String.Format("Exception in method - get_keyword_arguments : {0}",e.Message));
				throw new XmlRpcFaultException(1,e.Message);
			}
	﻿  ﻿  }

	    /// <summary>
	    /// Get documentation for specified Robot Framework keyword.
	    /// Done by reading the .NET compiler generated XML documentation
	    /// for the loaded class library.
	    /// </summary>
	    /// <returns>A documentation string for the given keyword.</returns>
	    public string get_keyword_documentation(string friendlyname)
	﻿  ﻿  {
		﻿  ﻿ 	Log.Debug(String.Format("XmlRpc Method call - get_keyword_documentation {0}", friendlyname));
			try
			{
				//check for INTRO 
                if (String.Equals(friendlyname, CIntro, StringComparison.CurrentCultureIgnoreCase))
				{
                    return String.Empty;
				}
				//check for init
                if (String.Equals(friendlyname, CInit, StringComparison.CurrentCultureIgnoreCase))
				{
					return String.Empty;    
				}
				//get keyword documentation
                var keyword = _keywordManager.GetKeyword(friendlyname);
			    var doc = keyword.KeywordDocumentation;
				Log.Debug(String.Format("Keyword documentation, {0}",doc));
				return doc;
			}
			catch (Exception e)
			{
				Log.Error(String.Format("Exception in method - get_keyword_documentation : {0}",e.Message));
				throw new XmlRpcFaultException(1,e.Message);
			}
				
	﻿  ﻿  }


#endregion


    }

}