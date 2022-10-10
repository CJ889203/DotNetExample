using System.Reflection;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);

            DirectoryInfo directoryInfo1 = directoryInfo.CreateSubdirectory("Test2/Test3");
            Console.WriteLine(directoryInfo1.Name);
            Console.WriteLine(directoryInfo1.FullName);
            Console.Read();
        }
    }
}