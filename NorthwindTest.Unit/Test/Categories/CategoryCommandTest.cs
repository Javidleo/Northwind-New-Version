using CustomException.Exceptions;
using FluentAssertions;
using NorthwindTest.Unit.CommandBuilder;
using Xunit;

namespace NorthwindTest.Unit.Test.Categories
{
    public class CategoryCommandTest
    {
        private readonly CategoryCommandBuilder _builder;
        public CategoryCommandTest()
        => _builder = new CategoryCommandBuilder();
        [Fact]
        public void UpsertCommand_CheckForNullName_ThrowsNotAcceptableException()
        {
            void command() => _builder.WithName("").BuildAsUpsertCommand();
            Assert.Throws<NotAcceptableException>(command);
        }

        [Fact]
        public void UpsertCommand_CheckForWorkingWell_CreateCommandSuccessfuly()
        {
            var command = _builder.WithName("javid").BuildAsUpsertCommand();
            command.Name.Should().Be("javid");
        }
    }
}
