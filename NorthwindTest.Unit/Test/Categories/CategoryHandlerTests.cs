using Application.Categories.Commands.Delete;
using Application.Categories.Commands.Upsert;
using Application.Contracts;
using CustomException.Exceptions;
using DomainModel.Entities;
using Moq;
using NorthwindTest.Unit.CommandBuilder;
using System.Threading.Tasks;
using Xunit;

namespace NorthwindTest.Unit.Test.Categories
{
    public class CategoryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly UpsertCategoryCommandHandler _upsertCommandHandler;
        private readonly DeleteCategoryCommandHandler _deleteCommandHandler;
        public CategoryHandlerTests()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _upsertCommandHandler = new UpsertCategoryCommandHandler(_categoryRepository.Object);
            _deleteCommandHandler = new DeleteCategoryCommandHandler(_categoryRepository.Object);
        }

        [Fact]
        public void UpsertCategory_CheckForEmptyCategoryId_InsertingNewCastegory()
        {
            var command = new CategoryCommandBuilder().BuildAsUpsertCommand();

            var result = _upsertCommandHandler.Handle(command);
            _categoryRepository.Verify(i => i.Add(It.IsAny<Category>()), Times.Once());
        }

        [Fact]
        public void UpsertCategory_CheckForInvalidCategoryId_ThrowNotFoundException()
        {
            var command = new CategoryCommandBuilder().WithId(2).BuildAsUpsertCommand();

            void result() =>  _upsertCommandHandler.Handle(command);
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void UpsertCategory_CheckForUpdatingCategory_OneCategoryShouldBeUpdated()
        {
            var command = new CategoryCommandBuilder().WithId(2).BuildAsUpsertCommand();
            _categoryRepository.Setup(i => i.Find(command.id.Value))
                .Returns(Category.Create(command.Name, command.Description, command.Picture));

            _upsertCommandHandler.Handle(command);
            _categoryRepository.Verify(i => i.Update(It.IsAny<Category>()), Times.Once());
        }

        // /////////////////////////////////////     Delete      /////////////////////////////////////
        [Fact]
        public void DeleteCategory_CheckForInvalidCategoryId_ThrowsNotFoundException()
        {
            var command = new CategoryCommandBuilder().WithId(1).BuildAsDeleteCommand();
            
            void result() => _deleteCommandHandler.Handle(command);
            Assert.Throws<NotFoundException>(result);
        }

        [Fact]
        public void DeleteCategory_CheckForHavingProducts_ThrowsForbiddenException()
        {
            var command = new CategoryCommandBuilder().WithId(1).BuildAsDeleteCommand();
            _categoryRepository.Setup(i => i.Find(command.id))
                .Returns(Category.Create("name", "desc", new byte[] {1,2,3,4,5}));
            _categoryRepository.Setup(i => i.HasProduct(command.id)).Returns(true);

            void result() => _deleteCommandHandler.Handle(command);
            Assert.Throws<ForbiddenException>(result);
        }

        [Fact]
        public void DeleteCategory_CheckForWorkingWell_DeleteCategory()
        {
            var command = new CategoryCommandBuilder().WithId(1).BuildAsDeleteCommand();
            new CategoryCommandBuilder().WithId(1).BuildAsDeleteCommand();
            _categoryRepository.Setup(i => i.Find(command.id))
                .Returns(Category.Create("name", "desc", new byte[] { 1, 2, 3, 4, 5 }));

            var result = _deleteCommandHandler.Handle(command);
            _categoryRepository.Verify(i => i.Delete(It.IsAny<Category>()), Times.Once());
        }
    }
}
