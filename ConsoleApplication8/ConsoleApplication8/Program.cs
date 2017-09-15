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
            List<byte> Final =new List<byte>();
            int position = 0x3966B0, de = 0x90B0, a, FF92 = 7, temp = 0, f = 0xD0, FF93 = 4, bc = 0xD09E, FF9A=0, FF9B=0x98, FF9C=0, TempPos=0;
            while (position < 0x3969E6)
            {
                do
                {
                    FF93 -= 1;
                    if (FF93 != 0)
                    {
                        a = (FF92);
                        FF92 = a>>1;
                        if ((a & 0x1) == 0)
                        {
                            TempPos=ROM[position++];
                            a = ROM[position];
                            TempPos += (((a>>4)+0xD0)*0x100);
                            position++;
                            a = (a & 0xF)+3;
                            f = 0;
                            FF9C = a;
                            do
                            {
                                f += 0xA0;
                                if (TempPos >= 0xD09E)
                                    a = Final[TempPos - 0xD09E];
                                else
                                    a = 0;
                                Final.Add((byte)a);
                                a = 0xC0;
                                a = a & 0x3;
                                f = 0x40;
                                if (a == 3)
                                    f += 0x80;
                                if (a < 3)
                                    f += 0x10;
                                if (((a >> 3) & 0x1) == 1)
                                    f += 0x20;
                                if (f >= 0x80)
                                {

                                }
                                TempPos++;
                                de++;
                                bc++;
                                a = TempPos >> 8;
                                a = a & 0xF;
                                a += 0xD0;
                                TempPos = (TempPos & 0xFF) + (a * 0x100);
                                a = bc >> 8;
                                a = a & 0xF;
                                a += 0xD0;
                                bc = (bc & 0xFF) + (a * 0x100);
                                a = FF9A;
                                f = 0;
                                if (a < (de & 0xFF))
                                    f = 0x10;
                                a = FF9C;
                                f = f & 0x10;
                                if ((a >> 3) == 1) { }
                                else
                                    f += 0x20;
                                f += 0x40;
                                if (a - 1 == 0)
                                    f += 0x80;
                                FF9C = a - 1;
                            } while (f < 0x80);
                            f = 0;
                            break;
                        }
                        a = ROM[position];
                        Final.Add((byte)a);
                        position += 1;
                        de += 1;
                        bc += 1;
                        a = (bc >> 8);
                        a = a & 0xF;
                        a += 0xD0;
                        bc = (bc & 0xFF) + (a * 0x100);
                        a = FF9A;
                        f = 0x40;
                        if (a == (de & 0xFF))
                            f += 0x80;
                        if ((a >> 3) == 0)
                            f += 0x20;
                        if (a < (de & 0xFF))
                            f += 0x10;
                    }
                    else
                    {
                        a = ROM[position];
                        position += 1;
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
                            a = ROM[position];
                            TempPos += (((a >> 4) + 0xD0) * 0x100);
                            position++;
                            a = (a & 0xF) + 3;
                            f = 0;
                            FF9C = a;
                            do
                            {
                                f += 0xA0;
                                if (TempPos >= 0xD09E)
                                    a = Final[TempPos - 0xD09E];
                                else
                                    a = 0;
                                Final.Add((byte)a);
                                a = 0xC0;
                                a = a & 0x3;
                                f = 0x40;
                                if (a == 3)
                                    f += 0x80;
                                if (a < 3)
                                    f += 0x10;
                                if (((a >> 3) & 0x1) == 1)
                                    f += 0x20;
                                if (f >= 0x80)
                                {

                                }
                                TempPos++;
                                de++;
                                bc++;
                                a = TempPos >> 8;
                                a = a & 0xF;
                                a += 0xD0;
                                TempPos = (TempPos & 0xFF) + (a * 0x100);
                                a = bc >> 8;
                                a = a & 0xF;
                                a += 0xD0;
                                bc = (bc & 0xFF) + (a * 0x100);
                                a = FF9A;
                                f = 0;
                                if (a < (de & 0xFF))
                                    f = 0x10;
                                a = FF9C;
                                f = f & 0x10;
                                if ((a >> 3) == 1) { }
                                else
                                    f += 0x20;
                                f += 0x40;
                                if (a - 1 == 0)
                                    f += 0x80;
                                FF9C = a - 1;
                            } while (f < 0x80);
                            f = 0;
                            break;
                        }
                        a = ROM[position];
                        Final.Add((byte)a);
                        position += 1;
                        de += 1;
                        bc += 1;
                        a = (bc >> 8);
                        f = 0x20;
                        a = a & 0xF;
                        a += 0xD0;
                        bc = (bc & 0xFF) + (a * 0x100);
                        a = FF9A;
                        f = 0x40;
                        if (a == (de & 0xFF))
                            f += 0x80;
                        if ((a >> 3) == 0)
                            f += 0x20;
                        if (a < (de & 0xFF))
                            f += 0x10;
                    }
                } while ((f < 0x80));
                a = FF9B;
                f = 0x40;
                if (a == ((de >> 8) & 0xFF))
                    f += 0x80;
                if ((a >> 3) == 0)
                    f += 0x20;
                if (a < ((de >> 8) & 0xFF))
                    f += 0x10;
                if (f >= 0x80)
                {

                }
            }
            File.WriteAllBytes("TestFont.bin", Final.ToArray());
        }
    }
}
