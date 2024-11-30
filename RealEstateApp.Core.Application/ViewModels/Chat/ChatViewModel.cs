
namespace RealEstateApp.Core.Application.ViewModels.Chat
{
    public class ChatViewModel
    {
        public int PropertyId { get; set; }
        public List<ChatMessageViewModel> Messages { get; set; } = new List<ChatMessageViewModel>();
    }
}
