using System;
using System.Collections.Generic;

namespace DomainModel.Entities
{
    public class Customer
    {
        public Customer()
        => Orders = new HashSet<Order>();

        public int Id { get; private set; }
        public Guid Guid { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }    
        public bool IsActive { get; private set; }
        public DateTime? BirthDate { get; private set; }
        public bool? Geneder { get; set; }
        public string UserName { get; private set; }
        public string NormalizedUserName { get;private set; }
        public string Email { get; private set; }
        public string NormalizedEmail { get; private set; }
        public bool EmailConfirmed { get; private set; }
        public string PasswordHash { get; private set; }
        public string SecurityStamp { get; private set; }
        public string ConcurrencyStamp { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool PhoneNumberConfirmed { get; private set; }
        public bool TwoFactorEnabled { get; private set; }
        public DateTimeOffset LockoutEnd { get; private set; }
        public bool LockoutEnabled { get; private set; }
        public int? AccessFailedCount { get; private set; }
        public string CompanyName { get; private set; }
        public string ContactName { get; private set; }
        public string ContactTitle { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string Region { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }
        public string Fax { get; private set; }

        public ICollection<Order> Orders { get; private set; }

        private Customer(int id, string address, string city, string companyName, string contactName,
                                        string contactTitle, string country, string fax, string phone, string postalCard)
        {
            Id = id;
            Address = address;
            CompanyName = CompanyName;
            ContactName = ContactName;
            ContactTitle = ContactTitle;
            Country = country;
            Fax = fax;
            PhoneNumber = phone;
            PostalCode = postalCard;
        }

        public static Customer Create(int id, string address, string city, string companyName, string contactName,
                                        string contactTitle, string country, string fax, string phone, string postalCard)
        => new(id, address, city, companyName, contactName, contactTitle, country, fax, phone, postalCard);
    }
}
