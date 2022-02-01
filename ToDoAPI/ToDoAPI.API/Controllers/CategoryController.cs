using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models;
using ToDoAPI.DATA.EF;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoryController : ApiController
    {
        //Step 3 - create the connection to the db
        ToDoEntities db = new ToDoEntities();

        //Step 4 - GetCategories
        public IHttpActionResult GetToDos()
        {

            List<CategoryViewModel> cats = db.Categories.Select(c => new CategoryViewModel()
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            }).ToList<CategoryViewModel>();

            if (cats.Count == 0)
            {
                return NotFound();//Error 404
            }

            return Ok(cats);
        }//end GetCategories

        //Step 5 - Create a Get for one category
        public IHttpActionResult GetToDo(int id)
        {

            CategoryViewModel cat = db.Categories.Where(c => c.CategoryId ==
              id).Select(c => new CategoryViewModel()
              {
                  CategoryId = c.CategoryId,
                  Name = c.Name,
                  Description = c.Description
              }).FirstOrDefault();

            if (cat == null)
            {
                return NotFound();
            }

            return Ok(cat);
        }//end GetCategory

        //Step - 6 Create functionality
        public IHttpActionResult PostToDo(CategoryViewModel cat)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            db.Categories.Add(new Category()
            {
                Name = cat.Name,
                Description = cat.Description
            });

            db.SaveChanges();
            return Ok();
        }//end PostCategory

        //Step 7
        //api/Categories (HttpPut)
        public IHttpActionResult PutToDo(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            //Category object to pass the info to the db
            Category existingCat = db.Categories.Where(c => c.CategoryId == cat.CategoryId).FirstOrDefault();

            if (existingCat != null)
            {
                existingCat.Name = cat.Name;
                existingCat.Description = cat.Description;

                db.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end PutCategory()

        //Step - 8        
        public IHttpActionResult DeleteToDo(int id)
        {
            Category cat = db.Categories.Where(c => c.CategoryId == id).FirstOrDefault();

            if (cat != null)
            {
                db.Categories.Remove(cat);

                db.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end DeleteCategory

        //Step - 9
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }

    }//end class
}//end namespace
