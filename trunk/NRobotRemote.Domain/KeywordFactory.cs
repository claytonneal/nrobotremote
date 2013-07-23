using System;
using System.Reflection;

namespace NRobotRemote.Domain
{
	/// <summary>
	/// Class to create a keyword from a method
	/// </summary>
	public class KeywordFactory
	{
		
		/// <summary>
		/// Creates a new keyword from a method, if cannot construct null returned
		/// </summary>
		public static Keyword CreateFromMethod(MethodInfo method, BuildMapOptions options)
		{
			if (HasValidSignature(method, options)) 
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
		private static Boolean HasValidSignature(MethodInfo mi, BuildMapOptions options)
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
			result = true;
			if (!mi.IsPublic) result = false;
			if (options == BuildMapOptions.OnlyStatic)
			{
				if (!mi.IsStatic) result = false;
			}
			//finish here if false
			if (!result) return result;
			
			//check if obsolete
			result = true;
			object[] methodattr = mi.GetCustomAttributes(false);
			if (methodattr.Length > 0)
			{
				for(int j = 0; j < methodattr.Length; j++)
				{
					if (methodattr[j].GetType().Equals(typeof(ObsoleteAttribute)))
					{
						result = false;
						break;
					}
				}
			}
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
