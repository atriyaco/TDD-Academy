using System.Collections.Generic;
using Academy.Domain;
using Academy.Domain.Exceptions;

namespace Academy.Application
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public int Create(CreateCourse command)
        {
            //TODO: Try Catch to return 0 on exception
            if (_courseRepository.GetBy(command.Name) != null)
                throw new DuplicatedCourseNameException();

            var course = new Course(command.Name, command.IsOnline, command.Tuition, command.Instructor);
            return _courseRepository.Create(course);
        }

        public int Edit(EditCourse command)
        {
            if (_courseRepository.GetBy(command.Id) == null)
                throw new CourseNotExistsException();

            _courseRepository.Delete(command.Id);
            var course = new Course(command.Name, command.IsOnline, command.Tuition, command.Instructor);
            return _courseRepository.Create(course);
        }

        public void Delete(int id)
        {
            _courseRepository.Delete(id);
        }

        public List<Course> GetAll()
        {
            return _courseRepository.GetAll();
        }
    }
}