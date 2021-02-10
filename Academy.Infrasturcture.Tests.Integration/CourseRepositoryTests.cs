using Academy.Domain.Tests.Unit.Builders;
using FluentAssertions;
using Xunit;

namespace Academy.Infrastructure.Tests.Integration
{
    public class CourseRepositoryTests : IClassFixture<RealDatabaseFixture>
    {
        private readonly CourseRepository _repository;
        private readonly CourseTestBuilder _courseBuilder;

        public CourseRepositoryTests(RealDatabaseFixture databaseFixture)
        {
            _courseBuilder = new CourseTestBuilder();
            _repository = new CourseRepository(databaseFixture.Context);
        }

        [Fact]
        public void Should_ReturnAllCourses()
        {
            //act
            var courses = _repository.GetAll();

            //assert
            courses.Should().HaveCountGreaterOrEqualTo(3);
        }

        [Fact]
        public void Should_CreateCourse()
        {
            //arrange
            var expected = _courseBuilder.Build();

            //act
            _repository.Create(expected);

            //assert
            var courses = _repository.GetAll();
            courses.Should().Contain(expected);
        }

        [Fact]
        public void Should_ReturnIdOfTheCreatedCourse()
        {
            var course = _courseBuilder.Build();

            var id = _repository.Create(course);

            id.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Should_GetCourseByName()
        {
            //arrange
            const string expectedName = "OnionArchitecture";
            var expected = _courseBuilder.WithName(expectedName).Build();
            _repository.Create(expected);

            //act
            var actual = _repository.GetBy(expectedName);
            
            //assert
            actual.Name.Should().Be(expectedName);
            actual.Tuition.Should().Be(expected.Tuition);
            actual.Instructor.Should().Be(expected.Instructor);
        }

        [Fact]
        public void Should_DeleteExistingCourse()
        {
            //arrange
            var course = _courseBuilder.Build();
            var id = _repository.Create(course);

            //act
            _repository.Delete(id);

            //assert
            var actual = _repository.GetBy(id);
            actual.Should().BeNull();
        }

        [Fact]
        public void Should_GetCourseById()
        {
            var expected = _courseBuilder.Build();
            var id = _repository.Create(expected);

            var actual = _repository.GetBy(id);

            actual.Should().Be(expected);
        }
    }
}