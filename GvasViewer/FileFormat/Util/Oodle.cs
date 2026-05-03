using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace GvasViewer.FileFormat.Util
{
	internal class Oodle
	{
		[DllImport("oo2core_9_win64.dll", CallingConvention = CallingConvention.Cdecl)]
		// return file size?
		private static extern long OodleLZ_Compress(
			int compressor,
			byte[] rawBuf,
			long rawLen,
			byte[] compBuf,
			int level,
			long option,
			IntPtr dictionaryBase,
			IntPtr lrm,
			IntPtr scratchMem,
			long scratchSize
		);

		[DllImport("oo2core_9_win64.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern long OodleLZ_Decompress(
			byte[] compBuf,
			long compBufSize,
			byte[] rawBuf,
			long rawLen,
			int fuzzSafe,
			int checkCRC,
			int verbosity,
			IntPtr decBufBase,
			long decBufSize,
			long fpCallback,
			long callbackUserData,
			IntPtr decoderMemory,
			long decoderMemorySize,
			int threadPhase
		);

		[DllImport("oo2core_9_win64.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern long OodleLZ_GetCompressedBufferSizeNeeded(
			int compressor,
			long rawSize
		);

		public Byte[] Compress(Byte[] src)
		{
			const int compressor = 8;
			long size = OodleLZ_GetCompressedBufferSizeNeeded(compressor, src.Length);
			Byte[] dest = new Byte[size];
			size = OodleLZ_Compress(compressor, src, src.Length, dest, 9, 0, 0, 0, 0, 0);
			return dest[..(int)size];
		}

		public Byte[] Decompress(Byte[] src, int dest_length)
		{
			Byte[] dest = new Byte[dest_length];
			OodleLZ_Decompress(src, src.Length, dest, dest.Length, 1, 0, 0, 0, 0, 0, 0, 0, 0, 3);
			return dest;
		}
	}
}
