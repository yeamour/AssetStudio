/* Copyright 2015 Google Inc. All Rights Reserved.

Distributed under MIT license.
See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
*/
namespace Org.Brotli.Dec
{
	/// <summary>Enumeration of all possible word transformations.</summary>
	/// <remarks>
	/// Enumeration of all possible word transformations.
	/// <p>There are two simple types of transforms: omit X first/last symbols, two character-case
	/// transforms and the identity transform.
	/// </remarks>
	public sealed class WordTransformType
	{
		public const int Identity = 0;

		public const int OmitLast1 = 1;

		public const int OmitLast2 = 2;

		public const int OmitLast3 = 3;

		public const int OmitLast4 = 4;

		public const int OmitLast5 = 5;

		public const int OmitLast6 = 6;

		public const int OmitLast7 = 7;

		public const int OmitLast8 = 8;

		public const int OmitLast9 = 9;

		public const int UppercaseFirst = 10;

		public const int UppercaseAll = 11;

		public const int OmitFirst1 = 12;

		public const int OmitFirst2 = 13;

		public const int OmitFirst3 = 14;

		public const int OmitFirst4 = 15;

		public const int OmitFirst5 = 16;

		public const int OmitFirst6 = 17;

		public const int OmitFirst7 = 18;

		public const int OmitFirst8 = 19;

		public const int OmitFirst9 = 20;

		public static int GetOmitFirst(int type)
		{
			return type >= OmitFirst1 ? (type - OmitFirst1 + 1) : 0;
		}

		public static int GetOmitLast(int type)
		{
			return type <= OmitLast9 ? (type - OmitLast1 + 1) : 0;
		}
	}
}
