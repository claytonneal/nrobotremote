using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using CookComputing.XmlRpc;
using log4net;
using System.Threading;
using System.Net.NetworkInformation;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using NRobotRemote.Domain;

namespace NRobotRemote
{
	/// <summary>
	/// HTTP Listener service
	/// </summary>
	public class HTTPService
	{
		
		//log4net
		private static readonly ILog log = LogManager.GetLogger(typeof(HTTPService));
		
		//properties
		private RemoteService _service;
		private HttpListener _listener;
		private Thread _httpthread;
		private Thread _keywordthread;
		private BlockingCollection<HttpListenerContext> _requests;
		private volatile Boolean _isprocessing;
		private volatile Boolean _islistening;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public HTTPService(RemoteService service)
		{
			//check
			if (service==null) throw new Exception("Unable to instanciate HTTPService - Service instance not specified");
			_service = service;
			//setup http listener
			_listener = new HttpListener();
			﻿_﻿listener.Prefixes.Add(String.Format("http://*:{0}/", _service._config.port));
            //set statuses
            _islistening = false;
            _isprocessing = false;
		}
		
		/// <summary>
		/// Background HTTP Listener thread
		/// </summary>
		private void DoWork_Listener()
		{
			log.Debug(String.Format("HTTP Listener started on port {0}",_service._config.port));
			_﻿listener.Start();
			_islistening = true;
			﻿while (true)
		﻿  ﻿  ﻿{
		﻿  ﻿  ﻿	try
				{
					var reqcontext = _listener.GetContext();
					var method = reqcontext.Request.HttpMethod;
					log.Debug(String.Format("Received Http request with method {0}",method));
					if (method == "DELETE")
					{
						log.Debug("Http request to close");
						reqcontext.Response.StatusCode = 200;
						reqcontext.Response.Close();
						_requests.Add(reqcontext);
						break;
					}
					else 
					{
						if (method == "POST")
						{
						    log.Debug("Http request added to processor queue");
                    		_requests.Add(reqcontext); 
						}
						else
						{
							WriteStatusPage(reqcontext.Response);
						}
					}
					
					 ﻿
				}
				catch(Exception e)
				{
					log.Error(e.ToString());
				}
		﻿  ﻿  ﻿}
            _listener.Stop();
            _islistening = false;
		}
		
		/// <summary>
		/// Processes requests from listener thread
		/// </summary>
		private void DoWork_Processor()
		{
			_isprocessing = true;
			while (true)
			{

				//block until request to process
				HttpListenerContext context = _requests.Take();
				
				//check for stop
				var method = context.Request.HttpMethod;
				if (method == "DELETE")
				{
					break;
				}
				
				log.Debug(String.Format("Processing Http request for Url : {0}",context.Request.Url));
				try
				{
					//check
					if ((context.Request.Url.Segments==null)||(context.Request.Url.Segments.Length==0))
					{
						log.Warn(String.Format("Invalid url in request : {0}",context.Request.Url));
						context.Response.StatusCode = 404;
						context.Response.Close();
					}
					else
					{
						//get full type name from url
						var seg = context.Request.Url.Segments;
						var type = String.Join("",seg,1,seg.Length-1).Replace("/",".");
						//check map
						if (!_service._keywordmaps.ContainsMap(type))
						{
							log.Error(String.Format("No keyword map found for type : {0}",type));
							context.Response.StatusCode = 404;
							context.Response.Close();
						}
						else
						{
							//process request with keyword map
							var map = _service._keywordmaps.GetMap(type);
							_service._xmlrpcservice.ProcessRequest(context,map);
						}
						
					}
					
				}
				catch (Exception e)
				{
					log.Error(String.Format("Error in HTTP worker thread {0}",e.ToString()));
					context.Response.StatusCode = 500;
					context.Response.Close();
				}
					
			}
			_isprocessing = false;
		}
		
		/// <summary>
		/// Starts the http listener and processor async
		/// </summary>
		public void StartAsync()
		{
			if (!_islistening)
			{
                if (IsPortInUse()) throw new Exception("Unable to start service, port already in use");
                _requests = new BlockingCollection<HttpListenerContext>();
                _httpthread = new Thread(DoWork_Listener);
            	_httpthread.IsBackground = true;
                _httpthread.Start();
			}
			if (!_isprocessing)
			{
				_keywordthread = new Thread(DoWork_Processor);
            	_keywordthread.IsBackground = true;
				_keywordthread.Start();
			}
			//wait for threads
			while (!_islistening) { }
			while (!_isprocessing) { }
		}
		
		/// <summary>
		/// Stop http listener and processor
		/// </summary>
		public void Stop()
		{
			//stop listener
			if (_islistening)
			{
				//send DELETE method call
                log.Debug("Sending HTTP request to stop");
                WebRequest stopreq = WebRequest.Create(String.Format("http://127.0.0.1:{0}/", _service._config.port));
                stopreq.Method = "DELETE";
                WebResponse resp = stopreq.GetResponse();
			}
            _httpthread.Join(Timeout.Infinite);
            while (_islistening) { }
            
            //stop processor
            log.Debug("Waiting for http worker thread");
            _keywordthread.Join(Timeout.Infinite);
            while (_isprocessing) { }
            _requests.Dispose();

		}

        /// <summary>
        /// Checks if port is available
        /// </summary>
        private Boolean IsPortInUse()
        {
            bool inUse = false;
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == _service._config.port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }
        
        /// <summary>
        /// Writes status page to a browser
        /// </summary>
        private void WriteStatusPage(HttpListenerResponse response)
        {
        	try
        	{
        		StringBuilder html = new StringBuilder();
        		//setup html doc
        		html.Append(String.Format("<html><body><h1>NRobotRemote</h1><p><table><tr><td>Version</td><td>{0}</td></tr></table><h2>Available Keywords</h2>",Assembly.GetExecutingAssembly().GetName().Version));
	        	//per map html
	        	foreach(KeywordMap map in _service._keywordmaps)
	        	{
	        		html.Append(map.GetHTMLDoc());
	        	}
	        	//finish html
	        	html.Append("</body></html>");
        		response.StatusCode = 200;
        		byte[] buffer = System.Text.Encoding.UTF8.GetBytes(html.ToString());
				response.OutputStream.Write(buffer,0,buffer.Length);
        	}
        	catch (Exception e)
        	{
        		log.Error(e.ToString());
        		response.StatusCode = 500;
        	}
        	response.Close();
        }
		
		
	}
}
