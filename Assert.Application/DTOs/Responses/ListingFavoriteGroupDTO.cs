using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class ListingFavoriteGroupDTO
    {
        public long FavoriteGroupListingId { get; set; }

        public int UserId { get; set; }

        public string FavoriteGroupName { get; set; } = null!;

        public DateTime? CreationDate { get; set; }

        public short GroupStatus { get; set; }

        public virtual ICollection<ListingFavoriteDTO> TlListingFavorites { get; set; } = new List<ListingFavoriteDTO>();

    }
    public partial class ListingFavoriteDTO
    {
        public long FavoriteListingId { get; set; }

        public long ListingRentId { get; set; }

        public int UserId { get; set; }

        public DateTime? CreateAt { get; set; }

        public long? FavoriteGroupId { get; set; }

        public virtual ListingRentDTO ListingRent { get; set; } = null!;
    }
}
