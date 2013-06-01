using System;

namespace CommonTools.Web.JavaScript
{
	/// <summary>
	/// Parse Tokens
	/// </summary>
	internal enum JsonToken
	{
		End,
		Null,
		False,
		True,
		NaN,
		PositiveInfinity,
		NegativeInfinity,
		Number,
		String,
		ArrayStart,
		ArrayEnd,
		ObjectStart,
		ObjectEnd,
		NameDelim,
		ValueDelim
	}
}
