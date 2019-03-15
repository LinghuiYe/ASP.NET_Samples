using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Hubs;
using WebApplication1.Models;
using WebApplication1.Models.DTO;
using WebApplication1.Repository;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private CustomerService objCust;

        //CustomerRepository CustRepository;
        public HomeController()
        {
            this.objCust = new CustomerService();
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAllData()
        {
            //int Count = 10; IEnumerable<object> Result = null;
            //try
            //{
            //    object[] parameters = { Count };
            //    Result = objCust.GetAll(parameters);

            //}
            //catch { }
            //return PartialView("_DataList", Result);

            IEnumerable<CustomerDTO> customers = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56775/api/");
                var getTask = client.GetAsync("customer");
                getTask.Wait();

                var getResult = getTask.Result;
                if (getResult.IsSuccessStatusCode)
                {
                    var readTask = getResult.Content.ReadAsAsync<IList<CustomerDTO>>();
                    readTask.Wait();

                    customers = readTask.Result;
                }
                else
                {
                    customers = Enumerable.Empty<CustomerDTO>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator");
                }
            }
            return PartialView("_DataList", customers);
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(CustomerDTO model)
        {
            //int result = 0;
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        object[] parameters = { model.CustName, model.CustEmail };
            //        result = objCust.Insert(parameters);
            //        if (result == 1)
            //        {
            //            //Notify to all
            //            CustomerHub.BroadcastData();
            //        }
            //    }
            //}
            //catch { }
            //return View("Index");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56775/api/customer");
                var postTask = client.PostAsJsonAsync<CustomerDTO>("customer", model);
                postTask.Wait();

                var postResult = postTask.Result;
                if (postResult.IsSuccessStatusCode)
                {
                    CustomerHub.BroadcastData();
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            Customer cus = new Customer() {
                Id = model.Id,
                CustName = model.CustName,
                CustEmail = model.CustEmail
            };

            return View(cus);
        }
        public ActionResult Delete(int id)
        {
            int result = 0;
            try
            {
                object[] parameters = { id };
                result = objCust.Delete(parameters);
                if (result == 1)
                {
                    //Notify to all
                    CustomerHub.BroadcastData();
                }
            }
            catch { }
            return View("Index");
        }

        public ActionResult Update(int id)
        {
            object result = null;
            try
            {
                object[] parameters = { id };
                result = this.objCust.GetbyID(parameters);
            }
            catch { }
            return View(result);
        }

        [HttpPost]
        public ActionResult Update(Customer model)
        {
            int result = 0;
            try
            {
                object[] parameters = { model.Id, model.CustName, model.CustEmail };
                result = objCust.Update(parameters);

                if (result == 1)
                {
                    //Notify to all
                    CustomerHub.BroadcastData();
                }
            }
            catch { }
            return View("Index");
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}