using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdeasAPI.Models;
using IdeasAPI.Domain;
using IdeasAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace IdeasAPI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class IdeasController : Controller
    {

        private readonly IIdeaRepository _repository;
        private readonly IUserRepository _userRepository;

        public IdeasController(IIdeaRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        protected async Task<Idea> GetIdeaById(string id)
        {
            var userID = (await Functions.GetCurrentUser(HttpContext, _userRepository)).id;
            return await _repository.Query().Where(c => c.idUsers == userID).
                FirstOrDefaultAsync(x => x.id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        }

        // GET ideas/1
        [HttpGet("{page}")]
        public async Task<IActionResult> Get(int page)
        {
            var userID = (await Functions.GetCurrentUser(HttpContext, _userRepository)).id;
            var ideas = await _repository.Query().Where(c => c.idUsers == userID).
                OrderByDescending(c=> c.impact + c.ease + c.confidence).
                Skip(Math.Max(0, (page - 1)) * 10).Take(10).ToListAsync();

            if (!ideas.Any())
                return NotFound();

            var result = new IdeaListModel
            {
                Ideas = ideas.Select(idea => new IdeaModel
                {
                    id = idea.id,
                    content = idea.content,
                    impact = idea.impact,
                    ease = idea.ease,
                    confidence = idea.confidence,
                    average_score = ((float)idea.impact + (float)idea.ease + (float)idea.confidence) / 3,
                    created_at = idea.created_at.Ticks
                })
            };

            return Ok(result);
        }

        // POST ideas
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]IdeaModel value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var idea = new Idea
            {
                id = Guid.NewGuid().ToString("n").Substring(0, 8),
                idUsers = (await Functions.GetCurrentUser(HttpContext, _userRepository)).id,
                content = value.content,
                impact = value.impact,
                ease = value.ease,
                confidence = value.confidence,
                created_at = DateTime.UtcNow
            };

            await _repository.InsertAsync(idea);
            return Created($"ideas/{idea.id}", idea);
        }

        // PUT ideas/ir9tctewu
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]IdeaModel value)
        {
            var idea = await GetIdeaById(id);

            if (idea == null)
                return NotFound();

            idea.content = value.content;
            idea.impact = value.impact;
            idea.ease = value.ease;
            idea.confidence = value.confidence;

            await _repository.UpdateAsync(idea);

            return Ok(idea);

        }

        // DELETE ideas/ir9tctewu
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var idea = await GetIdeaById(id);

            if (idea == null)
                return NotFound();

            await _repository.DeleteAsync(idea);

            return NoContent();
        }
    }
}
