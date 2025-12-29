using System.ComponentModel.DataAnnotations;

namespace luckyoneApiv3.Models
{
    public class PointsModel
    {



        public class PointsBalanceDTO
        {
            public decimal Balance { get; set; }
        }

        public class PointsTransactionDTO
        {
            public string Id { get; set; }
            public string UserId { get; set; }
            public decimal Amount { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public string? ContestId { get; set; }
            public string? ContestTitle { get; set; }
            public string? ReferenceId { get; set; }
            public string? PaymentMethod { get; set; }
            public DateTime CreatedAt { get; set; }
        }


        public class AddPointsRequest
        {
            [Required]
            [Range(100, 100000)]
            public decimal Amount { get; set; }

            [Required]
            public string PaymentMethod { get; set; } // credit_card, paypal, stripe, bank_transfer

            public string? TransactionId { get; set; }
        }






    }
}
