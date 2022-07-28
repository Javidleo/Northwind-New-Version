using Application.Categories.Commands;
using Application.Contracts;
using CustomException.Exceptions;
using DomainModel.Entities;
using Moq;
using NorthwindTest.Unit.CommandBuilder;
using System.Threading.Tasks;
using Xunit;

namespace NorthwindTest.Unit.Test.Categories
{
    public class CateogryTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly UpsertCategoryCommandHandler _commandHandler;
        public CateogryTests()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _commandHandler = new UpsertCategoryCommandHandler(_categoryRepository.Object);
        }

        [Fact]
        public void UpsertCategory_CheckForEmptyCategoryId_InsertingNewCastegory()
        {
            var command = new CategoryCommandBuilder().BuildAsUpsertCommand();

            var result = _commandHandler.Handle(command);
            _categoryRepository.Verify(i => i.Add(It.IsAny<Category>()), Times.Once());
        }

        [Fact]
        public void UpsertCategory_CheckForInvalidCategoryId_ThrowNotFoundException()
        {
            var command = new CategoryCommandBuilder().WithId(2).BuildAsUpsertCommand();

            async Task result() => await _commandHandler.Handle(command);
            Assert.ThrowsAsync<NotFoundException>(result);
        }

        [Fact]
        public async void UpsertCategory_CheckForUpdatingCategory_OneCategoryShouldBeUpdated()
        {
            var command = new CategoryCommandBuilder().WithId(2).BuildAsUpsertCommand();
            _categoryRepository.Setup(i => i.Find(2))
                .Returns(Task.FromResult(Category.Create(command.Name, command.Description, command.Picture)));

            await _commandHandler.Handle(command);
            _categoryRepository.Verify(i => i.Update(It.IsAny<Category>()), Times.Once());
        }
    }
}
