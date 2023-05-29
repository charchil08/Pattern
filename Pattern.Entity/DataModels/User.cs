using System;
using System.Collections.Generic;

namespace Pattern.Entity.DataModels
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Position { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
