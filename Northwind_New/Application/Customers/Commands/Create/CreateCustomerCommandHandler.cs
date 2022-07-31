using Application.Contracts;
using CommandHandling.Abstractions;
using CustomException.Exceptions;
using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customers.Commands.Create
{
    public class CreateCustomerCommandHandler : IHandleCommand<CreateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly CreateCustomerCommandValidator _validator;
        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _validator = new CreateCustomerCommandValidator();
        }

        public Task Handle(CreateCustomerCommand command)
        {
            

            if (_customerRepository.DoesPhoneNumberExist(command.Phone))
                throw new ForbiddenException("duplicate phone number");

            var customer = Customer.Create(command.id, command.Address, command.City, command.CompanyName, command.ContactName,
                                            command.ContactTitle, command.Country, command.Fax, command.Phone, command.PostalCard);


            _customerRepository.Add(customer);
            return Task.CompletedTask;
        }
    }
}
