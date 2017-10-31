using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication8
{
    class Program
    {
        static int Main()
        {
            Decompress();
            return 0;
        }
        static byte[] search(byte[] Decompressed, ref int position)
        {
            int TempPos = position;
            List<byte> Result = new List<byte>();

            return Result.ToArray();
        }
        static void Compress()
        {
            byte[] Decompressed = File.ReadAllBytes("TestFont.bin");
            List<byte> Compressed = new List<byte>();
            int position = 0, de = 0x8000, a, FF92 = 0x7E, FF9A = 0, FF9B = 0x98, Repetition = 0, TempPos = 0, starting = 0xEF, num = 0;
            while (position < Decompressed.Length)
            {
                Compressed.AddRange(search(Decompressed, ref position));
            }
        }
        static void Decompress()
        {
            byte[] ROM = File.ReadAllBytes("ShantaePure.gbc");
            List<byte> Final = new List<byte>();
            int position = 0x3964A3, de = 0x8000, a, FF92 = 0x7E, FF93 = 1, FF9A = 0, FF9B = 0x98, Repetition = 0, TempPos = 0, starting = 0xEF, num = 0;
            while (FF9B != ((de >> 8) & 0xFF))
            {
                do
                {
                    if (--FF93 != 0)
                        a = (FF92);
                    else
                    {
                        a = ROM[position++];
                        FF93 = 8;
                    }
                    FF92 = a >> 1;
                    if ((a & 0x1) == 0)
                    {
                        TempPos = ROM[position++];
                        a = ROM[position++];
                        if (a < starting - 0x80) //There has to be a better way
                            num += 0x10;
                        if (a > starting + 0x80)
                            num -= 0x10;
                        starting = a;
                        TempPos += (((a >> 4) + 0xD0 + num) * 0x100);
                        Repetition = (a & 0xF) + 3;
                        do
                        {
                            a = Final[TempPos - 0xDFEE];
                            Final.Add((byte)a);
                            TempPos++;
                            de++;
                        } while (--Repetition != 0);
                    }
                    else
                    {
                        a = ROM[position++];
                        Final.Add((byte)a);
                        de++;
                    }
                } while (FF9A != (de & 0xFF));
            }
            File.WriteAllBytes("TestFont.bin", Final.ToArray());
        }
    }
}