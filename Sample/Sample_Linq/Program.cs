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


            IEnumerable<Employee> items1 = list.Where(e => e.Age > 20);
            foreach (var e in items1)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine(list.Count().GetType().Name +": " +list.Count());
            Console.WriteLine( list.Count(e=>e.Age>20));
        }
    }
}