using System.Collections.Generic;

namespace UdelarOnlineApi.Models
{
  public class UserDTO
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public long FacultyId { get; set; }
    public FacultyDTO Faculty { get; set; }

    public ICollection<CourseUserDTO> CourseUsers { get; set; }
  }
}