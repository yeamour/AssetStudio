/* Copyright 2015 Google Inc. All Rights Reserved.

Distributed under MIT license.
See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
*/
namespace Org.Brotli.Dec
{
	public sealed class State
	{
		public int runningState = Org.Brotli.Dec.RunningState.Uninitialized;

		public int nextRunningState;

		public readonly Org.Brotli.Dec.BitReader br = new Org.Brotli.Dec.BitReader();

		public byte[] ringBuffer;

		public readonly int[] blockTypeTrees = new int[3 * Org.Brotli.Dec.Huffman.HuffmanMaxTableSize];

		public readonly int[] blockLenTrees = new int[3 * Org.Brotli.Dec.Huffman.HuffmanMaxTableSize];

		public int metaBlockLength;

		public bool inputEnd;

		public bool isUncompressed;

		public bool isMetadata;

		public readonly Org.Brotli.Dec.HuffmanTreeGroup hGroup0 = new Org.Brotli.Dec.HuffmanTreeGroup();

		public readonly Org.Brotli.Dec.HuffmanTreeGroup hGroup1 = new Org.Brotli.Dec.HuffmanTreeGroup();

		public readonly Org.Brotli.Dec.HuffmanTreeGroup hGroup2 = new Org.Brotli.Dec.HuffmanTreeGroup();

		public readonly int[] blockLength = new int[3];

		public readonly int[] numBlockTypes = new int[3];

		public readonly int[] blockTypeRb = new int[6];

		public readonly int[] distRb = new int[] { 16, 15, 11, 4 };

		public int pos = 0;

		public int maxDistance = 0;

		public int distRbIdx = 0;

		public bool trivialLiteralContext = false;

		public int literalTreeIndex = 0;

		public int literalTree;

		public int j;

		public int insertLength;

		public byte[] contextModes;

		public byte[] contextMap;

		public int contextMapSlice;

		public int distContextMapSlice;

		public int contextLookupOffset1;

		public int contextLookupOffset2;

		public int treeCommandOffset;

		public int distanceCode;

		public byte[] distContextMap;

		public int numDirectDistanceCodes;

		public int distancePostfixMask;

		public int distancePostfixBits;

		public int distance;

		public int copyLength;

		public int copyDst;

		public int maxBackwardDistance;

		public int maxRingBufferSize;

		public int ringBufferSize = 0;

		public long expectedTotalSize = 0;

		public byte[] customDictionary = new byte[0];

		public int bytesToIgnore = 0;

		public int outputOffset;

		public int outputLength;

		public int outputUsed;

		public int bytesWritten;

		public int bytesToWrite;

		public byte[] output;

		// Current meta-block header information.
		// TODO: Update to current spec.
		private static int DecodeWindowBits(Org.Brotli.Dec.BitReader br)
		{
			if (Org.Brotli.Dec.BitReader.ReadBits(br, 1) == 0)
			{
				return 16;
			}
			int n = Org.Brotli.Dec.BitReader.ReadBits(br, 3);
			if (n != 0)
			{
				return 17 + n;
			}
			n = Org.Brotli.Dec.BitReader.ReadBits(br, 3);
			if (n != 0)
			{
				return 8 + n;
			}
			return 17;
		}

		/// <summary>Associate input with decoder state.</summary>
		/// <param name="state">uninitialized state without associated input</param>
		/// <param name="input">compressed data source</param>
		public static void SetInput(Org.Brotli.Dec.State state, System.IO.Stream input)
		{
			if (state.runningState != Org.Brotli.Dec.RunningState.Uninitialized)
			{
				throw new System.InvalidOperationException("State MUST be uninitialized");
			}
			Org.Brotli.Dec.BitReader.Init(state.br, input);
			int windowBits = DecodeWindowBits(state.br);
			if (windowBits == 9)
			{
				/* Reserved case for future expansion. */
				throw new Org.Brotli.Dec.BrotliRuntimeException("Invalid 'windowBits' code");
			}
			state.maxRingBufferSize = 1 << windowBits;
			state.maxBackwardDistance = state.maxRingBufferSize - 16;
			state.runningState = Org.Brotli.Dec.RunningState.BlockStart;
		}

		/// <exception cref="System.IO.IOException"/>
		public static void Close(Org.Brotli.Dec.State state)
		{
			if (state.runningState == Org.Brotli.Dec.RunningState.Uninitialized)
			{
				throw new System.InvalidOperationException("State MUST be initialized");
			}
			if (state.runningState == Org.Brotli.Dec.RunningState.Closed)
			{
				return;
			}
			state.runningState = Org.Brotli.Dec.RunningState.Closed;
			Org.Brotli.Dec.BitReader.Close(state.br);
		}
	}
}
