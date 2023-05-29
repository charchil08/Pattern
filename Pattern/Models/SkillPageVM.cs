namespace Pattern.Models
{
    public class SkillPageVM
    {
        public FilterParamsVM Filters { get; set; }

        public PageListVM<UpsertSkillVm> Skills { get; set; }
    }
}
