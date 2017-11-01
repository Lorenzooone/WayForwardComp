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
            int i = 0;
            while ((i != 49) && (i != 53)) //1 or 5
            {
                Console.WriteLine("The ROM must be named ShantaePure.gbc\nThe file to be compressed from/decompressed into is named TestFont.bin\nPress 1 to Compress, 5 to Decompress");
                i=Console.Read();
            }
            if (i == 49)
                i = 1;
            else
                i = 5;
            byte[] ROM = File.ReadAllBytes("ShantaePure.gbc");
            if(i==5)
            Decompress(ROM);
            else
            Compress(ROM);
            return i;
        }
        static byte InvertByte(byte Result)
        {
            return (byte)(((Result & 1) << 7) + (((Result >> 1) & 1) << 6) + (((Result >> 2) & 1) << 5) + (((Result >> 3) & 1) << 4) + (((Result >> 4) & 1) << 3) + (((Result >> 5) & 1) << 2) + (((Result >> 6) & 1) << 1) + ((Result >> 7) & 1));
        }
        static byte[] search(byte[] Decompressed, ref int position)
        {
            int TempPos = position, length=0, BestPos=0, StartPos=0xFEE;
            List<byte> Result = new List<byte>();
            Result.Add(0);
            for (int Repetition = 8; (Repetition > 0)&&(position<Decompressed.Count()); Repetition--)
            {
                for (int i = 1; (i < 0x1000) && (i < position+1); i++)
                {
                    int k;
                    for (k = 0; (k < 0x12) && (Decompressed[position - i + k] == Decompressed[position + k]); k++)
                        if (position + k == Decompressed.Length - 1)
                        {
                            k++;
                            break;
                        }
                    if (length < k)
                    {
                        length = k;
                        BestPos = position - i;
                        if (length == 0x12) //We reached the maximum
                            break;
                    }
                }
                if (length < 3)
                {
                    Result[0] = (byte)((Result[0] << 1) + 1);
                    Result.Add(Decompressed[position++]);
                }
                else
                {
                    Result[0] = (byte)(Result[0] << 1);
                    BestPos += StartPos;
                    BestPos = (BestPos & 0xFF) + (((BestPos >> 8) & 0xF) * 0x100); //Reset BestPos
                    Result.Add((byte)(BestPos & 0xFF));
                    Result.Add((byte)(((BestPos >> 8)<<4)+length-3));
                    position += length;
                }
                length = 0;
            }
            Result[0] = InvertByte(Result[0]);
            return Result.ToArray();
        }
        static void Compress(byte[] ROM)
        {
            byte[] SubROM = ROM;
            byte[] Decompressed = File.ReadAllBytes("TestFont.bin");
            List<byte> Compressed = new List<byte>();
            int position = 0;
            while (position < Decompressed.Length)
            {
                Compressed.AddRange(search(Decompressed, ref position));
            }
            File.WriteAllBytes("TestFont_C.bin", Compressed.ToArray());
            for (int i = 0; i < Compressed.Count; i++)
                SubROM[i + 0x3964A3] = Compressed[i];
            File.WriteAllBytes("ShantaeComp.gbc", SubROM);
        }
        static void Decompress(byte[] ROM)
        {
            List<byte> Final = new List<byte>();
            byte[] Temp = new byte[0x1000];
            int position = 0x3964A3, de = 0x8000, a, FF92 = 0, FF93 = 1, FF9A = 0, FF9B = 0x98, Repetition = 0, TempPos = 0, bc=0xFEE;
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
                            if (FF9A == (de & 0xFF)) //End of File
                                if (FF9B == ((de >> 8) & 0xFF))
                                    break;
                        } while (--Repetition != 0);
                    }
                    else
                    {
                        Temp[bc] = ROM[position++];
                        Final.Add(Temp[bc]);
                        bc++;
                        de++;
                        bc = (bc & 0xFF) + (((bc >> 8) & 0xF) * 0x100); //Reset bc
                        if (FF9A == (de & 0xFF)) //End of File
                            if (FF9B == ((de >> 8) & 0xFF))
                                break;
                    }
                } while (FF9A != (de & 0xFF));
            }
            File.WriteAllBytes("TestFont.bin", Final.ToArray());
        }
    }
}