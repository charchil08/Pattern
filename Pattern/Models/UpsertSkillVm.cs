using System.ComponentModel.DataAnnotations;

namespace Pattern.Models
{
    public class UpsertSkillVm
    {

        public int Id { get; set; }

        [Required(ErrorMessage="Skill name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage="Select Status")]
        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
