using AutoMapper;
using Pattern.DTO;
using Pattern.Entity.DataModels;
using Pattern.Models;

namespace Pattern
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Skill, SkillDTO>();
            CreateMap<SkillDTO, UpsertSkillVm>();
            CreateMap<UpsertSkillVm, Skill>();
            CreateMap<Skill, UpsertSkillVm>();
            CreateMap<FilterParamsVM, FilterParams>();
            CreateMap(typeof(PageListVM<>), typeof(PageList<>));
            CreateMap(typeof(PageList<>), typeof(PageListVM<>));

            CreateMap<LoginVM, LoginDTO>();
            CreateMap<User, UserDTO>();
        }
    }
}

