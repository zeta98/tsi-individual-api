using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdelarOnlineApi.Entities;
using UdelarOnlineApi.Models;
using AutoMapper;


namespace UdelarOnlineApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly UdelarOnlineContext _context;
    private readonly IMapper _mapper;

    public UsersController(UdelarOnlineContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
      var users = await _context.Users
        .AsNoTracking()
        .Include(_ => _.Faculty)
        .Select(_ => new UserDTO { Id = _.Id, Name = _.Name, FacultyId = _.FacultyId, Faculty = new FacultyDTO { Id = _.Faculty.Id, Name = _.Faculty.Name } })
        .ToListAsync();
      return Ok(users);
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser(long id)
    {
      var user = await _context.Users
        .Include(m => m.Faculty)
        .FirstOrDefaultAsync(i => i.Id == id);

      if (user == null)
      {
        return NotFound();
      }
      var userDTO = _mapper.Map<UserDTO>(user);

      return Ok(userDTO);
    }

    [HttpGet("coursesAllowed/{id}")]
    public async Task<IActionResult> GetUserCoursesAllowed([FromRoute] long id)
    {
      var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == id);

      if (user == null)
      {
        return NotFound();
      }

      var courses = await _context.Courses
        .AsNoTracking()
        .Include(_ => _.CourseUsers)
        .Where(_ => (_.FacultyId == user.FacultyId)/* && (_.CourseUsers.FirstOrDefault(_ => _.UserId == id) == null)*/)
        .Select(_ => new CourseDTO { Id = _.Id, Name = _.Name })
        .ToListAsync();

      return Ok(courses);
    }

    // PUT: api/Users/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(long id, UserDTO user)
    {
      if (id != user.Id)
      {
        return BadRequest();
      }

      var newUser = _mapper.Map<User>(user);

      _context.Entry(newUser).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!UserExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Users
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<UserDTO>> PostUser(UserDTO user)
    {
      var newUser = _mapper.Map<User>(user);
      if (newUser.Id == 0)
      {
        _context.Users.Add(newUser);
        try
        {
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
          return BadRequest();
        }
      }
      else
      {
        return BadRequest();
      }

      return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(long id)
    {
      var user = await _context.Users.Include(_ => _.CourseUsers).FirstOrDefaultAsync(i => i.Id == id);
      if (user == null)
      {
        return NotFound();
      }

      _context.Users.Remove(user);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException e)
      {
        return BadRequest(e);
      }

      return Ok();
    }

    private bool UserExists(long id)
    {
      return _context.Users.Any(e => e.Id == id);
    }
  }
}
