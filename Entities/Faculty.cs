using System.Collections.Generic;

namespace UdelarOnlineApi.Entities
{
  public class Faculty
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public ICollection<User> Users { get; set; }
    public ICollection<Course> Courses { get; set; }
  }

}