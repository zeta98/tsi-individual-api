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
  public class CoursesController : ControllerBase
  {
    private readonly UdelarOnlineContext _context;
    private readonly IMapper _mapper;

    public CoursesController(UdelarOnlineContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    // GET: api/Courses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCourses()
    {
      var courses = await _context.Courses
        .AsNoTracking()
        .Include(_ => _.Faculty)
        .Select(_ => new CourseDTO { Id = _.Id, Name = _.Name, FacultyId = _.FacultyId, Faculty = new FacultyDTO { Id = _.Faculty.Id, Name = _.Faculty.Name } })
        .ToListAsync();
      return Ok(courses);
    }

    // GET: api/Courses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDTO>> GetCourse([FromRoute] long id)
    {
      var course = await _context.Courses
        .Include(m => m.Faculty)
        .FirstOrDefaultAsync(i => i.Id == id);

      if (course == null)
      {
        return NotFound();
      }

      var courseDTO = _mapper.Map<CourseDTO>(course);

      return Ok(courseDTO);
    }

    // PUT: api/Courses/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCourse([FromRoute] long id, [FromBody] CourseDTO course)
    {
      if (id != course.Id)
      {
        return BadRequest();
      }

      var newCourse = _mapper.Map<Course>(course);

      _context.Entry(newCourse).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!CourseExists(id))
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

    // POST: api/Courses
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<CourseDTO>> PostCourse([FromBody] CourseDTO course)
    {
      var newCourse = _mapper.Map<Course>(course);
      if (newCourse.Id == 0)
      {
        _context.Courses.Add(newCourse);
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

      return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
    }

    // DELETE: api/Courses/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCourse([FromRoute] long id)
    {
      var course = await _context.Courses.Include(_ => _.CourseUsers).FirstOrDefaultAsync(i => i.Id == id);
      if (course == null)
      {
        return NotFound();
      }

      _context.Courses.Remove(course);
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

    private bool CourseExists(long id)
    {
      return _context.Courses.Any(e => e.Id == id);
    }
  }
}
