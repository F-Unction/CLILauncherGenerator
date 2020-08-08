using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;

namespace CLILauncherGenerator
{
    static class MainClass
    {
        public static void Main(string[] args)
        {
            Console.Write("ApplicationName > ");
            var filename = Console.ReadLine();
            Console.Write("OutputName      > ");
            var oname = Console.ReadLine();

            string code = @"using System.Diagnostics;
                class MainClass
                {
                    static void Main(string[] args)
                    {
                        string argString = """";
                        foreach(var Arg in args)
                        {
                            argString += Arg;
                        }
                        using (Process myProcess = new Process())
                        {
                            myProcess.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
                            myProcess.StartInfo.UseShellExecute = false;
                            myProcess.StartInfo.Arguments = argString;
                            myProcess.StartInfo.FileName = """ + filename.Replace("\\", "\\\\") + @""";
                            myProcess.Start();
                        }
                    }
                }
                ";
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameter = new CompilerParameters();

            parameter.ReferencedAssemblies.Add("System.dll");

            parameter.GenerateExecutable = true;
            parameter.OutputAssembly = "C:\\PathApp\\" + oname + ".exe";
            var results = provider.CompileAssemblyFromSource(parameter, code);
            if (results.Errors.HasErrors)
            {
                Console.WriteLine(results.Errors[0].ToString());
                Console.ReadKey();
            }
        }
    }
}
