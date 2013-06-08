using System;
using System.Runtime.Serialization;

namespace NRobotRemote.Keywords
{
	/// <summary>
	/// Desctiption of InvalidKeywordArgumentsException.
	/// </summary>
	public class InvalidKeywordArgumentsException : Exception, ISerializable
	{
		public InvalidKeywordArgumentsException()
		{
		}

	 	public InvalidKeywordArgumentsException(string message) : base(message)
		{
		}

		public InvalidKeywordArgumentsException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// This constructor is needed for serialization.
		protected InvalidKeywordArgumentsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}