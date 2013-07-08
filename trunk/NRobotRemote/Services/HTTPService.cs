using System;
using System.Collections.Generic;
using System.Net;
using CookComputing.XmlRpc;
using log4net;
using System.Threading;
using System.Net.NetworkInformation;
using System.Collections.Specialized;
using System.Reflection;

namespace NRobotRemote.Services
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
		private Queue<HttpListenerContext> _requests;
		private volatile Boolean _isprocessing;
		private volatile Boolean _islistening;
		private volatile Boolean _processorstop;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public HTTPService(RemoteService service)
		{
			//check
			if (service==null) throw new Exception("Unable to instanciate HTTPService - Service instance specified");
			_service = service;
			//setup http listener
			_listener = new HttpListener();
			﻿_﻿listener.Prefixes.Add(String.Format("http://127.0.0.1:{0}/", _service._config.port));
			_requests = new Queue<HttpListenerContext>();
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
						log.Debug("Http request to close listener");
						reqcontext.Response.StatusCode = 200;
						reqcontext.Response.Close();
						break;
					}
					else 
					{
						if (method == "POST")
						{
						    log.Debug("Http request added to processor queue");
                    		_requests.Enqueue(reqcontext); 
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
			while (!_processorstop)
			{
				if (_requests.Count > 0)
				{
					HttpListenerContext context = _requests.Dequeue();
					_service._xmlrpcservice.ProcessRequest(context);
				}
			}
			_isprocessing = false;
			_processorstop = false;
		}
		
		/// <summary>
		/// Starts the http listener and processor async
		/// </summary>
		public void StartAsync()
		{
			if (!_islistening)
			{
                if (IsPortInUse()) throw new Exception("Unable to start service, port already in use");
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
                log.Debug("Sending HTTP request to stop listener service");
                WebRequest stopreq = WebRequest.Create(String.Format("http://127.0.0.1:{0}/", _service._config.port));
                stopreq.Method = "DELETE";
                WebResponse resp = stopreq.GetResponse();
			}
            _httpthread.Join(Timeout.Infinite);
            while (_islistening) { }
            
            //stop processor
            log.Debug("Stopping processor service");
            _requests.Clear();
            _processorstop = true;
            _keywordthread.Join(Timeout.Infinite);
            while (_isprocessing) { }

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
        	string html = String.Format("<html><body><h1>NRobotRemote</h1><p><table><tr><td>Version</td><td>{0}</td></tr></table><h2>Available Keywords</h2>{1}</body>",Assembly.GetExecutingAssembly().GetName().Version,_service._keyworddoc.GetHTMLDoc());
        	response.StatusCode = 200;
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(html);
			response.OutputStream.Write(buffer,0,buffer.Length);
			response.Close();
        }
		
		
	}
}
