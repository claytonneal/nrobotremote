using System;
using System.Runtime.Serialization;

namespace NRobotRemote
{
	/// <summary>
	/// Desctiption of KeywordDomainException.
	/// </summary>
	public class KeywordDomainException : Exception, ISerializable
	{
		public KeywordDomainException()
		{
		}

	 	public KeywordDomainException(string message) : base(message)
		{
		}

		public KeywordDomainException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// This constructor is needed for serialization.
		protected KeywordDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}