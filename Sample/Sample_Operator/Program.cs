namespace Sample_Operator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region ?: 三目运算符
            string a = "非空";
            Console.WriteLine(a != null ? a : "空");
            Console.WriteLine(a == null ? "空" : a);
            #endregion

            #region ??= null合并运算符,不为空则为当前值,否则返回指定值;
            string? n = null;
            Console.WriteLine(n ??= "空");

            int? a1 = null;
            Console.WriteLine(a1 ??= 0);
            #endregion 
        }
    }
}