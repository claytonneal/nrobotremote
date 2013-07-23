using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace NRobotRemote.Domain
{
	/// <summary>
	/// Description of KeywordExecutor.
	/// </summary>
	public class KeywordExecutor
	{
		
		//keyword map
		private KeywordMap _keywords;
		private Object _instance;
		private TextWriterTraceListener _tracelistener;
        private MemoryStream _tracecontent;
        private Stopwatch _timer;
		
		/// <summary>
		/// Constructor from keyword map
		/// </summary>
		public KeywordExecutor(KeywordMap map, object instance)
		{
			if (map==null) throw new ArgumentNullException("Unable to instanciate keyword executor - null keyword map");
			_keywords = map;
			_instance = instance;
			_tracecontent = new MemoryStream();
            _tracelistener = new TextWriterTraceListener(_tracecontent);
            _timer = new Stopwatch();
		}
		
		/// <summary>
		/// Executes keyword with arguments, returns a keyword result
		/// </summary>
		public KeywordResult ExecuteKeyword(string name, object[] args)
		{
			//setup
			var keyword = _keywords.GetKeyword(name);
			var method = keyword.Method;
			var kwresult = new KeywordResult();
			var numargs = keyword.ArgumentCount;
			//check number of arguments
			if (args.Length!=numargs) throw new InvalidKeywordArgumentsException("Incorrect number of keyword arguments supplied");
            //setup trace listener
            Trace.Listeners.Add(_tracelistener);
			//invoke
			_timer.Start();
			try 
			{
				if (method.ReturnParameter.ParameterType.Equals(typeof(void)))
				{
					method.Invoke(_instance, args);
					kwresult.@return = "";
				}
				else
				{
					kwresult.@return = method.Invoke(_instance, args);
					if (kwresult.@return==null)
					{
						kwresult.@return = "";
					}
				}
				kwresult.status = KeywordStatus.PASS;
			}
			catch (Exception e)
			{
				//invoke exception, inner exception gives keyword exception
				if (e.InnerException!=null)
				{
					kwresult.traceback = e.InnerException.StackTrace;
	﻿  ﻿  ﻿  ﻿  		kwresult.error = e.InnerException.Message;
				}
				else
				{
					kwresult.traceback = e.StackTrace;
	﻿  ﻿  ﻿  ﻿  		kwresult.error = e.Message;
				}
				kwresult.status = KeywordStatus.FAIL;
				kwresult.@return = "";
			}
			//stop timer
			_timer.Stop();
			kwresult.duration = _timer.Elapsed.TotalSeconds;	
            //stop trace listener
            _tracelistener.Flush();
            Trace.Listeners.Remove(_tracelistener);
            kwresult.output = System.Text.Encoding.Default.GetString(_tracecontent.ToArray());
            _tracecontent.SetLength(0);
            //finish
			return kwresult;
		}
		
		
	}
}
