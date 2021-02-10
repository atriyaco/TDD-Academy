using System.Collections.Generic;
using Academy.Domain;

namespace Academy.Application
{
    public interface ICourseService
    {
        int Create(CreateCourse command);
        int Edit(EditCourse command);
        void Delete(int id);
        List<Course> GetAll();
    }
}
