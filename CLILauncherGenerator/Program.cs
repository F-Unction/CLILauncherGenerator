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
            Console.Write("RedirectWorkDir > ");
            var redirect = Console.ReadLine();

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
                        {   "+
                        (
                            redirect=="y" ? 
                            "myProcess.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();":
                            "myProcess.StartInfo.WorkingDirectory = @\""+filename.Substring(0,filename.LastIndexOf("\\"))+"\";"
                        )
                            +@"
                            myProcess.StartInfo.UseShellExecute = false;
                            myProcess.StartInfo.Arguments = argString;
                            myProcess.StartInfo.FileName = @""" + filename + @""";
                            myProcess.Start();
                        }
                    }
                }
                ";

            Console.WriteLine(code);
            Console.ReadKey();
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
