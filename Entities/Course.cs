using System.Collections.Generic;

namespace UdelarOnlineApi.Entities
{
  public class Course
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public long FacultyId { get; set; }
    public Faculty Faculty { get; set; }

    public ICollection<CourseUser> CourseUsers { get; set; }
  }

}