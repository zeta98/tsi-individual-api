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
  public class CourseUsersController : ControllerBase
  {
    private readonly UdelarOnlineContext _context;
    private readonly IMapper _mapper;

    public CourseUsersController(UdelarOnlineContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    // GET: api/Courses
    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCourses([FromRoute] long userId)
    {
      var courseUsers = await _context.CourseUsers
        .AsNoTracking()
        .Where(_ => _.UserId == userId)
        .Select(_ => new CourseUserDTO
        {
          CourseId = _.CourseId,
          UserId = _.UserId,
          Course = new CourseDTO
          {
            Id = _.Course.Id,
            Name = _.Course.Name,
            Faculty = new FacultyDTO
            {
              Id = _.Course.Faculty.Id,
              Name = _.Course.Faculty.Name
            }
          },
          User = new UserDTO
          {
            Id = _.User.Id,
            Name = _.User.Name
          }
        })
        .ToListAsync();
      return Ok(courseUsers);
    }

    // POST: api/Courses
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost("{userId}")]
    public async Task<ActionResult<CourseUserDTO>> PostCourseUser([FromRoute] long userId, [FromBody] CourseUserDTO courseUser)
    {
      var newCourseUser = _mapper.Map<CourseUser>(courseUser);
      newCourseUser.UserId = userId;

      // validate Faculty

      var course = await _context.Courses.FindAsync(newCourseUser.CourseId);
      var user = await _context.Users.FindAsync(newCourseUser.UserId);

      if (user == null || course == null || course.FacultyId != user.FacultyId)
      {
        return BadRequest();
      }

      _context.CourseUsers.Add(newCourseUser);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        return BadRequest();
      }

      return Ok();
    }

    // DELETE: api/Courses/5
    [HttpDelete("{userId}/{courseId}")]
    public async Task<ActionResult> DeleteCourse([FromRoute] long userId, [FromRoute] long courseId)
    {
      var courseUser = await _context.CourseUsers.FirstOrDefaultAsync(_ => (_.CourseId == courseId) && (_.UserId == userId));
      if (courseUser == null)
      {
        return NotFound();
      }

      _context.CourseUsers.Remove(courseUser);
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
  }
}
