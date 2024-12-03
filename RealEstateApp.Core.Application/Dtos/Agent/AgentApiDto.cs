
namespace RealEstateApp.Core.Application.Dtos.Agent
{
    public class AgentApiDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfProperties { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public bool IsActive { get; set; }
    }
}
