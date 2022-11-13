using Application.Common.Exceptions;
using Application.Contracts;
using Application.Services.CategoryServices.Commands.Delete;
using Application.Services.CategoryServices.Commands.Upsert;
using DomainModel.Entities;
using Moq;
using NorthwindTest.Unit.CommandBuilder;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NorthwindTest.Unit.Test
{
    public class CategoryTests
    {
        private readonly UpsertCategoryCommandHandler _upsertHandler;
        private readonly DeletecategorycommandHandler _deleteHandler;
        private readonly Mock<ICategoryRepository> _categoryRepository;
        public CategoryTests()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _upsertHandler = new UpsertCategoryCommandHandler(_categoryRepository.Object);
            _deleteHandler = new DeletecategorycommandHandler(_categoryRepository.Object);
        }

        // //////////////////////////////////////       Upsert      //////////////////////////////////////////
        [Fact]
        public async void UpsertCategory_CheckForDuplicateName_ThrowsConflictException()
        {
            var command = new CategoryCommandBuilder().BuildAsUpsertCommand();
            _categoryRepository.Setup(i => i.DoesExist(i => i.CategoryName == command.name)).Returns(true);

            Func<Task> action = async () => await _upsertHandler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<ConflictException>(action);
        }

        [Fact]
        public async void UpsertCategory_CreateSituation_CheckForWorkingWell()
        {
            var command = new CategoryCommandBuilder().BuildAsUpsertCommand();
            var result = await _upsertHandler.Handle(command, CancellationToken.None);

            _categoryRepository.Verify(i => i.Add(It.IsAny<Category>()), Times.Once());
        }

        [Fact]
        public async void UpsertCategory_UpdateSituation_CheckForInvalidId_ThrowsNotFoundException()
        {
            var command = new CategoryCommandBuilder().WithId(1).BuildAsUpsertCommand();
            Func<Task> action = async () => await _upsertHandler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        [Fact]
        public async void UpsertCategory_UpdateSituation_CheckForWorkingWell()
        {
            var command = new CategoryCommandBuilder().WithId(1).BuildAsUpsertCommand();
            _categoryRepository.Setup(i => i.FindAsync(command.id.Value)).ReturnsAsync(Category.Create("name", "desc", new byte[3] { 1, 2, 3 }));

            var result = await _upsertHandler.Handle(command, CancellationToken.None);

            _categoryRepository.Verify(i => i.Update(It.IsAny<Category>()), Times.Once());
        }

        // /////////////////////////////////////////       Delete      ////////////////////////////////////////////////
        [Fact]
        public async void DeleteCategory_CheckForInvalidId_ThrowsNotFoundException()
        {
            var command = new CategoryCommandBuilder().BuildAsDeleteCommand();

            Func<Task> action = async () => await _deleteHandler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        [Fact]
        public async void DeleteCategory_CheckForChildCategories_ThrowsNotAcceptableExcetion()
        {
            var command = new CategoryCommandBuilder().BuildAsDeleteCommand();
            _categoryRepository.Setup(i => i.FindAsync(command.Id)).ReturnsAsync(Category.Create("name", "description", new byte[3] { 1, 2, 3 }));
            _categoryRepository.Setup(i => i.HasChildren(command.Id)).Returns(true);

            Func<Task> action = async () => await _deleteHandler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<NotAcceptableException>(action);
        }

        [Fact]
        public async void DeleteCategory_CheckForWorkingWell()
        {
            var command = new CategoryCommandBuilder().BuildAsDeleteCommand();
            _categoryRepository.Setup(i => i.FindAsync(command.Id)).ReturnsAsync(Category.Create("name", "description", new byte[3] { 1, 2, 3 }));

            var result = await _deleteHandler.Handle(command, CancellationToken.None);

            _categoryRepository.Verify(i => i.Delete(It.IsAny<Category>()), Times.Once());
        }
    }
}
