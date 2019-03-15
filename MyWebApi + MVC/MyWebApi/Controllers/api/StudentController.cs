using MyWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebApi.Controllers
{
    public class StudentController : ApiController
    {
        #region "GET Action"
        public IHttpActionResult GetAllStudents(bool includeAddress = false)
        {
            IList<StudentViewModel> students = null;
            using (var ctx = new SchoolDBEntities1())
            {
                students = ctx.Students.Include("StudentAddress").Select(s => new StudentViewModel()
                {
                    Id = s.StudentID,
                    StudentName = s.StudentName,
                    Address = s.StudentAddress == null || includeAddress==false ? null : new AddressViewModel()
                    {
                        StudentId = s.StudentAddress.StudentID,
                        Address1 = s.StudentAddress.Address1,
                        Address2 = s.StudentAddress.Address2,
                        City = s.StudentAddress.City,
                        State = s.StudentAddress.State
                    }
                }).ToList<StudentViewModel>();
            }

            if (students.Count == 0)
            {
                return NotFound();
            }
            return Ok(students);
        }

        public IHttpActionResult GetStudentById(int id)
        {
            StudentViewModel student = null;
            using (var ctx = new SchoolDBEntities1())
            {
                student = ctx.Students.Include("StudentAddress")
                    .Where(s => s.StudentID == id)
                    .Select(s => new StudentViewModel()
                    {
                       Id = s.StudentID,
                       StudentName = s.StudentName
                    }).FirstOrDefault<StudentViewModel>();
            }

            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        public IHttpActionResult GetStudentByName(string name)
        {
            StudentViewModel student = null;
            using (var ctx = new SchoolDBEntities1())
            {
                student = ctx.Students.Include("StudentAddress")
                    .Where(s => s.StudentName.ToLower() == name.ToLower())
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName,
                        Address = s.StudentAddress == null ? null : new AddressViewModel()
                        {
                            StudentId = s.StudentAddress.StudentID,
                            Address1 = s.StudentAddress.Address1,
                            Address2 = s.StudentAddress.Address2,
                            City = s.StudentAddress.City,
                            State = s.StudentAddress.State
                        }
                    }).FirstOrDefault<StudentViewModel>();
            }
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        public IHttpActionResult GetAllStudentsInSameStandard(int standardId)
        {
            IList<StudentViewModel> students = null;
            using (var ctx = new SchoolDBEntities1())
            {
                students = ctx.Students.Include("StudentAddress")
                    .Where(s=>s.StandardId == standardId)
                    .Select(s => new StudentViewModel()
                {
                    Id = s.StudentID,
                    StudentName = s.StudentName,
                    Address = s.StudentAddress == null ? null : new AddressViewModel()
                    {
                        StudentId = s.StudentAddress.StudentID,
                        Address1 = s.StudentAddress.Address1,
                        Address2 = s.StudentAddress.Address2,
                        City = s.StudentAddress.City,
                        State = s.StudentAddress.State
                    },
                    Standard = new StandardViewModel()
                    {
                        StandardId = s.Standard.StandardId,
                        StandardName = s.Standard.StandardName
                    }
                }).ToList<StudentViewModel>();
            }

            if (students.Count == 0)
            {
                return NotFound();
            }
            return Ok(students);
        }
        #endregion

        #region "POST Action"
        public IHttpActionResult PostNewStudent(StudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            using (var ctx = new SchoolDBEntities1())
            {
                ctx.Students.Add(new Student()
                {
                    StudentID = student.Id,
                    StudentName = student.StudentName
                });

                ctx.SaveChanges();
            }

            return Ok();
        }
        #endregion

        #region "PUT Action"
        public IHttpActionResult Put(StudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not a valid model");
            }

            using (var ctx = new SchoolDBEntities1())
            {
                var existingStudent = ctx.Students.Where(s => s.StudentID == student.Id)
                    .FirstOrDefault<Student>();

                if (existingStudent != null)
                {
                    existingStudent.StudentName = student.StudentName;

                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }
        #endregion

        #region "DELETE Actoin"
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Not a valid student id");
            }

            using (var ctx = new SchoolDBEntities1())
            {
                var student = ctx.Students.Where(s => s.StudentID == id)
                    .FirstOrDefault();
                if (student == null)
                {
                    return NotFound();
                }
                else
                {
                    ctx.Entry(student).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
            }

            return Ok();
        }
        #endregion
    }
}
