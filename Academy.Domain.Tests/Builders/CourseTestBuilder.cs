namespace Academy.Domain.Tests.Unit.Builders
{
    public class CourseTestBuilder
    {
        private string _name = "tdd & bdd";
        private const bool IsOnline = true;
        private double _tuition = 600;
        private static string _instructor = "hossein";

        public CourseTestBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CourseTestBuilder WithTuition(double tuition)
        {
            _tuition = tuition;
            return this;
        }

        public CourseTestBuilder WithInstructor(string instructor)
        {
             _instructor = instructor;
            return this;
        }
        public Course Build()
        {
            return new Course(_name, IsOnline, _tuition, _instructor);
        }
    }
}