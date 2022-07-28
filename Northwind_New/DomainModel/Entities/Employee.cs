using DomainModel.Common;
using System;
using System.Collections.Generic;

namespace DomainModel.Entities
{
    public class Employee : AuditableEntity
    {
        public Employee()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritory>();
            DirectReports = new HashSet<Employee>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; private set; }
        public string UserId { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public string Title { get; private set; }
        public string TitleOfCourtesy { get; private set; }
        public DateTime? BirthDate { get; private set; }
        public DateTime? HireDate { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string Region { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }
        public string HomePhone { get; private set; }
        public string Extension { get; private set; }
        public byte[] Photo { get; private set; }
        public string Notes { get; private set; }
        public int? ReportsTo { get; private set; }
        public string PhotoPath { get; private set; }

        public Employee Manager { get; private set; }
        public ICollection<EmployeeTerritory> EmployeeTerritories { get; private set; }
        public ICollection<Employee> DirectReports { get; private set; }
        public ICollection<Order> Orders { get; private set; }
    }
}
