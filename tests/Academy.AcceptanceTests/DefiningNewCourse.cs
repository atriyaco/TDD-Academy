using System;
using Academy.Application;
using FluentAssertions;
using Newtonsoft.Json;
using RestSharp;
using TestStack.BDDfy;
using Xunit;

namespace Academy.AcceptanceTests
{
    public class DefiningNewCourse : IClassFixture<StartHostFixture>
    {
        [Fact]
        public void CreatingANewCourse()
        {
            var course = CreateSomeCourse();

            this.Given(_ => _.IWantToCreateTheFollowingCourse(course), "Given I Want To Create WebApi As A Course")
                .When(_ => _.IPressAddButton())
                .Then(_ => _.TheFollowingCourseShouldBeAvailableOnList(course),
                    "Then WebApi Should Be Available On List")
                .BDDfy();
        }

        [Fact]
        public void DuplicatedCourseCantBeCreated()
        {
            var course = CreateSomeCourse();

            this.Given(_ => _.IHaveAlreadyCreatedFollowingCourse(course))
                .When(_ => _.ITryToCreateItAgain())
                .Then(_ => _.TheCourseShouldNotBeAppearedInListTwice())
                .BDDfy();
        }

        #region Functions

        private CreateCourse _course;

        public void IWantToCreateTheFollowingCourse(CreateCourse course)
        {
            _course = course;
        }

        public void IPressAddButton()
        {
            var id = PostTheCourse();
            _course.Id = id;
        }


        public void TheFollowingCourseShouldBeAvailableOnList(CreateCourse course)
        {
            _course.Id.Should().Should().NotBe(0);
        }


        private static CreateCourse CreateSomeCourse()
        {
            return new CreateCourse
            {
                Id = 126,
                Instructor = "",
                IsOnline = false,
                Name = Guid.NewGuid().ToString(),
                Tuition = 154
            };
        }

        public void IHaveAlreadyCreatedFollowingCourse(CreateCourse course)
        {
            _course = course;
            PostTheCourse();
        }

        public void ITryToCreateItAgain()
        {
            var id = PostTheCourse();
            _course.Id = id;
        }

        public void TheCourseShouldNotBeAppearedInListTwice()
        {
            _course.Id.Should().Be(0);
        }

        private int PostTheCourse()
        {
            var restClient = new RestClient(HostConstants.Endpoint);
            var restRequest = new RestRequest("Course", DataFormat.Json);
            restRequest.AddJsonBody(_course);
            //restRequest.Method = Method.POST;
            var response = restClient.Post(restRequest);
            var id = JsonConvert.DeserializeObject<int>(response.Content);
            return id;
        }

        #endregion
    }
}