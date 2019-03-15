using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Controllers.api
{
    public class CustomerController : ApiController
    {
        public IHttpActionResult GetAllCustomer()
        {
            IList<CustomerDTO> customers = null;
            using (var ctx = new CRUD_SampleEntities())
            {
                customers = ctx.Customers.Select(c => new CustomerDTO()
                {
                    Id = c.Id,
                    CustName = c.CustName,
                    CustEmail = c.CustEmail
                }).ToList<CustomerDTO>();
            }

            if (customers.Count == 0)
            {
                return NotFound();
            }
            return Ok(customers);
        }

        public IHttpActionResult GetCustomerById(int id)
        {
            CustomerDTO customer = null;
            using (var ctx = new CRUD_SampleEntities())
            {
                customer = ctx.Customers.Where(c => c.Id == id)
                    .Select(c => new CustomerDTO()
                    {
                        Id = c.Id,
                        CustName = c.CustName,
                        CustEmail = c.CustEmail
                    }).FirstOrDefault<CustomerDTO>();
            }
            if (customer==null)
            { 
                return NotFound();
            }
            return Ok(customer);
        }

        public IHttpActionResult PostNewCustomer(CustomerDTO customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            using (var ctx = new CRUD_SampleEntities())
            {
                ctx.Customers.Add(new Customer()
                {
                    Id = customer.Id,
                    CustName = customer.CustName,
                    CustEmail = customer.CustEmail
                });

                ctx.SaveChanges();
            }
            return Ok();
        }
        
        public IHttpActionResult Put(CustomerDTO customer)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");
            using (var ctx = new CRUD_SampleEntities())
            {
                var existingCustomer = ctx.Customers.Where(c => c.Id == customer.Id).FirstOrDefault();
                if (existingCustomer == null)
                {
                    return NotFound();
                }
                existingCustomer.CustName = customer.CustName;
                existingCustomer.CustEmail = customer.CustEmail;
                ctx.SaveChanges();
            }
            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest("Invalid customer id");
            using (var ctx = new CRUD_SampleEntities())
            {
                var existingCustomer = ctx.Customers.Where(c => c.Id == id).FirstOrDefault();
                if (existingCustomer == null)
                    return NotFound();
                ctx.Entry(existingCustomer).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return Ok();
        }
    }
}
