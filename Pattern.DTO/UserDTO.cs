namespace Pattern.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; }  = null!;
        public string Role { get; set; }  = null!;
        public string Position { get; set; }  = null!;
    }
}
