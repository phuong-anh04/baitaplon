namespace baitaplon.Models
{
    
    public class CheckoutViewModel
    {
        public int UserId { get; set; }  // ID của người dùng
        public string? PaymentType { get; set; }  // Loại thanh toán

        // Các thông tin khác về người dùng trong quá trình thanh toán
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

    

}
