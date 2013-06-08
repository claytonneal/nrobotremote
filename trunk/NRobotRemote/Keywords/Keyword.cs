using System;
using System.Reflection;
using log4net;
using System.Diagnostics;
using System.IO;

namespace NRobotRemote.Keywords
{
	/// <summary>
	/// Description of Keyword.
	/// </summary>
	public class Keyword
	{
		
		//private fields
		private MethodInfo _method;
		
		//log4net
		private static readonly ILog log = LogManager.GetLogger(typeof(Keyword));
		
		/// <summary>
		/// Get the keyword name
		/// </summary>
		public String Name
		{
			get
			{
				return KeywordNameParser.ToFriendlyName(_method.Name);
			}
		}
		
		/// <summary>
		/// Get the keyword argument names
		/// </summary>
		public string[] ArgumentNames
		{
			get
			{
				//get method parameters
	﻿  ﻿  ﻿  		ParameterInfo[] pis = _method.GetParameters();
	﻿  ﻿  ﻿  		string[] args = new String[pis.Length];
	﻿  ﻿  ﻿   		int i = 0;
	﻿  ﻿  ﻿  		foreach(ParameterInfo pi in pis)
	﻿  ﻿  ﻿  		{
	﻿  ﻿  ﻿  ﻿  		args[i++] = pi.Name;
	﻿  ﻿  ﻿  		}
	﻿  ﻿  ﻿  		return args;
			}
		}
		
		/// <summary>
		/// Get method for keyword
		/// </summary>
		public MethodInfo Method
		{
			get
			{
				return _method;
			}
		}
		
		/// <summary>
		/// Get the number of arguments to the keyword
		/// </summary>
		public int ArgumentCount
		{
			get
			{
				return _method.GetParameters().Length;
			}
		}
		
		/// <summary>
		/// Constructor from method
		/// </summary>
		public Keyword(MethodInfo method)
		{
			//check
			if (method==null) throw new ArgumentNullException("Cannot instanciate keyword from null method");
			_method = method;
		}
		
	}
}
