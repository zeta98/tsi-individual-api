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
  public class FacultiesController : ControllerBase
  {
    private readonly UdelarOnlineContext _context;
    private readonly IMapper _mapper;

    public FacultiesController(UdelarOnlineContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    // GET: api/Faculties
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FacultyDTO>>> GetFaculties()
    {
      var faculties = await _context.Faculties
        .Select(_ => new FacultyDTO { Id = _.Id, Name = _.Name })
        .ToListAsync();
      return Ok(faculties);
    }

    // GET: api/Faculties/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FacultyDTO>> GetFaculty(long id)
    {
      var faculty = await _context.Faculties
        .FindAsync(id);

      if (faculty == null)
      {
        return NotFound();
      }

      var facultyDTO = _mapper.Map<FacultyDTO>(faculty);

      return Ok(facultyDTO);
    }

    // PUT: api/Faculties/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFaculty(long id, FacultyDTO faculty)
    {
      if (id != faculty.Id)
      {
        return BadRequest();
      }

      var newFaculty = _mapper.Map<Faculty>(faculty);

      _context.Entry(newFaculty).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!FacultyExists(id))
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

    // POST: api/Faculties
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<FacultyDTO>> PostFaculty(FacultyDTO faculty)
    {
      var newFaculty = _mapper.Map<Faculty>(faculty);

      if (newFaculty.Id == 0)
      {
        _context.Faculties.Add(newFaculty);
        try
        {
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          return BadRequest();
        }
      }
      else
      {
        return BadRequest();
      }

      return CreatedAtAction(nameof(GetFaculty), new { id = faculty.Id }, faculty);
    }

    // DELETE: api/Faculties/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFaculty(long id)
    {
      var faculty = await _context.Faculties
        .Include(_ => _.Courses).ThenInclude(_ => _.CourseUsers)
        .Include(_ => _.Users).ThenInclude(_ => _.CourseUsers)
        .FirstOrDefaultAsync(_ => _.Id == id);
      if (faculty == null)
      {
        return NotFound();
      }

      try
      {
        _context.Faculties.Remove(faculty);
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException e)
      {
        return BadRequest(e);
      }

      return Ok();
    }

    private bool FacultyExists(long id)
    {
      return _context.Faculties.Any(e => e.Id == id);
    }
  }
}
