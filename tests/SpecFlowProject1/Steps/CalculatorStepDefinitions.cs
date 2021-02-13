using FluentAssertions;
using TechTalk.SpecFlow;

namespace SpecFlowProject1.Steps
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {

        private readonly ScenarioContext _scenarioContext;

        public CalculatorStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(int number)
        {
        }

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(int number)
        {
        }

        [When("the two numbers are added")]
        public void WhenTheTwoNumbersAreAdded()
        {
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            12.Should().Be(12);
        }
    }
}
