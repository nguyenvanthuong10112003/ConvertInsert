using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace convert
{
    class MyClass
    {
        public MyClass() { }
        public void input(string url,ref string []datas, ref int n)
        {
            using (StreamReader reader = new StreamReader(url))
            {
                int index = 0;
                while (!string.IsNullOrEmpty(datas[index] = reader.ReadLine()))
                {
                    index++;
                }
                n = index;
            }
        }
        public void output(string url, string []datas, int n)
        {
            using (StreamWriter wr = new StreamWriter(url)) {
                for (int i = 0; i < n; i++)
                     wr.WriteLine(datas[i]);
            }
        }
        public void tach(string rs, string []vls, int col)
        {
            int index = 0;
            for (int i = 0; i < rs.Length - 1; i++)
            {
                if (rs[i] == '\t')
                {
                    vls[index++] = "";
                    continue;
                }
                for (int j = i + 1; j < rs.Length; j++)
                {
                    if (rs[j] == '\t')
                    {
                        vls[index++] = rs.Substring(i, j - i);
                        i = j;
                        break;
                    }
                }
            }
            for (int i = index; i < col; i++)
                vls[i] = "";
        }
        public string toDate(string dt)
        {
            return dt.Substring(6, 4) + "-" + dt.Substring(3, 2) + "-" + dt.Substring(0, 2);
        }
        public string toInsert(string []data, int col, string nameTable, string []type)
        {
            string rs = "INSERT INTO " + nameTable + " VALUES (";
            for (int i = 0; i < col; i++)
            {
                if (type[i] == "number")
                    rs += data[i];
                else if (type[i] == "date")
                    rs += "'" + toDate(data[i]) + "'";
                else
                    rs += "'" + data[i] + "'";
                if (i < col - 1)
                    rs += ", ";
            }
            return rs + ");";
        }
        public void cv(string []datas, ref string []query, string []type, bool []required, ref int n, int numCol, string nameTable)
        {
            string []vl = new string[n];
            int index = 0;
            for (int i = 0; i < n; i++)
            {
                string []rs = new string[numCol];
                tach(datas[i], rs, numCol);
                for (int j = 0; j < numCol; j++)
                {
                    if (required[j] && rs[j].Length == 0)
                        break;
                    else if (j == numCol - 1)
                        query[index++] = toInsert(rs, numCol, nameTable, type);
                }
            }
            n = index;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            int col = 13;
            string nameTable = "LICHHOC";
            string []type = {
                "number",
                "text",
                "textVNI",
                "text",
                "textVNI",
                "number",
                "text",
                "number",
                "text",
                "date",
                "date",
                "text",
                "textVNI"
            };
            bool []required = {
                false,
                false,
                false,
                true,
                false,
                true,
                true,
                false,
                false,
                true,
                true,
                true,
                false
            };
            MyClass obj = new MyClass();
            int n = 0;
            Console.Write("Input path here: ");
            string path = Console.ReadLine();
            string []datas = new string[9999];
            obj.input(path + "\\input.txt", ref datas, ref n);
            if (n > 0)
            {
                string []querys = new string[n];
                obj.cv(datas, ref querys, type, required, ref n, col, nameTable);
                obj.output(path + "\\output.txt", querys, n);
                Console.WriteLine("Success!");
            }
            Console.ReadLine();
        }
    }
}
