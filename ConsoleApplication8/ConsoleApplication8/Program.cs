using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication8
{
    class Comp
    {
        public int Size;
        public int Location;
        public string path;
    }
    class Program
    {
        static int Main()
        {
            int i = 0; //0x41E9 VRAM Loading 0x540 and 0x1366 OAM loading? Should work next on those to get the last files
            List<Comp> Data = new List<Comp>();
            string[] list=File.ReadAllLines("List.txt");
            int Entries = StringToInt(list[0]);
            string FileName = list[1];
            for(int k=0; k<Entries; k++)
            {
                Comp Temp = new Comp();
                Temp.path = list[3 + (k * 4)];
                Temp.Location = StringToInt(list[4 + (k * 4)]);
                Temp.Size = StringToInt(list[5 + (k * 4)]);
                Data.Add(Temp);
            }
            while ((i != 49) && (i != 53)) //1 or 5
            {
                Console.WriteLine("Tool from Lorenzooone - Lorenzo Carletti, 2017\nThe ROM must be named the same as the first line in list.txt and it must be in the same folder\nLook at the source code on Github to see what are the files names\nPress 1 to Compress, 5 to Decompress");
                i=Console.Read();
            }
            if (i == 49)
                i = 1;
            else
                i = 5;
            byte[] ROM = File.ReadAllBytes(FileName);
            if(i==5)
                for(i=0; i<Entries; i++)
                    Decompress(ROM, Data[i]);
            else
                for(i=0; i<Entries; i++)
                    Compress(ROM, Data[i], FileName);
            return i;
        }
        static int CharToInt(char a)
        {
            switch(a)
            {
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                    return a-'A'+0xA;
                default:
                    return a - '0';
            }
        }
        static int StringToInt(string a)
        {
            int k = 0;
            for (int i = 0; i < a.Length; i++)
                k += (CharToInt(a[a.Length-1-i]) << (4 * i));
            return k;
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
            for (int Repetition = 8; (Repetition > 0); Repetition--)
            {
                if (position < Decompressed.Count())
                {
                    for (int i = 1; (i < 0x1000) && (i < position + 1); i++)
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
                        Result.Add((byte)(((BestPos >> 8) << 4) + length - 3));
                        position += length;
                    }
                    length = 0;
                }
                else
                    Result[0] = (byte)(Result[0] << 1);
            }
            Result[0] = InvertByte(Result[0]);
            return Result.ToArray();
        }
        static void Compress(byte[] ROM, Comp Data, string FileName)
        {
            byte[] SubROM = ROM;
            byte[] Decompressed = File.ReadAllBytes(Data.path);
            List<byte> Compressed = new List<byte>();
            int position = 0;
            while (position < Decompressed.Length)
                Compressed.AddRange(search(Decompressed, ref position));
            File.WriteAllBytes(Data.path.Remove(Data.path.Count()-4)+"_C.bin", Compressed.ToArray());
            for (int i = 0; i < Compressed.Count; i++)
                SubROM[i + Data.Location] = Compressed[i];
            File.WriteAllBytes(FileName, SubROM);
        }
        static void Decompress(byte[] ROM, Comp Data)
        {
            List<byte> Final = new List<byte>();
            byte[] Temp = new byte[0x1000];
            int  a, FF92 = 0, FF93 = 1, Repetition = 0, TempPos = 0, bc=0xFEE, position=Data.Location;
            while (Final.Count<Data.Size)
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
                        bc = (bc & 0xFF) + (((bc >> 8) & 0xF) * 0x100); //Reset bc
                    }
            }
            File.WriteAllBytes(Data.path, Final.ToArray());
        }
    }
}