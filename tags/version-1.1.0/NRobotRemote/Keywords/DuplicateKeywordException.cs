using System;
using System.Runtime.Serialization;

namespace NRobotRemote.Keywords
{
	/// <summary>
	/// Desctiption of DuplicateKeywordException.
	/// </summary>
	public class DuplicateKeywordException : Exception, ISerializable
	{
		public DuplicateKeywordException()
		{
		}

	 	public DuplicateKeywordException(string message) : base(message)
		{
		}

		public DuplicateKeywordException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// This constructor is needed for serialization.
		protected DuplicateKeywordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}