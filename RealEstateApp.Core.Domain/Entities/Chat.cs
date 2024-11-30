using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Chat : AuditableBaseEntity
    {
        public string? UserId { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public string Message { get; set; }
        public bool IsAgent { get; set; }
        public DateTime SendDate { get; set; }

        public string? ReceiverId { get; set; }


    }
}
