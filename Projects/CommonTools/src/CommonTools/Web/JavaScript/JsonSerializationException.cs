using System;

namespace CommonTools.Web.JavaScript
{
    /// <summary>
    /// 
    /// </summary>
	public class JsonSerializationException : InvalidOperationException
	{
		#region Init

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationException"/> class.
        /// </summary>
		public JsonSerializationException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
		public JsonSerializationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
		public JsonSerializationException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
		public JsonSerializationException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}

		#endregion Init
	}

    /// <summary>
    /// 
    /// </summary>
	public class JsonDeserializationException : JsonSerializationException
	{
		#region Fields

		private int index = -1;

		#endregion Fields

		#region Init

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDeserializationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="index">The index.</param>
		public JsonDeserializationException(string message, int index) : base(message)
		{
			this.index = index;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDeserializationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="index">The index.</param>
		public JsonDeserializationException(string message, Exception innerException, int index)
			: base(message, innerException)
		{
			this.index = index;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDeserializationException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
		public JsonDeserializationException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets the character position in the stream where the error occurred.
		/// </summary>
		public int Index
		{
			get { return this.index; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Helper method which converts the index into Line and Column numbers
		/// </summary>
		/// <param name="source"></param>
		/// <param name="line"></param>
		/// <param name="col"></param>
		public void GetLineAndColumn(string source, out int line, out int col)
		{
			if (source == null)
			{
				throw new ArgumentNullException();
			}

			col = 1;
			line = 1;

			bool foundLF = false;
			int i = Math.Min(this.index, source.Length);
			for (; i>0; i--)
			{
				if (!foundLF)
				{
					col++;
				}
				if (source[i-1] == '\n')
				{
					line++;
					foundLF = true;
				}
			}
		}

		#endregion Methods
	}
}
