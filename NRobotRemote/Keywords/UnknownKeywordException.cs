using System;
using System.Runtime.Serialization;

namespace NRobotRemote.Keywords
{
	/// <summary>
	/// Desctiption of UnknownKeywordException.
	/// </summary>
	public class UnknownKeywordException : Exception, ISerializable
	{
		public UnknownKeywordException()
		{
		}

	 	public UnknownKeywordException(string message) : base(message)
		{
		}

		public UnknownKeywordException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// This constructor is needed for serialization.
		protected UnknownKeywordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}