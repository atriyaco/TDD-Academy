using System;
using System.Collections.Generic;
using Academy.Application;
using Academy.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public List<Course> GetCourses()
        {
            return _courseService.GetAll();
        }

        [HttpPost]
        public int Create(CreateCourse command)
        {
            try
            {
                return _courseService.Create(command);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        [HttpPut]
        public int Edit(EditCourse command)
        {
            return _courseService.Edit(command);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _courseService.Delete(id);
        }
    }
}