using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string connectionString;
        public EmployeeController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MVCConnectionString");
        }

        public ActionResult Index()
        {
            List<Employee> employees = GetEmployees();
            return View(employees);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Employee employee)
        {
            AddEmployee(employee);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound(); // Or handle accordingly
            }

            return View(employee);
        }
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            UpdateEmployee(employee);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            DeleteEmployee(Id);
            return RedirectToAction("Index");
        }

        private List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //string query = "SELECT * FROM Employee";
                //SqlCommand command = new SqlCommand(query, connection);
                SqlCommand command = new SqlCommand("GetEmployees", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Address = reader["Address"].ToString(),
                            Salary = Convert.ToDecimal(reader["Salary"]),
                            Department = reader["Department"].ToString()
                        });
                    }
                }
            }

            return employees;
        }

        private void AddEmployee(Employee employee)
        {
            // Implement logic to add employee to database using ADO.NET
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Employee (Name,Address, Salary,Department) VALUES (@Name, @Address, @Salary, @Department)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", employee.Name);
                command.Parameters.AddWithValue("@Address", employee.Address);
                command.Parameters.AddWithValue("@Department", employee.Department);
                command.Parameters.AddWithValue("@Salary", employee.Salary);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private Employee GetEmployeeById(int id)
        {
            Employee employee = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //string query = "SELECT Id, Name, Address, Salary, Department FROM Employee WHERE Id = @Id";
               //SqlCommand command = new SqlCommand(query, connection);
                SqlCommand command = new SqlCommand("GetEmployeeById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
               

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Address = reader["Address"].ToString(),
                            Salary = Convert.ToDecimal(reader["Salary"]),
                            Department = reader["Department"].ToString()
                        };
                    }
                }
            }

            return employee;
        }
        private void UpdateEmployee(Employee employee)
        {
            // Implement logic to update employee in database using ADO.NET
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // string query = "UPDATE Employee SET Name = @Name, Department = @Department,Address=@Address,Salary = @Salary WHERE Id = @Id";
                //SqlCommand command = new SqlCommand(query, connection);
                SqlCommand command = new SqlCommand("UpdateEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", employee.Name);
                command.Parameters.AddWithValue("@Department", employee.Department);
                command.Parameters.AddWithValue("@Address", employee.Address);
                command.Parameters.AddWithValue("@Salary", employee.Salary);
                command.Parameters.AddWithValue("Id", employee.Id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        private void DeleteEmployee(int Id)
        {
            // Implement logic to delete employee from database using ADO.NET
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //string query = "DELETE FROM Employee WHERE Id = @Id";
                //SqlCommand command = new SqlCommand(query, connection);
                SqlCommand command = new SqlCommand("DeleteEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", Id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
