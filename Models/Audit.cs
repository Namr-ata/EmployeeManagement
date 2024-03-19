namespace EmployeeManagement.Models
{
    public class Audit
    {
        public int AuditID { get; set; }
        public int EmployeeID { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
        public string ChangeType { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangedBy { get; set; }
    }
}
