using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class User
    {
        public User()
        {
            HotelStaffs = new HashSet<HotelStaff>();
            HotelVerifications = new HashSet<HotelVerification>();
            Hotels = new HashSet<Hotel>();
            Reservations = new HashSet<Reservation>();
            WalletHistoryPayeeNavigations = new HashSet<WalletHistory>();
            WalletHistoryPayerNavigations = new HashSet<WalletHistory>();
            Wallets = new HashSet<Wallet>();
        }

        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Image { get; set; }
        public string? IdentificationNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public Guid? RoleId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<HotelStaff> HotelStaffs { get; set; }
        public virtual ICollection<HotelVerification> HotelVerifications { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<WalletHistory> WalletHistoryPayeeNavigations { get; set; }
        public virtual ICollection<WalletHistory> WalletHistoryPayerNavigations { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
