using System.Collections.Generic;
using Academy.Application;
using Academy.Domain;
using Academy.Presentation.Controllers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Academy.Presentation.Tests.Unit
{
    public class CourseControllerTests
    {
        private readonly ICourseService _service;
        private readonly CourseController _controller;

        public CourseControllerTests()
        {
            _service = Substitute.For<ICourseService>();
            _controller = new CourseController(_service);
        }

        [Fact]
        public void Should_ReturnAllCourses()
        {
            //act
            _controller.GetCourses();

            //assert
            _service.Received().GetAll();
        }

        [Fact]
        public void Should_ReturnListOfAllCourses()
        {
            //arrange
            _service.GetAll().Returns(new List<Course>());

            //act
            var courses = _controller.GetCourses();

            //assert
            courses.Should().BeOfType<List<Course>>();
        }

        [Fact]
        public void Should_CreateANewCourse()
        {
            //arrange
            var command = new CreateCourse
            {
                Name = "tdd bdd",
                Tuition = 880
            };

            //act
            _controller.Create(command);

            //assert
            _service.Received().Create(command);
        }

        [Fact]
        public void Should_EditExistingCourse()
        {
            //arrange
            var command = new EditCourse()
            {
                Id = 5,
                Name = "tdd bdd",
                Tuition = 880
            };

            //act
            _controller.Edit(command);

            //assert
            _service.Received().Edit(command);
        }

        [Fact]
        public void Should_DeleteExistingCourse()
        {
            //arrange
            const int id = 5;

            //act
            _controller.Delete(id);

            //assert
            _service.Received().Delete(id);
        }
    }
}