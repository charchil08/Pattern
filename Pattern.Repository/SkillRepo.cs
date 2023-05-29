using Pattern.Entity.Data;
using Pattern.Entity.DataModels;
using Pattern.Repository.Interface;

namespace Pattern.Repository
{
    public class SkillRepo: GenericRepo<Skill>, ISkillRepo
    {

        public SkillRepo(PatternContext context) : base(context) { }
        
    }
}
