using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using MicroKubernete.Model;
using MicroKubernete.Repository;

namespace MicroKubernete.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ActorController : Controller
   {
      private readonly ActorRepository actorRepository;

      public ActorController(IConfiguration configuration)
      {
         actorRepository = new ActorRepository(configuration);
      }
      [HttpGet("")]
      public IActionResult GetAllActor()
      {
         IEnumerable<Actor> actors = actorRepository.FindAll();
         return Ok(actors);
      }
      [HttpGet("{id}")]
      public IActionResult GetActor(int id)
      {
         Actor order = actorRepository.FindByID(id);
         if (order == null)
         {
            return NotFound();
         }
         return Ok(order);
      }

      // POST: Actor/Create
      [HttpPost]
      public IActionResult Post([FromBody] Actor actor)
      {
         using (var scope = new TransactionScope())
         {
           int id=actorRepository.Add(actor);
            actor.actor_id = id;
            scope.Complete();
            return CreatedAtAction(nameof(GetActor), new { id = actor.actor_id },actor);
         }
      }
      // POST: /Actor/Edit   
      [HttpPut]
      public IActionResult Edit([FromBody] Actor actor)
      {
         if (actor != null)
         {
            using (var scope = new TransactionScope())
            {
               actorRepository.Update(actor);
               scope.Complete();
               return new OkResult();
            }
         }
         return new NoContentResult();
      }

      // GET:/Actor/Delete/1
      [HttpDelete("{id}")]
      public IActionResult Delete(int id)
      {
         actorRepository.Remove(id);
         return new OkResult();
      }
   }
}
