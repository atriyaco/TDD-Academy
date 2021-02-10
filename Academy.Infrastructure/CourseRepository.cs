using System;
using System.Collections.Generic;
using System.Linq;
using Academy.Domain;

namespace Academy.Infrastructure
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AcademyContext _context;

        public CourseRepository(AcademyContext context)
        {
            _context = context;
        }

        public List<Course> GetAll()
        {
            return _context.Courses.ToList();
        }

        public Course GetBy(string name)
        {
            return _context.Courses.FirstOrDefault(x => x.Name == name);
        }

        public int Create(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
            return course.Id;
        }

        public void Delete(int id)
        {
            var course = _context.Courses.Find(id);
            _context.Courses.Remove(course);
            _context.SaveChanges();
        }

        public Course GetBy(int id)
        {
            return _context.Courses.Find(id);
        }
    }
}