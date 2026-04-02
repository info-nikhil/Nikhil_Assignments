namespace MVC_Example.Models
{
    public class Employee
    {
        public int EmployeeID { set; get; }
        public string? EmpName { set; get; }
        public int Salary { set; get; }
        public string? ImageUrl { set; get; }

        // FK + Reference

        public int DeptID { get; set; }
        public Department? Dept { set; get; }
    }
}
