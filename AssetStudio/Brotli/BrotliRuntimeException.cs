/* Copyright 2015 Google Inc. All Rights Reserved.

Distributed under MIT license.
See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
*/
namespace Org.Brotli.Dec
{
	/// <summary>Unchecked exception used internally.</summary>
	[System.Serializable]
	public class BrotliRuntimeException : System.Exception
	{
		public BrotliRuntimeException(string message)
			: base(message)
		{
		}

		public BrotliRuntimeException(string message, System.Exception cause)
			: base(message, cause)
		{
		}
	}
}
