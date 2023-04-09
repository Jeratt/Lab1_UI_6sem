using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLibrary
{
    public class RawData
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public int NodeCnt { get; set; }
        public bool IsUnifgorm { get; set; }
        public FRaw Func { get; set; }
        public double[] Grid { get; set; }
        public double[] Field { get; set; }
        public RawData(double left, double right, int nodeCnt, bool isUnifgorm, FRaw func)
        {
            Left = left;
            Right = right;
            NodeCnt = nodeCnt;
            IsUnifgorm = isUnifgorm;
            Func = func;
            Grid = new double[nodeCnt];
            Field = new double[nodeCnt];

            Grid[0] = left;
            Grid[NodeCnt - 1] = right;

            if (!isUnifgorm)
            {
                Random rnd = new Random();
                for (int i = 1; i < nodeCnt - 1; ++i)
                {
                    Grid[i] = Left + rnd.NextDouble() * (Right - Left);
                }
                Array.Sort(Grid);
            }
            else
            {
                for(int i = 1; i < nodeCnt - 1; ++i)
                {
                    Grid[i] = Left + i * (Right - Left) / NodeCnt;
                }
            }

            for(int i = 0; i < nodeCnt; ++i)
            {
                Field[i] = func(Grid[i]);
            }
        }
        public RawData(string filename)
        {
            try
            {
                Load(filename, out RawData rawData);
                Left = rawData.Left;
                Right = rawData.Right;
                NodeCnt = rawData.NodeCnt;
                IsUnifgorm = rawData.IsUnifgorm;
                Func = rawData.Func;
                Grid = rawData.Grid;
                Field = rawData.Field;
            }
            catch(Exception x) {
                throw;
            }
            //Load("C:\\Users\\gera9\\OneDrive\\Рабочий стол\\exp3", out RawData rawData);
        }
        public void Save(string filename)
        {
            FileStream fs = null;
            StreamWriter writer = null;
            try
            {
                fs = File.Create(filename);
                writer = new StreamWriter(fs);
                writer.WriteLine(Left.ToString());
                writer.WriteLine(Right.ToString());
                writer.WriteLine(NodeCnt.ToString());
                writer.WriteLine(IsUnifgorm.ToString());
                /*
                switch (Func)
                {
                    case FRawEnum.FRawLinear:
                        writer.WriteLine("FRawLinear");
                        break;
                    case FRawEnum.FRawCubic:
                        writer.WriteLine("FRawCubic");
                        break;
                    case FRawEnum.FRawRandom:
                        writer.WriteLine("FRawRandom");
                        break;
                    default:
                        throw new Exception();
                }*/
                for (int i = 0; i < NodeCnt; ++i)
                {
                    writer.WriteLine(Grid[i].ToString());
                }
                for (int i = 0; i < NodeCnt; ++i)
                {
                    writer.WriteLine(Field[i].ToString());
                }
                return;
            }
            catch (Exception x)
            {
                Console.WriteLine($"ERROR SAVING FILE ! ! !: {x}");
                throw;
                //return;
            }
            finally
            {
                writer?.Dispose();
                fs?.Close();
            }
        }
        public static void Load(string filename, out RawData rawData) 
        {
            FileStream fs = null;
            StreamReader reader = null;
            try
            {
                double left, right;
                int node_cnt;
                bool is_uniform;
                //FRawEnum f;
                //string f_name;
                fs = File.OpenRead(filename);
                reader = new StreamReader(fs);
                left = double.Parse(reader.ReadLine());
                right = double.Parse(reader.ReadLine());
                node_cnt = Convert.ToInt32(reader.ReadLine());
                is_uniform = Convert.ToBoolean(reader.ReadLine());
                /*
                f_name = reader.ReadLine();
                switch (f_name)
                {
                    case "FRawLinear":
                        f = FRawEnum.FRawLinear; 
                        break;
                    case "FRawCubic":
                        f = FRawEnum.FRawCubic;
                        break;
                    case "FRawRandom":
                        f = FRawEnum.FRawRandom;
                        break;
                    default:
                        throw new Exception();
                }
                */

                rawData = new RawData(left, right, node_cnt, is_uniform, FRawLinear);
                for(int i = 0; i < node_cnt; ++i)
                {
                    rawData.Grid[i] = double.Parse(reader.ReadLine());
                }
                for (int i = 0; i < node_cnt; ++i)
                {
                    rawData.Field[i] = double.Parse(reader.ReadLine());
                }
                return;
            }
            catch (Exception x)
            {
                Console.WriteLine($"ERROR LOADING FILE ! ! !: {x}");
                //rawData = new RawData(0f, 0f, 0, true, FRawLinear);
                throw;
                //return;
            }
            finally
            {
                reader?.Dispose();
                fs?.Close();
            }
        }
        public static double FRawLinear(double x)
        {
            return x * 2 + 5;
        }
        public static double FRawCubic(double x)
        {
            return x * x * x + 3;
        }
        public static double FRawRandom(double x)
        {
            Random rand = new Random();
            return rand.NextDouble();
        }
    }
}
