using Pattern.DTO;
using Pattern.Entity.DataModels;

namespace Pattern.Service.Interface
{
    public interface ISkillService : IBaseService<Skill>
    {
        PageList<SkillDTO> GetList(FilterParams filterParams);
    }
}
