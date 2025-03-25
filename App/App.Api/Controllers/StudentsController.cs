using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using App.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        public static List<Student> studentsList = new()
        {
            new Student {Number = 1, Name = "Mahmut", Surname = "Tunç", Class = "A", Grade = 51},
            new Student {Number = 2, Name = "Cemil", Surname = "Yılmaz", Class = "B", Grade = 51}
        };
        public static int number = 3;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        public IActionResult GetList()
        {
            return Ok(studentsList);
        }

        [HttpGet("{number}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetStudent([FromRoute] int number)
        {
            var student = studentsList.Find(x => x.Number == number);

            if (student == null)
            {
                return NotFound("Böyle bir öğrenci bulunamadı.");
            }

            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]        
        public IActionResult NewStudent([FromBody] NewStudent student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = new Student
            {
                Number = number,
                Name = student.Name,
                Surname = student.Surname,
                Class = student.Class,
                Grade = student.Grade,
            };
            number++;

            studentsList.Add(item);
            return Ok();
        }

        [HttpPut("{number}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]        
        public IActionResult ChangeStudent([FromRoute] int number, [FromBody] NewStudent student)
        {
            var item = studentsList.Find(x => x.Number == number);
            if (item == null)
            {
                return NotFound("Böyle bir öğrenci bulunamadı.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            item.Name = student.Name;
            item.Surname = student.Surname;
            item.Class = student.Class;
            item.Grade = student.Grade;
            return Ok("Öğrenci güncellendi.");
        }

        [HttpDelete("delete/{number}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(string))]
        public IActionResult DeleteStudent([FromRoute] int number)
        {
            var item = studentsList.Find(x => x.Number == number);
            if (item == null)
            {
                return NotFound("Böyle bir öğrenci bulunamadı.");
            }

            studentsList.Remove(item);
            return Ok("Öğrenci silindi.");
        }
    }
}
