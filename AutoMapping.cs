using AutoMapper;
using UdelarOnlineApi.Models;
using UdelarOnlineApi.Entities;
public class AutoMapping : Profile
{
  public AutoMapping()
  {
    CreateMap<User, UserDTO>();
    CreateMap<UserDTO, User>();
    CreateMap<Faculty, FacultyDTO>();
    CreateMap<FacultyDTO, Faculty>();
    CreateMap<Course, CourseDTO>();
    CreateMap<CourseDTO, Course>();
    CreateMap<CourseUser, CourseUserDTO>();
    CreateMap<CourseUserDTO, CourseUser>();
  }
}