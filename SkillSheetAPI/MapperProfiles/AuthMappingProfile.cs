using AutoMapper;
using DataAccess.Entities.Entities;
using SkillSheetAPI.Models.DTOs; 

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<User, UserDTO>(); // Entity to Domain Model
        CreateMap<UserRegisterDTO,User>();  
     
        CreateMap< ChangePasswordDTO,UserRegisterDTO>();  // DTO to Model  
        CreateMap< UserRegisterDTO, UserLoginDTO>(); 
    }
}
