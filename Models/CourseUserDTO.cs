namespace UdelarOnlineApi.Models
{
  public class CourseUserDTO
  {
    public long UserId { get; set; }
    public UserDTO User { get; set; }

    public long CourseId { get; set; }
    public CourseDTO Course { get; set; }
  }
}