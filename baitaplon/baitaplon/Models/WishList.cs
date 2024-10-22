using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace baitaplon.Models
{
    public class Wishlist
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }  // ID của tài khoản (người dùng)

        public virtual Account Account { get; set; } // Đối tượng Account

        [ForeignKey("Product")]
        public int ProductId { get; set; }  // ID của sản phẩm

        public virtual Product Product { get; set; } // Đối tượng Product
    }
}
