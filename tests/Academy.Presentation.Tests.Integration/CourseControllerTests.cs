using System;
using System.Collections.Generic;
using Academy.Application;
using Academy.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RESTFulSense.Clients;
using Xunit;

namespace Academy.Presentation.Tests.Integration
{
    public class CourseControllerTests
    {
        private const string Path = "/api/course";
        private readonly RESTFulApiFactoryClient _restClient;

        public CourseControllerTests()
        {
            var applicationFactory = new CustomWebApplicationFactory<Startup>();
            var httpClient = applicationFactory.CreateClient();
            _restClient = new RESTFulApiFactoryClient(httpClient);
        }

        [Fact]
        public async void Should_GetAllCourses()
        {
            //arrange

            //act
            var actual = await _restClient.GetContentAsync<List<Course>>(Path);

            //assert
            actual.Should().HaveCountGreaterOrEqualTo(0);
        }

        [Fact]
        public async void Should_CreateNewCourse()
        {
            //arrange
            var command = SomeCreateCourse();

            //act
            var id = await _restClient.PostContentAsync<CreateCourse, int>(Path, command);
            var courses = await _restClient.GetContentAsync<List<Course>>(Path);

            //assert
            courses.Should().ContainSingle(x => x.Id == id);
            await _restClient.DeleteContentAsync($"{Path}/{id}");
        }

        private static CreateCourse SomeCreateCourse()
        {
            return new CreateCourse
            {
                Name = Guid.NewGuid().ToString(),
                Instructor = "Hossein",
                IsOnline = true,
                Tuition = 780
            };
        }


        [Fact]
        public async void Should_EditExistingCourse()
        {
            //arrange
            var createCourse = SomeCreateCourse();
            var id = await _restClient.PostContentAsync<CreateCourse, int>(Path, createCourse);
            var editCourse = new EditCourse
            {
                Id = id,
                Name = "ASP.NET CORE 5",
                Instructor = createCourse.Instructor,
                IsOnline = createCourse.IsOnline,
                Tuition = 580
            };

            //act
            var newId = await _restClient.PutContentAsync<object>(Path, editCourse);

            //assert
            var courses = await _restClient.GetContentAsync<List<Course>>(Path);
            courses.Should().ContainSingle(x => x.Id == Convert.ToInt32(newId));
            courses.Should().NotContain(x => x.Id == id);
            await _restClient.DeleteContentAsync($"{Path}/{newId}");
        }

        [Fact]
        public async void Should_DeleteCourse()
        {
            var createCourse = SomeCreateCourse();
            var id = await _restClient.PostContentAsync<CreateCourse, int>(Path, createCourse);

            await _restClient.DeleteContentAsync($"{Path}/{id}");

            var courses = await _restClient.GetContentAsync<List<Course>>(Path);
            courses.Should().NotContain(x => x.Id == id);
        }
    }
}