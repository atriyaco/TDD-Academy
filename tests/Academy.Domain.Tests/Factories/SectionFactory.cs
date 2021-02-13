namespace Academy.Domain.Tests.Unit.Factories
{
    public class SectionFactory
    {
        public static Section Create()
        {
            return new Section(1, "tdd section");
        }
    }
}