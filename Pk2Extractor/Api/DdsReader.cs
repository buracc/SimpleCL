using System;
using System.Drawing;
using System.IO;

namespace Pk2Extractor.Api
{
	public static class DdsReader
	{
		public static Bitmap FromFile(string fileName)
		{
			return DevIL.DevIL.LoadBitmap(fileName);
    }
		public static Bitmap FromDdj(string fileName)
		{
			byte[] ddjStream = File.ReadAllBytes(fileName);
			string tempFile = Path.GetTempFileName();
			byte[] ddsStream = ToDds(ddjStream);
			File.WriteAllBytes(tempFile, ddsStream);
			Bitmap bmp = DevIL.DevIL.LoadBitmap(tempFile);
			File.Delete(tempFile);
			return bmp;
		}
		public static Bitmap FromDdj(byte[] ddjBuffer)
		{
			string tempFile = Path.GetTempFileName();
			byte[] ddsBuffer = ToDds(ddjBuffer);
			File.WriteAllBytes(tempFile, ddsBuffer);
			Bitmap bmp = DevIL.DevIL.LoadBitmap(tempFile);
			File.Delete(tempFile);
			return bmp;
		}
		private static byte[] ToDds(byte[] ddjBuffer)
		{
			if (ddjBuffer.Length <= 0)
			{
				return ddjBuffer;
			}
			byte[] ddsBuffer = new byte[ddjBuffer.Length - 20];
			Array.Copy(ddjBuffer, 20, ddsBuffer, 0, ddsBuffer.Length);
			return ddsBuffer;
		}
	}
}
