
using RealEstateApp.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.Offer
{
    public class OfferSaveViewModel
    {
        public string UserId { get; set; }
        public int PropertyId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public OfferStatus Status { get; set; } = OfferStatus.Pending;
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
