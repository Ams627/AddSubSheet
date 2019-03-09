using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddSubSheet
{
    class Program
    {
        private static string MakeFilename(int sheet)
        {
            return $"addsub-worksheet-{sheet + 1:D3}.html";
        }
        private static void Main(string[] args)
        {
            try
            {
                if (!args.Any())
                {
                    var message = "you must speficiy the number of worksheets to generate.";
                    message += "\r\n" + "For example";
                    message += "\r\n\r\n    " + "AddSubGen 5\r\n\r\ngenerates 5 worksheets.";
                    throw new Exception(message);
                }
                int numberOfWorksheets = 0;
                foreach (var arg in args)
                {
                    if (arg.All(c => char.IsDigit(c)))
                    {
                        numberOfWorksheets = Convert.ToInt32(arg);
                    }
                }

                var batchConverterName = "makepdf.bat";
                using (var batchFileStream = new StreamWriter(batchConverterName))
                {
                    batchFileStream.WriteLine("@echo off");
                    batchFileStream.WriteLine("pskill -nobanner foxitreader >nul");
                    for (var sheet = 0; sheet < numberOfWorksheets; sheet++)
                    {
                        var filename = MakeFilename(sheet);
                        batchFileStream.WriteLine($"wkhtmltopdf --dpi 400 {filename} {Path.ChangeExtension(filename, "pdf")}");
                    }
                }

                const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const int columns = 4;
                const int rows = 18;
                for (int sheet = 0; sheet < numberOfWorksheets; sheet++)
                {
                    var answers = new List<int>();
                    var filename = MakeFilename(sheet);
                    using (var stream = new StreamWriter(filename))
                    {
                        stream.WriteLine("<!doctype html>");
                        stream.WriteLine("<html lang=\"en\">");
                        stream.WriteLine("<head>");
                        stream.WriteLine("<title>Nothing</title>");
                        stream.WriteLine("<style type='text/css'>");
                        stream.WriteLine("body {");
                        stream.WriteLine("font-size:20pt;");
                        stream.WriteLine("}");
                        stream.WriteLine(".sumstable {");
                        stream.WriteLine("text-align:center;");
                        stream.WriteLine("margin:0;");
                        stream.WriteLine("border-collapse:collapse;");
                        stream.WriteLine("}");
                        stream.WriteLine(".sumstable,.sumstable td + td {");
                        stream.WriteLine("padding-left:20px;");
                        stream.WriteLine("padding-top:20px;");
                        stream.WriteLine("padding-right:100px;");
                        stream.WriteLine("padding-bottom:20px;");
                        stream.WriteLine("color:black;");
                        stream.WriteLine("white-space: nowrap;");
                        stream.WriteLine("border:solid 1px #dddddd;");
                        stream.WriteLine("}");
                        stream.WriteLine(".sumstable,.sumstable td {");
                        stream.WriteLine("padding:0px;");
                        stream.WriteLine("padding-right:20px;");
                        stream.WriteLine("color:#dddddd;");
                        stream.WriteLine("white-space: nowrap;");
                        stream.WriteLine("border:solid 1px #dddddd;");
                        stream.WriteLine("}");
                        stream.WriteLine(".answerstable {");
                        stream.WriteLine("border-collapse:collapse;");
                        stream.WriteLine("text-align:center;");
                        stream.WriteLine("}");
                        stream.WriteLine(".answerstable, .answerstable td {");
                        stream.WriteLine("padding-left:20px;");
                        stream.WriteLine("padding-right:20px;");
                        stream.WriteLine("color:black;");
                        stream.WriteLine("white-space: nowrap;");
                        stream.WriteLine("border:solid 1px #dddddd;");
                        stream.WriteLine("}");
                        stream.WriteLine(".answers {");
                        stream.WriteLine("page-break-before: always;");
                        stream.WriteLine("}");
                        stream.WriteLine("</style>");
                        stream.WriteLine("<script>");
                        stream.WriteLine("</script>");
                        stream.WriteLine("</head>");

                        var rnd = new Random();
                        stream.WriteLine($"<table class=\"sumstable\">");

                        var sum = new AddSubSum(rows * columns);
                        var e = sum.GetEnumerable().GetEnumerator();
                        for (int i = 0; i < rows * columns; i++)
                        {
                            if (i % columns == 0)
                            {
                                stream.WriteLine($"<tr>");
                                stream.WriteLine($"<td class=\"col1\">{alphabet[i / columns]}</td>");
                            }

                            e.MoveNext();
                            var s = e.Current;
                            answers.Add(s.answer);

                            if (s.answer < 0)
                            {
                                Console.WriteLine();
                            }

                            stream.WriteLine($"<td>{s.first} {s.op} {s.second} = </td>");
                            if ((i + 1) % columns == 0)
                            {
                                stream.WriteLine($"</tr>");
                            }
                        }
                        stream.WriteLine($"</table>");
                        stream.WriteLine("<div class=\"answers\">");
                        stream.WriteLine("<h1>Answers</h1>");

                        stream.WriteLine($"<table class=\"answerstable\">");
                        for (int i = 0; i < answers.Count; i++)
                        {
                            if (i % columns == 0)
                            {
                                stream.WriteLine($"<tr>");
                                stream.WriteLine($"<td class=\"col1\">{alphabet[i / columns]}</td>");
                            }

                            stream.WriteLine($"<td>{answers[i]}</td>");

                            if ((i + 1) % columns == 0)
                            {
                                stream.WriteLine($"</tr>");
                            }

                        }
                        stream.WriteLine("</div>");
                        stream.WriteLine($"</table>");

                        stream.WriteLine($"</body>");
                        stream.WriteLine($"</html>");
                    }
                }
            }
            catch (Exception ex)
            {
                var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
                var progname = Path.GetFileNameWithoutExtension(fullname);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
