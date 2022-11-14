using Application.Common.Mapping;
using AutoMapper;
using DomainModel.Entities;

namespace Application.Services.CustomerService.Queries.GetCustomerList
{
    public class GetCustomerListViewModel : IMapFrom<Customer>
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }

        public void Mapping(Profile profile)
        {

        }
    }
}
