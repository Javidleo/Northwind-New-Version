using CommandHandling.Abstractions;
using CustomException.Exceptions;

namespace Application.Customers.Commands.Create
{
    public record CreateCustomerCommand(string id, string Address, string City, string CompanyName, string ContactName,
                                        string ContactTitle, string Country, string Fax, string Phone, string PostalCard,
                                        string Region) : Acommand(0)
    {

        public static CreateCustomerCommand Create(string id, string Address, string City, string CompanyName, string ContactName,
                                        string ContactTitle, string Country, string Fax, string Phone, string PostalCard,
                                        string Region)

        {
            var validator = new CreateCustomerCommandValidator();

            var createCustomerCommand = new CreateCustomerCommand(id, Address, City, CompanyName, ContactName, ContactTitle,
                                                                    Country, Fax, Phone, PostalCard, Region);

            if (validator.Validate(createCustomerCommand).IsValid is false)
                throw new NotAcceptableException("invalid customer data");

            return createCustomerCommand;
        }

    }
}
