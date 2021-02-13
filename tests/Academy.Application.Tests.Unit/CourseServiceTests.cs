using System;
using System.Collections.Generic;
using Academy.Domain;
using Academy.Domain.Exceptions;
using Academy.Domain.Tests.Unit.Builders;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Tynamix.ObjectFiller;
using Xunit;

namespace Academy.Application.Tests.Unit
{
    public class CourseServiceTests
    {
        private readonly CourseTestBuilder _courseTestBuilder;
        private readonly CourseService _courseService;
        private readonly ICourseRepository _courseRepository;

        public CourseServiceTests()
        {
            _courseTestBuilder = new CourseTestBuilder();
            _courseRepository = Substitute.For<ICourseRepository>();
            _courseService = new CourseService(_courseRepository);
        }

        [Fact]
        public void Should_CreateANewCourse()
        {
            //arrange
            var command = SomeCreateCourse();

            //act
            _courseService.Create(command);

            //assert
            _courseRepository.ReceivedWithAnyArgs().Create(default);
        }

        private static CreateCourse SomeCreateCourse()
        {
            var filler = new Filler<CreateCourse>();
            filler.Setup().OnProperty(x => x.Tuition).Use(780);
            return filler.Create();
        }

        [Fact]
        public void Should_CreateNewCourseAndReturnId()
        {
            //arrange
            var command = SomeCreateCourse();
            _courseRepository.Create(default).ReturnsForAnyArgs(10);

            //act
            var actual = _courseService.Create(command);

            //assert
            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Should_ThrowException_WhenAddingCourseIsDuplicated()
        {
            //arrange
            var command = SomeCreateCourse();
            var course = _courseTestBuilder.Build();
            _courseRepository.GetBy(Arg.Any<string>()).Returns(course);

            //act
            Action actual = () => _courseService.Create(command);

            //assert
            actual.Should().Throw<DuplicatedCourseNameException>();
        }

        [Fact]
        public void Should_UpdateCourse()
        {
            //arrange
            var command = SomeEditCourse();
            var course = _courseTestBuilder.Build();
            _courseRepository.GetBy(command.Id).Returns(course);

            //act
            _courseService.Edit(command);

            //assert
            Received.InOrder(() =>
            {
                _courseRepository.Delete(command.Id);
                _courseRepository.Create(Arg.Any<Course>());
            });
        }

        [Fact]
        public void Should_ReturnIdOfUpdatedRecord()
        {
            //arrange
            var command = SomeEditCourse();
            _courseRepository.Create(default).ReturnsForAnyArgs(10);
            var course = _courseTestBuilder.Build();
            _courseRepository.GetBy(Arg.Any<int>()).Returns(course);


            //act
            var actual = _courseService.Edit(command);

            //assert
            actual.Should().BeGreaterThan(0);
        }


        [Fact]
        public void Should_ThrowException_WhenUpdatingCourseNotExists()
        {
            //arrange
            var command = SomeEditCourse();
            _courseRepository.GetBy(command.Id).ReturnsNull();

            //act
            Action action = () => _courseService.Edit(command);

            //assert
            action.Should().Throw<CourseNotExistsException>();
        }

        private static EditCourse SomeEditCourse()
        {
            return new EditCourse
            {
                Id = 12,
                IsOnline = true,
                Instructor = "Hossein",
                Name = "Onion Architecture",
                Tuition = 200
            };
        }

        [Fact]
        public void Should_DeleteCourse()
        {
            //arrange
            const int id = 12;

            //act
            _courseService.Delete(id);

            //assert
            _courseRepository.Received().Delete(id);
        }

        [Fact]
        public void Should_GetListOfCourses()
        {
            //arrange
            _courseRepository.GetAll().Returns(new List<Course>());

            //act
            var courses = _courseService.GetAll();

            //assert
            courses.Should().BeOfType<List<Course>>();
            _courseRepository.Received().GetAll();
        }
    }
}