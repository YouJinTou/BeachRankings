namespace BR.Core.Models
{
    public class CreateUserResult
    {
        public CreateUserResult()
        {
            this.Message = "Registration successful. Please login.";
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}
