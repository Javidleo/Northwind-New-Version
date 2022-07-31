using Application.Customers.Commands.Create;

namespace NorthwindTest.Unit.CommandBuilder
{
    public class CustomerCommandBuilder
    {
        private string Id = "43252"; // should not be empty
        private string Address = "fldskfjsdlkfdsjf";
        private string City = "lfkdsfjd";
        private string CompanyName = "negso123"; // should not be empty;
        private string ContactName = "contact 1";
        private string ContactTitle = "Title ";
        private string Country = "iran";
        private string Fax = "fd2lkj3l4kj23";
        private string Phone = "4350u350";
        private string PostalCode = " 4234knfl2k4"; // should not be empty
        private string Region = "4321';'";

        public CreateCustomerCommand BuildAsCreateCommand()
        => CreateCustomerCommand.Create(Id, Address, City, CompanyName, ContactName, ContactTitle, Country,
            Fax, Phone, PostalCode, Region);

        public CustomerCommandBuilder WithCompanyName(string companyName)
        {
            CompanyName = companyName;
            return this;
        }
    }
}