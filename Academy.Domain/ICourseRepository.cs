using System.Collections.Generic;

namespace Academy.Domain
{
    public interface ICourseRepository
    {
        List<Course> GetAll();
        Course GetBy(string name);
        int Create(Course course);
        void Delete(int id);
        Course GetBy(int id);
    }
}