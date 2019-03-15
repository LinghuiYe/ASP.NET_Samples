using MyWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MyWebApi.Controllers
{
    public class StudentMVCController : Controller
    {
        // GET: StudentMVC
        public ActionResult Index()
        {
            IEnumerable<StudentViewModel> students = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65341/api/");
                //HTTP GET
                var getTask = client.GetAsync("student");
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<StudentViewModel>>();
                    readTask.Wait();

                    students = readTask.Result;
                }
                else //web api send error response
                {
                    //log response status here...
                    students = Enumerable.Empty<StudentViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator");
                }
            }
                return View(students);
        }

        public ActionResult Details(int id)
        {
            StudentViewModel student = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65341/api/");
                var getTask = client.GetAsync("student?id=" + id.ToString());
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultTask = result.Content.ReadAsAsync<StudentViewModel>();
                    resultTask.Wait();

                    student = resultTask.Result;
                }
            }
            return View(student);
        }

        //POST: StudentMVC
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentViewModel student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65341/api/student");

                //Http Post
                var postTask = client.PostAsJsonAsync<StudentViewModel>("student", student);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(student);
        }

        //PUT: StudentMVC
        public ActionResult Edit(int id)
        {
            StudentViewModel student = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65341/api/");
                //Http GET
                var getTask = client.GetAsync("student?id=" + id.ToString());
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<StudentViewModel>();
                    readTask.Wait();

                    student = readTask.Result;
                }
            }
            return View(student);
        }

        [HttpPost]
        public ActionResult Edit(StudentViewModel student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65341/api/student");
                //Http PUT
                var putTask = client.PutAsJsonAsync<StudentViewModel>("student", student);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(student);
        }

        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65341/api/");
                //Http DELETE
                var deleteTask = client.DeleteAsync("student?id=" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}