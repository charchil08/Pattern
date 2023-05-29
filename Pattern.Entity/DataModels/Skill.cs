using System;
using System.Collections.Generic;

namespace Pattern.Entity.DataModels
{
    public partial class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool? Status { get; set; }
    }
}
