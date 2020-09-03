using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace KanbanService
{
	public class FeigCommunication
	{
		private BinaryReader binaryReader;
		private BinaryWriter binaryWriter;
		private byte[] selectedAntennasByte;

		public FeigCommunication(ReadingSettings readingSettings, Stream stream)
		{
			binaryReader = new BinaryReader(stream);
			binaryWriter = new BinaryWriter(stream);
			selectedAntennasByte = HexToByte(SelectAntennas(readingSettings.Antennas));
		}

		public Inventory Inventory()
		{
			byte[] buffer = new byte[256];
			try
			{
				using (MemoryStream ms = new MemoryStream())
				{
					binaryWriter.Write(selectedAntennasByte);
					buffer = binaryReader.ReadBytes(3);

					ms.Write(buffer, 0, 3);
					ms.Write(binaryReader.ReadBytes(buffer[2] - 3), 0, buffer[2] - 3);

					buffer = ms.ToArray();
					string responseHex = ByteToHex(buffer);

					var response = new Inventory(responseHex);
					return response;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		private string ByteToHex(byte[] buffer)
		{
			StringBuilder hex = new StringBuilder();
			foreach (byte b in buffer)
			{
				hex.AppendFormat("{0:x2}", b); hex.Append(' ');
			}

			return hex.ToString();
		}

		private byte[] HexToByte(string data)
		{
			data = data.Replace(" ", "");
			byte[] result = new byte[data.Length / 2];
			for (int i = 0; i < data.Length; i += 2)
			{
				result[i / 2] = (byte)Convert.ToByte(data.Substring(i, 2), 16);
			}

			return result;
		}

		public string SelectAntennas(bool[] antennaNumber)
		{
			string data = "02 00 0A 00 B0 01 10";

			int A = 0;
			for (int i = 0; i < antennaNumber.Length; i++)
			{
				if (antennaNumber[i])
				{
					A += (int)Math.Pow(2, i);
				}
			}

			data = (A < 16) ? data += " 0" : data += " ";

			return data += A.ToString("X") + ChecksumGenerate(data += A.ToString("X"));
		}

		private string ChecksumGenerate(string text)
		{
			text = text.Replace(" ", "");
			byte[] bytes = new byte[text.Length / 2];
			for (int i = 0; i < text.Length; i += 2)
				bytes[i / 2] = (byte)Convert.ToByte(text.Substring(i, 2), 16);

			ushort crc = 0xFFFF;
			for (int k = 0; k < bytes.Length; k++)
			{
				crc = (ushort)(crc ^ bytes[k]);
				for (int i = 0; i < 8; i++)
					if ((crc & 0x0001) == 1) crc = (ushort)((crc >> 1) ^ 0x8408);
					else crc >>= 1;
			}
			return " " + crc.ToString("X").PadLeft(4, '0').Substring(2, 2) + " " + crc.ToString("X").PadLeft(4, '0').Substring(0, 2);
		}
	}
}
