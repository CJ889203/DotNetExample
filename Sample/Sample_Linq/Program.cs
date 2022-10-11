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

            #region OrderBy 用指定的键排序,默认升序

            #region 使用内部数据项进行排序

            Console.WriteLine("".PadLeft(60, '-'));
            // 对象用内部数据排序
            Console.WriteLine("=".PadLeft(20, '=') + "OrderBy".PadRight(40, '='));
            IOrderedEnumerable<Employee> employees = list.OrderBy(e => e.Age);
            foreach (var item in employees)
            {
                Console.WriteLine(item);
            }
            #endregion

            #region 使用经过处理的内部数据项进行排序

            Console.WriteLine("".PadLeft(60, '-'));
            IOrderedEnumerable<Employee> employees3 = list.OrderBy(e => e.Name[e.Name.Length - 1]);
            foreach (var item in employees3)
            {
                Console.WriteLine(item);
            }

            #endregion

            #region 使用自己排序
            // 数组用自己排序
            int[] ints = { 5, 3, 2, 1, 4 };
            IOrderedEnumerable<int> orderedEnumerable = ints.OrderBy(i => i);
            foreach (var item in orderedEnumerable)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("".PadLeft(60, '-'));
            #endregion

            #region 引入外部数据进行排序
            // 使用Guid进行随机排序
            IOrderedEnumerable<int> orderedEnumerable1 = ints.OrderBy(i => Guid.NewGuid());
            foreach (var item in orderedEnumerable1)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("".PadLeft(60, '-'));
            // 使用Randon进行随机排序
            Random random = new Random();
            IOrderedEnumerable<Employee> employees2 = list.OrderBy(e => random.Next());
            foreach (var item in employees2)
            {
                Console.WriteLine(item);
            }
            #endregion

            #endregion

            #region OrderByDescending 用指定的键倒序排序

            Console.WriteLine("=".PadLeft(20, '=') + "OrderByDescending".PadRight(40, '='));
            IOrderedEnumerable<Employee> employees1 = list.OrderByDescending(e => e.Age);
            foreach (var item in employees1)
            {
                Console.WriteLine(item);
            }
            #endregion

            #region ThenBy/ThenByDescending 次要的排序规则

            Console.WriteLine("=".PadLeft(20, '=') + "ThenBy".PadRight(40, '='));
            IOrderedEnumerable<Employee> employees4 = list.OrderBy(e => e.Age).ThenBy(e => e.Salary);
            foreach (var item in employees4)
            {
                Console.WriteLine(item);
            }

            #endregion

            #region Skip 跳过n条数据项
            Console.WriteLine("=".PadLeft(20, '=') + "ThenBy".PadRight(40, '='));
            IEnumerable<Employee> enumerable = list.Skip(3);
            foreach (var item in enumerable)
            {
                Console.WriteLine(item);
            }
            #endregion

            #region Take 获取n条数据

            Console.WriteLine("=".PadLeft(20, '=') + "ThenBy".PadRight(40, '='));
            IEnumerable<Employee> enumerable1 = list.Take(3);
            foreach (var item in enumerable1)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("".PadLeft(60, '-'));
            IEnumerable<Employee> enumerable2 = list.Take(new Range(3, 5));
            foreach (var item in enumerable2)
            {
                Console.WriteLine(item);
            }
            #endregion

            #region Max 获取指定的最大值(不是元素)
            Console.WriteLine("=".PadLeft(20, '=') + "Max".PadRight(40, '='));

            int v = list.Max(e => e.Age);
            Console.WriteLine(v);

            Console.WriteLine("".PadLeft(60, '-'));

            string? v1 = list.Max(e => e.Name); //string 采用的是字符串比较算法
            Console.WriteLine(v1);
            #endregion

            #region Average 获取平均值(不是元素)

            Console.WriteLine("=".PadLeft(20, '=') + "Average".PadRight(40, '='));

            double v2 = list.Average(e => e.Salary);
            Console.WriteLine(v2);

            #endregion

            #region GroupBy 根据指定的条件进行分组

            Console.WriteLine("=".PadLeft(20, '=') + "GroupBy".PadRight(40, '='));
            IEnumerable<IGrouping<int, Employee>> enumerable3 = list.GroupBy(e => e.Age);
            //IEnumerable<IGrouping<int, Employee>> enumerable3 = list.GroupBy(e=> { return 3; });// 可以从外部引入分组条件
            foreach (IGrouping<int, Employee> item in enumerable3)
            {
                Console.WriteLine($"按照{item.Key}进行分组: ");
                Console.WriteLine("  当组最大工资为: " + item.Max(i => i.Salary));
                foreach (Employee item1 in item)
                {
                    Console.WriteLine("    元素为: " + item1);
                }
            }

            #endregion

            #region 投影运算

            #region Select 投影操作,将转为另一个类型

            // 投影内部数据
            Console.WriteLine("=".PadLeft(20, '=') + "Select".PadRight(40, '='));

            IEnumerable<int> enumerable4 = list.Select(e => e.Age);
            foreach (var item in enumerable4)
            {
                Console.WriteLine(item);
            }

            // 投影为指定类型
            Console.WriteLine("".PadLeft(60, '-'));

            IEnumerable<Dog> enumerable6 = list.Select(e => new Dog { Name = e.Name, Age = e.Age });
            foreach (var item in enumerable6)
            {
                Console.WriteLine(item);
            }

            // 投影为匿名类型
            Console.WriteLine("".PadLeft(60, '-'));

            var enumerable5 = list.Select(e => new
            {
                e.Name,
                e.Age,
            });
            foreach (var item in enumerable5)
            {
                Console.WriteLine(item);
            }

            #endregion

            #region SelectMany

            #endregion

            #region Zip 引入另一个序列同当前序列进行运算

            int[] numbers = { 1, 2, 3, 4 };
            string[] words2 = { "one", "two", "three" };

            var numbersAndWords = numbers.Zip(words2, (first, second) => first + " " + second);

            foreach (var item in numbersAndWords)
                Console.WriteLine(item);

            #endregion

            #endregion

            #region 综合

            Console.WriteLine("=".PadLeft(20, '=') + "综合".PadRight(40, '='));

            var enumerable7 = list.GroupBy(e => e.Age)
                .Select(
                g => new
                {
                    NianLing = g.Key,
                    MaxS = g.Max(e => e.Salary),
                    MinS = g.Min(e => e.Salary),
                    RenShu = g.Count()
                }
                );
            foreach (var e in enumerable7)
            {
                Console.WriteLine(e.NianLing + "," + e.MaxS + "," + e.MinS + "," + e.RenShu);
            }
            #endregion

            #region ToArray/ToList

            IEnumerable<Employee> enumerable8 = list.Where(e => e.Salary > 6000);
            List<Employee> employees5 = enumerable8.ToList();
            Employee[] employees6 = enumerable8.ToArray();

            #endregion

            #region 链式编程案例

            var enumerable9 = list.Where(e => e.Id > 2).GroupBy(e => e.Age).OrderBy(g => g.Key).Take(3)
                .Select(g => new { NL = g.Key, RS = g.Count(), PJ = g.Average(e => e.Salary) });
            foreach (var item in enumerable9)
            {
                Console.WriteLine(item.NL + "," + item.RS + "," + item.PJ);
            }
            #endregion

            #region Set

            // 返回的序列包含输入序列的唯一元素。
            #region Distinct
            string[] planets = { "Mercury", "Venus", "Venus", "Earth", "Mars", "Earth" };

            IEnumerable<string> enumerable10 = planets.Distinct();

            foreach (var str in enumerable10)
            {
                Console.WriteLine(str);
            }

            /* This code produces the following output:
             *
             * Mercury
             * Venus
             * Earth
             * Mars
             */
            #endregion

            #region DistinctBy
            IEnumerable<Employee> enumerable11 = list.DistinctBy(e => e.Age);
            foreach (var item in enumerable11)
            {
                Console.WriteLine(item);
            }
            #endregion

            // 返回的序列只包含位于第一个输入序列但不位于第二个输入序列的元素。
            #region Except
            string[] planets1 = { "Mercury", "Venus", "Earth", "Jupiter" };
            string[] planets2 = { "Mercury", "Earth", "Mars", "Jupiter" };

            //IEnumerable<string> query = from planet in planets1.Except(planets2) select planet;
            IEnumerable<string> query1 = planets1.Except(planets2);
            foreach (var str in query1)
            {
                Console.WriteLine(str);
            }

            /* This code produces the following output:
             *
             * Venus
             */
            #endregion

            #endregion

            #region Ling 解决面试问题

            int i = 5;
            int j = 8;
            int k = 6;
            int[] nums = new int[] { i, j, k };

            #region 求最大值
            nums.Max();// 性能不好

            int max = Math.Max(i, Math.Max(j, k));// 性能更好 
            #endregion

            #region 求逗号分割的字符串的平均值
            string str1 = "61,90,100,99,18,22,38,66,80,93,55,50,89";
            string[] strings = str1.Split(',');
            double v3 = strings.Select(e => Convert.ToDouble(e)).Average();
            Console.WriteLine(v3);
            #endregion

            #endregion
        }
    }
}

internal class Dog
{
    public string? Name { get; set; }
    public int Age { get; set; }
    public override string ToString()
    {
        return $"Dog{{Name={Name}, Age={Age}}}";
    }
}