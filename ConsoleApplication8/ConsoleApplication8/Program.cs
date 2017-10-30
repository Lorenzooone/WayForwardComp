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
        static void Main()
        {
            Decompress();
        }
        static void Compress()
        {
            byte[] Decompressed = File.ReadAllBytes("TestFont.bin");
            List<byte> Compressed = new List<byte>();
            int position = 0, de = 0x90B0, a, FF92 = 7, temp = 0, f = 0xD0, FF93 = 4, FF94 = 0, bc = 0xD09E, FF9A = 0, FF9B = 0x98, FF95 = 0, FF9C = 0, TempPos = 0;
            while (position < Decompressed.Length)
            {

            }
        }
        static void Decompress()
        {
            byte[] ROM = File.ReadAllBytes("ShantaePure.gbc");
            List<byte> Final = new List<byte>();
            int position = 0x3964A3, de = 0x8000, a, FF92 = 0x7E, temp = 0, f = 0xD0, FF93 = 1, bc = 0xDFEE, FF9A = 0, FF9B = 0x98, Repetition = 0, TempPos = 0, starting = 0xEF, num = 0;
            while (position < 0x3969E6)
            {
                do
                {
                    FF93 -= 1;
                    if (FF93 != 0)
                    {
                        a = (FF92);
                        FF92 = a >> 1;
                        if ((a & 0x1) == 0)
                        {
                            TempPos = ROM[position++];
                            a = ROM[position++];
                            if (a < starting - 0xE0) //There has to be a better way
                                num += 0x10;
                            if (a > starting + 0xE0)
                                num -= 0x10;
                            starting = a;
                            TempPos += (((a >> 4) + 0xD0 + num) * 0x100);
                            Repetition = (a & 0xF) + 3;
                            do
                            {
                                if (TempPos >= 0xDFEF)
                                    a = Final[TempPos - 0xDFEE];
                                else
                                    a = 0;
                                Final.Add((byte)a);
                                TempPos++;
                                de++;
                                bc++;
                                bc = (bc & 0xFF) + ((((bc >> 8) & 0xF) + 0xD0) * 0x100); //Reset bc
                                Repetition -= 1;
                            } while (Repetition != 0);
                            f = 0;
                            break;
                        }
                        a = ROM[position];
                        Final.Add((byte)a);
                        position += 1;
                        de += 1;
                        bc += 1;
                        bc = (bc & 0xFF) + ((((bc >> 8) & 0xF) + 0xD0) * 0x100); //Reset bc
                    }
                    else
                    {
                        a = ROM[position++];
                        FF93 = 8;
                        temp = a >> 1;
                        if (temp == 0)
                            f = 0x80 + ((a & 0x1) * 0x10);
                        else
                            f = ((a & 0x1) * 0x10);
                        a = temp;
                        FF92 = a;
                        if ((f & 0x10) == 0)
                        {
                            TempPos = ROM[position++];
                            a = ROM[position++];
                            if (a < starting - 0xE0) //There has to be a better way
                                num += 0x10;
                            if (a > starting + 0xE0)
                                num -= 0x10;
                            starting = a;
                            TempPos += (((a >> 4) + 0xD0 + num) * 0x100);
                            Repetition = (a & 0xF) + 3;
                            do
                            {
                                if (TempPos >= 0xDFEF)
                                    a = Final[TempPos - 0xDFEE];
                                else
                                    a = 0;
                                Final.Add((byte)a);
                                TempPos++;
                                de++;
                                bc++;
                                bc = (bc & 0xFF) + ((((bc >> 8) & 0xF) + 0xD0) * 0x100); //Reset bc
                                Repetition -= 1;
                            } while (Repetition != 0);
                            f = 0;
                            break;
                        }
                        a = ROM[position++];
                        Final.Add((byte)a);
                        de += 1;
                        bc += 1;
                        bc = (bc & 0xFF) + ((((bc >> 8) & 0xF) + 0xD0) * 0x100); //Reset bc
                    }
                } while (FF9A != (de & 0xFF));
                if (FF9B == ((de >> 8) & 0xFF))
                {
                    break;
                }
            }
            File.WriteAllBytes("TestFont.bin", Final.ToArray());
        }
    }
}