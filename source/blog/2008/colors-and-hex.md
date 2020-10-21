---
Title: Colors and Hex
Date: 2008-12-21
Tags: .net, xna
---

I recently needed to write out Color(s) as an xml attribute. I wrote 2 methods to read and write the Color(s) as Hex strings. Here ya go:

<!--more-->

```c#
namespace Snow.Xna.Graphics
{
	public static class ColorHelper
	{
		private static char[] _hexDigits = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

		public static string ToHexString(Color color)
		{
			byte[] bytes = new byte[4];

			bytes[0] = color.A;
			bytes[1] = color.R;
			bytes[2] = color.G;
			bytes[3] = color.B;

			char[] chars = new char[8];

			for(int i = 0; i < 4; i++)
			{
				int b = bytes[i];
				chars[i * 2] = _hexDigits[b >> 4];
				chars[i * 2 + 1] = _hexDigits[b & 0xF];
			}

			return new string(chars);
		}

		private static byte HexDigitToByte(char c)
		{
			switch(c)
			{
				case '0': return (byte)0;
				case '1': return (byte)1;
				case '2': return (byte)2;
				case '3': return (byte)3;
				case '4': return (byte)4;
				case '5': return (byte)5;
				case '6': return (byte)6;
				case '7': return (byte)7;
				case '8': return (byte)8;
				case '9': return (byte)9;
				case 'A': return (byte)10;
				case 'B': return (byte)11;
				case 'C': return (byte)12;
				case 'D': return (byte)13;
				case 'E': return (byte)14;
				case 'F': return (byte)15;
			}

			return (byte)0;
		}

		public static Color FromHexString(string hex)
		{
			if( hex.Length != 8 )
				return Color.Black;

			int a = (HexDigitToByte(hex[0]) << 4) + HexDigitToByte(hex[1]);
			int r = (HexDigitToByte(hex[2]) << 4) + HexDigitToByte(hex[3]);
			int g = (HexDigitToByte(hex[4]) << 4) + HexDigitToByte(hex[5]);
			int b = (HexDigitToByte(hex[6]) << 4) + HexDigitToByte(hex[7]);

			return new Color((byte)r, (byte)g, (byte)b, (byte)a);
		}
	}
}
```
