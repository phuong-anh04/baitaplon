using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace baitaplon.Models;

	public partial class Cart
	{
		[Key]
		public int Id { get; set; }

		public int? Uid { get; set; } // Liên kết đến bảng Account
		public int? Pid { get; set; } // Liên kết đến bảng Product
		public int? Quantity { get; set; }

		// Thiết lập mối quan hệ với bảng Account và Product
		[ForeignKey("Uid")]
		public virtual Account Account { get; set; }

		[ForeignKey("Pid")]
		public virtual Product Product { get; set; }
	}

