using Antlr.Runtime.Misc;
using outletstore.Functions;
using outletstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace outletstore.Controllers
{
  
    public class ItemsController : ApiController
    {

       static List<item> items = new List<item> {
        new item { ID = 220450, Name = "Jersey", Price = 15, Quantity = 50 },
        new item { ID = 220550, Name = "Tank Top", Price = 8, Quantity = 105 },
        new item { ID = 220650, Name = "Levis Jeans", Price = 45, Quantity = 10 }

        };
        // GET: api/Items
        public IHttpActionResult Get()
        {
            // Return a list of all items
        
            return Content(HttpStatusCode.OK,items);

        }

        // GET: api/Items/5
        public IHttpActionResult Get(int id)
        {
            item item = items.Find(s => s.ID == id); 
            if(item == null)
            {
                return Content(HttpStatusCode.NotFound,"Item not found.");
            }
            return Content(HttpStatusCode.OK, item);
        }

        public IHttpActionResult Get(string name)
        {
            List<item> filteredItems = items.Where(i => i.Name.ToLower().Contains(name.ToLower())).ToList();
            if (filteredItems.Count == 0)
            {
                return Content(HttpStatusCode.NotFound, "No items match this name.");
            }
            return Content(HttpStatusCode.OK, filteredItems);
        }

        // POST: api/Items
        [ApiAuthentication]
        public IHttpActionResult Post(item item)
        {
            if(!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "All fields are required.");
            }
            // Check if the ID already exists
            if (items.Any(i => i.ID == item.ID))
            {
                return Content(HttpStatusCode.BadRequest, "Item with the same ID already exists.");
            }
            items.Add(item);
            return Content(HttpStatusCode.Created,"Item was added Successfully.");
        }
        
        // PUT: api/Items/5
        [ApiAuthentication]
        public IHttpActionResult Put(int id, [FromBody]item item)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "All fields are required");
            }
            bool found = false;
            item updatedItem = new item();
            items.ForEach(s=>
            {
                if(s.ID == id)
                {
                    s.Name = item.Name;
                    s.Price = item.Price;
                    s.Quantity = item.Quantity;
                    updatedItem = s;
                    found = true;
                }
            });
            if (!found)
            {
                return Content(HttpStatusCode.NotFound, "Item not found.");
            }
            return Content(HttpStatusCode.OK, "Item was updated Successfully.");
        }

        // DELETE: api/Items/5
        [ApiAuthentication]
        public IHttpActionResult Delete(int id)
        {
            item item = items.Find(s => s.ID == id);
            if (item == null)
            {
                return Content(HttpStatusCode.NotFound, "Item not found.");
            }
            items.Remove(item);
            return StatusCode(HttpStatusCode.NoContent);


        }
    }
}
