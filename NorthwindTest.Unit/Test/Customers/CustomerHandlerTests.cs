using Application.Contracts;
using Application.Customers.Commands.Create;
using CustomException.Exceptions;
using DomainModel.Entities;
using Moq;
using NorthwindTest.Unit.CommandBuilder;
using Xunit;

namespace NorthwindTest.Unit.Test.Customers
{
    public class CustomerHandlerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepository;
        private readonly CreateCustomerCommandHandler _createCommandHandler;
        public CustomerHandlerTests()
        {
            _customerRepository = new Mock<ICustomerRepository>();
            _createCommandHandler = new CreateCustomerCommandHandler(_customerRepository.Object);
        }
        [Fact]
        public void CreateCustomer_CheckForDuplicatePhoneNumber_ThorwsForbiddenException()
        {
            var command = new CustomerCommandBuilder().BuildAsCreateCommand();
            _customerRepository.Setup(i => i.DoesPhoneNumberExist(command.Phone)).Returns(true);
            void result() => _createCommandHandler.Handle(command);

            Assert.Throws<ForbiddenException>(result);
        }

        [Fact]
        public void CreateCustomer_CheckForInvalidCustomer_ThrowsNotAcceptableException()
        {
            var command = new CustomerCommandBuilder().WithCompanyName("").BuildAsCreateCommand();
            void result() => _createCommandHandler.Handle(command);
            Assert.Throws<NotAcceptableException>(result);
        }

        [Fact]
        public void CreateCustomer_CheckForWorkingWell_OneCustomerShouldBeCreated()
        {
            var command = new CustomerCommandBuilder().BuildAsCreateCommand();
            var result = _createCommandHandler.Handle(command);

            _customerRepository.Verify(i => i.Add(It.IsAny<Customer>()), Times.Once());
        }
        // 
        
    }
}
