using System.Collections.Generic;

namespace UdelarOnlineApi.Models
{
  public class FacultyDTO
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public ICollection<UserDTO> Users { get; set; }
    public ICollection<CourseDTO> Courses { get; set; }
  }

}