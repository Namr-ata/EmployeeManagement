using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeManagement.Controllers
{
    public class AuditController : Controller
    {
        private readonly string connectionString;
        public AuditController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MVCConnectionString");
        }

        public IActionResult Index()
        {
            List<Audit> auditRecords = GetAuditrecords();
            return View(auditRecords);
        }
        private List<Audit> GetAuditrecords()
        {
            List<Audit> auditRecords = new List<Audit>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM EmployeeAudit";
                SqlCommand command = new SqlCommand(query, connection);
               
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        auditRecords.Add(new Audit
                        {
                            AuditID = (int)reader["AuditID"],
                            EmployeeID = (int)reader["EmployeeID"],
                            OldName = reader["OldName"].ToString(),
                            NewName = reader["NewName"].ToString(),
                            ChangeType = reader["ChangeType"].ToString(),
                            ChangeDate = (DateTime)reader["ChangeDate"],
                            ChangedBy = reader["ChangedBy"].ToString()
                        });
                    }
                }
            }

            return auditRecords;
        }

    }
}
