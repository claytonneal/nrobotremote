using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace NRobotRemote.Domain
{
	/// <summary>
	/// Description of Keyword.
	/// </summary>
	public class Keyword
	{
		
		//fields
		internal MethodInfo KeywordMethod;
		internal String KeywordDocumentation;
		
		/// <summary>
		/// Get the keyword name
		/// </summary>
		public String Name
		{
			get
			{
				return KeywordNameParser.ToFriendlyName(KeywordMethod.Name);
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
	﻿  ﻿  ﻿  		ParameterInfo[] pis = KeywordMethod.GetParameters();
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
				return KeywordMethod;
			}
		}
		
		/// <summary>
		/// Get the number of arguments to the keyword
		/// </summary>
		public int ArgumentCount
		{
			get
			{
				return KeywordMethod.GetParameters().Length;
			}
		}
		
		/// <summary>
		/// Constructor from method
		/// </summary>
		public Keyword(MethodInfo method)
		{
			if (method==null) throw new ArgumentNullException("Cannot instanciate keyword from null method");
			KeywordMethod = method;
			KeywordDocumentation = String.Empty;
		}
		
	}
}
