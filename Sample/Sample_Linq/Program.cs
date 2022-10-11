namespace Sample_Linq
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Employee> list = new List<Employee>();
            list.Add(new Employee() { Id = 1, Name = "jerry", Age = 28, Gender = true, Salary = 5000 });
            list.Add(new Employee() { Id = 2, Name = "jim", Age = 33, Gender = true, Salary = 3000 });
            list.Add(new Employee() { Id = 3, Name = "lily", Age = 35, Gender = false, Salary = 9000 });
            list.Add(new Employee() { Id = 4, Name = "lucy", Age = 16, Gender = false, Salary = 2000 });
            list.Add(new Employee() { Id = 5, Name = "kimi", Age = 25, Gender = true, Salary = 1000 });
            list.Add(new Employee() { Id = 6, Name = "nanacy", Age = 35, Gender = false, Salary = 8000 });
            list.Add(new Employee() { Id = 7, Name = "zack", Age = 35, Gender = true, Salary = 8500 });
            list.Add(new Employee() { Id = 8, Name = "jack", Age = 35, Gender = true, Salary = 8000 });

            #region Where 筛选满足条件的数据
            Console.WriteLine("=".PadLeft(20, '=') + "Where".PadRight(40, '='));
            IEnumerable<Employee> items1 = list.Where(e => e.Age > 20);
            foreach (var e in items1)
            {
                Console.WriteLine(e);
            }
            #endregion

            #region Count 获取满足条件的元素个数
            Console.WriteLine("=".PadLeft(20, '=') + "Count".PadRight(40, '='));
            Console.WriteLine(list.Count());
            Console.WriteLine(list.Count(e => e.Age > 20));
            Console.WriteLine(list.Count(e => e.Age > 20 && e.Salary > 8000));
            #endregion

            #region Any 判断是否有满足条件的元素
            Console.WriteLine("=".PadLeft(20, '=') + "Any".PadRight(40, '='));
            Console.WriteLine(list.Any(e => e.Salary > 30000));
            #endregion

            #region Single 获取唯一的数据,没有或有多条会报错
            Console.WriteLine("=".PadLeft(20, '=') + "Single".PadRight(40, '='));
            try
            {
                //Console.WriteLine(list.Single(e => e.Id == 17));
                Console.WriteLine(list.Single(e => e.Id == 17));// 没有会报错
            }
            catch (Exception)
            {

                Console.WriteLine("没有该数据存在");
            }

            Console.WriteLine(list.Single(e => e.Id == 7));
            #endregion

            #region SingleOrDefault 获取唯一的数据,没有就返回默认值,有多条就报错
            Console.WriteLine("=".PadLeft(20, '=') + "SingleOrDefault".PadRight(40, '='));
            Console.WriteLine(list.SingleOrDefault(e => e.Id == 17) ?? null); // 没有就返回默认值

            Console.WriteLine(list.SingleOrDefault(e => e.Salary == 8500));// 若果存在多条就报错
            try
            {
                Console.WriteLine(list.SingleOrDefault(e => e.Salary == 8000));// 若果存在多条就报错
            }
            catch (Exception)
            {
                Console.WriteLine("存在有多条数据");
            }
            #endregion

            #region First 获取(满足条件的)第一条数据,没有就报错
            Console.WriteLine("=".PadLeft(20, '=') + "First".PadRight(40, '='));
            Console.WriteLine(list.First());
            Console.WriteLine(list.First(e => e.Age > 30));
            #endregion

            #region FirstOrDefault 获取(满足条件的)第一条数据,没有就返回默认值
            Console.WriteLine("=".PadLeft(20, '=') + "FirstOrDefault".PadRight(40, '='));
            Console.WriteLine(list.FirstOrDefault());
            Console.WriteLine(list.FirstOrDefault(e => e.Age > 300) ?? null);
            #endregion

            #region OrderBy 用指定的键排序
            Console.WriteLine("=".PadLeft(20, '=') + "OrderBy".PadRight(40, '='));
            IOrderedEnumerable<Employee> employees = list.OrderBy(e => e.Age);
            foreach (var item in employees)
            {
                Console.WriteLine(item);
            }
            #endregion

            #region OrderByDescending 用指定的键倒序排序
            Console.WriteLine("=".PadLeft(20, '=') + "OrderByDescending".PadRight(40, '='));
            IOrderedEnumerable<Employee> employees1 = list.OrderByDescending(e => e.Age);
            foreach (var item in employees1)
            {
                Console.WriteLine(item);
            }
            #endregion

        }
    }
}