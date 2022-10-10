namespace Sample_Linq
{
    internal class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public int Age { get; set; }
        public bool Gender { get; set; } = default(Boolean);
        public double Salary { get; set; }

        public override string ToString()
        {
            return $"id={Id}, Name={Name}, Age={Age}, Gender={Gender}, Salary={Salary}";
        }
    }
}