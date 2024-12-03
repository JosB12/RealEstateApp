
using RealEstateApp.Core.Domain.Enums;

namespace RealEstateApp.Core.Application.ViewModels.Offer
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PropertyId { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
        public OfferStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
