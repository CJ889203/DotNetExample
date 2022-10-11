using System.Collections;

namespace Sample_IEnumerable
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Console.WriteLine(list.Count);
        }
    }

    internal class Students : IEnumerable<Student>, IEnumerator<Student>
    {
        List<Student> ls = new List<Student>() { };
        int index = -1;
        public Students()
        {
        }

        public Student Current => ls[index];

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Student> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            index++;
            return index <= ls.Count - 1 ? true : false;
        }

        public void Reset()
        {
            index = -1;
        }

        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    internal class Student
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }
}