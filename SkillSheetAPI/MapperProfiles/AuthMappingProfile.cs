using AutoMapper;
using DataAccess.Entities.Entities;
using SkillSheetAPI.Models.DTOs; 

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<UserDetail, UserDTO>(); // Entity to Domain Model
        CreateMap<UserRegisterDTO,UserDetail>();  
     
        CreateMap< ChangePasswordDTO,UserRegisterDTO>();  // DTO to Model  
        CreateMap< UserRegisterDTO, UserLoginDTO>(); 
    }
}
