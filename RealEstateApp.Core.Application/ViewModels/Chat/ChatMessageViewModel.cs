namespace RealEstateApp.Core.Application.ViewModels.Chat
{
    public class ChatMessageViewModel
    {
        public string UserId { get; set; }
        public int PropertyId { get; set; }
        public string Message { get; set; }
        public bool IsAgent { get; set; }
        public DateTime SendDate { get; set; }
    }
}
