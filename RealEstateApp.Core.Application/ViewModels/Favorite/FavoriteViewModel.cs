
namespace RealEstateApp.Core.Application.ViewModels.Favorite
{
    public class FavoriteViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PropertyId { get; set; }
        public DateTime MarkedDate { get; set; }
    }
}
