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
            Result.Add(0);

            return Result.ToArray();
        }
        static void Compress()
        {
            byte[] Decompressed = File.ReadAllBytes("TestFont.bin");
            List<byte> Compressed = new List<byte>();
            int position = 0, de = 0x8000, a, FF92 = 0x7E, FF93 = 1, FF9A = 0, FF9B = 0x98, Repetition = 0, TempPos = 0, bc = 0xFEE;
            while (position < Decompressed.Length)
            {
                Compressed.AddRange(search(Decompressed, ref position));
            }
        }
        static void Decompress()
        {
            byte[] ROM = File.ReadAllBytes("ShantaePure.gbc");
            List<byte> Final = new List<byte>();
            byte[] Temp = new byte[0x1000];
            int position = 0x3964A3, de = 0x8000, a, FF92 = 0x7E, FF93 = 1, FF9A = 0, FF9B = 0x98, Repetition = 0, TempPos = 0, bc=0xFEE;
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
                        TempPos += ((a >> 4) * 0x100);
                        Repetition = (a & 0xF) + 3;
                        do
                        {
                            Temp[bc] = Temp[TempPos];
                            Final.Add(Temp[bc]);
                            TempPos++;
                            de++;
                            bc++;
                            TempPos = (TempPos & 0xFF) + (((TempPos >> 8) & 0xF) * 0x100); //Reset TempPos
                            bc = (bc & 0xFF) + (((bc >> 8) & 0xF) * 0x100); //Reset bc
                        } while (--Repetition != 0);
                    }
                    else
                    {
                        Temp[bc] = ROM[position++];
                        Final.Add(Temp[bc]);
                        bc++;
                        de++;
                        bc = (bc & 0xFF) + (((bc >> 8) & 0xF) * 0x100); //Reset bc
                    }
                } while (FF9A != (de & 0xFF));
            }
            File.WriteAllBytes("TestFont.bin", Final.ToArray());
        }
    }
}