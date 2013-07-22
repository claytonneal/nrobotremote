using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace NRobotRemote.Test.Keywords
{
		/// <summary>
		/// Class for keywords for unit testing
		/// </summary>
        public class PublicClass
        {


#region valid method signatures no return value

			public void noreturnvalue_noarguments() { }
			public void noreturnvalue_stringarguments(string arg1, string arg2) { }
			public void noreturnvalue_stringarrayarguments(string[] arg1, string[] arg2) { }
			public void noreturnvalue_stringandarrayarguments(string arg1, string[] arg2) { }
	
#endregion

#region valid method signatures with return value

			public string stringreturnvalue() { return String.Empty; }
			public string[] stringarrayreturnvalue() { return new String[0];}
			public void noreturnvalue() { }
	
#endregion

#region invalid return value

			public object objectreturnvalue() { return new object(); }

#endregion

#region invalid parameter types

			public void intparameter(int arg1) { }
			public void objectparameter(object arg1) { }

#endregion

#region non public instance methods

			private void privatemethod() { }
			internal void internalmethod() { }
			protected void protectedmethod() { }
			public static void publicstaticmethod() { }
	
#endregion

#region methods for execution

	public void exec_noerror() { }
	public void exec_exception() { throw new Exception("keyword failed"); }
    public string exec_stringparam(string arg1, string arg2) { return (arg1 + arg2); }
	public void exec_withtrace() { Trace.Write("Hello"); }
    public string exec_returnsnull() { return null; }
    public bool exec_returnfalse() { return false; }
	public Boolean exec_returntrue() { return true; }
	public int exec_returnint() { return 100; }
	public Int32 exec_returnint32() { return 100; }
	public long exec_returnlong() { return 100; }
	public Int64 exec_returnint64() { return 100; }
	public double exec_returndouble() { return 3.14; }
	public Double exec_returnsysdouble() { return 3.14; }
	public String[] exec_returnstringarray(string arg1, string arg2) { return new String[] {arg1, arg2}; }
	
#endregion

#region methods for doc

	public void nodocmethod() { }
	
	/// <summary>
	/// A method with documentation
	/// </summary>
	/// <param name="arg1">parameter1</param>
	/// <param name="arg2">parameter2</param>
	/// <returns>param1+param2</returns>
	public string withdocmethod(string arg1, string arg2) { return (arg1+arg2); }
	

#endregion

#region obsolete keywords

	[Obsolete]
	public void obsolete_keyword() { }

#endregion
    
        }
}

