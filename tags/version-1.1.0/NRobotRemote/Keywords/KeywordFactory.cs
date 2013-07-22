using System;
using System.Reflection;

namespace NRobotRemote.Keywords
{
	/// <summary>
	/// Description of KeywordFactory.
	/// </summary>
	public class KeywordFactory
	{
		
		/// <summary>
		/// Creates a new keyword from a method, if cannot construct null returned
		/// </summary>
		public static Keyword CreateFromMethod(MethodInfo method)
		{
			if (HasValidSignature(method)) 
			{
				return new Keyword(method);
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>
		/// Checks method signature for keyword suitability
		/// </summary>
		private static Boolean HasValidSignature(MethodInfo mi)
		{
			Boolean result = false;
			
			//check return types (void, string, boolean, int32, int64, double, string[] )
			Type returntype = mi.ReturnParameter.ParameterType;
			if (returntype.Equals(typeof(void)))
			{
				result = true;
			}
			if (returntype.Equals(typeof(System.String)))
			{
				result = true;
			}
			if (returntype.Equals(typeof(System.Boolean)))
			{
				result = true;
			}
			if (returntype.Equals(typeof(System.Int32)))
			{
				result = true;
			}
			if (returntype.Equals(typeof(System.Int64)))
			{
				result = true;
			}
			if (returntype.Equals(typeof(System.Double)))
			{
				result = true;
			}
			if (returntype.IsArray && returntype.GetElementType().Equals(typeof(System.String)))
			{
				result = true;
			}
			//finish here if false
			if (!result) return result;
			
			//check method access
			if (mi.IsPublic) result = true;
			if (mi.IsStatic) result = false;
			//finish here if false
			if (!result) return result;
			
			//check argument types
			result = true;
			ParameterInfo[] parameters = mi.GetParameters();
			foreach(ParameterInfo par in parameters)
			{
				if (par.ParameterType!=typeof(System.String))
				{
					result = false;
					break;
				}
			}
			//finish
			return result;
		}
		
	}
}
