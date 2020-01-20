using System;
using ShellScript;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Shell shell = new Shell();
            shell.DoString(File.ReadAllText("test.shs"));
            Console.ReadLine();
        }
    }
}
