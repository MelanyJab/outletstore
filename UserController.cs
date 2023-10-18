using Microsoft.IdentityModel.Tokens;
using outletstore.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using outletstore.Functions;

namespace outletstore.Controllers
{
    public class UserController : ApiController
    {
       static List<UserMo> users = new List<UserMo>
        {
            new UserMo { ID = 1, LastName = "Jabanian", FirstName = "Melany", PhoneNumber = "76406524", Email = "jabanianmelany@gmail.com"},
            new UserMo { ID = 2, LastName = "Paul", FirstName = "Logan", PhoneNumber = "78999999", Email = "user2@gmail.com" }
        };

        // GET: api/Users
        public IHttpActionResult Get()
        {
         
            var usersWithoutToken = users.Select(user => new UserMo
            {
                ID = user.ID,
                LastName = user.LastName,
                FirstName = user.FirstName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            }).ToList();

            return Content(HttpStatusCode.OK, usersWithoutToken);
        }

        // GET: api/Users/1
        public IHttpActionResult Get(int id)
        {
            UserMo user = users.Find(u => u.ID == id);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "User not found.");
            }

          
            user.Token = null;
            return Content(HttpStatusCode.OK, user);
        }

        // POST: api/Users
        public IHttpActionResult Post(UserMo user)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "All fields are required.");
            }

            // Check if the ID already exists.
            if (users.Any(u => u.ID == user.ID))
            {
                return Content(HttpStatusCode.BadRequest, "User with the same ID already exists.");
            }

            Crypto userauth = new Crypto();
            user.Token = userauth.GenerateToken(user.ID.ToString(), user.Email);
            users.Add(user);

            return Content(HttpStatusCode.Created, user);
        }

        // PUT: api/Users/1
        public IHttpActionResult Put(int id, [FromBody] UserMo user)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "All fields are required.");
            }

            bool found = false;
            UserMo updatedUser = new UserMo();

            users.ForEach(u =>
            {
                if (u.ID == id)
                {
                    u.LastName = user.LastName;
                    u.FirstName = user.FirstName;
                    u.PhoneNumber = user.PhoneNumber;
                    u.Email = user.Email;
                    u.Token = user.Token;
                    updatedUser = u;
                    found = true;
                }
            });

            if (!found)
            {
                return Content(HttpStatusCode.NotFound, "User not found.");
            }

            return Content(HttpStatusCode.OK, "User was updated successfully.");
        }

        // DELETE: api/Users/1
        public IHttpActionResult Delete(int id)
        {
            UserMo user = users.Find(u => u.ID == id);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "User not found.");
            }

            users.Remove(user);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
