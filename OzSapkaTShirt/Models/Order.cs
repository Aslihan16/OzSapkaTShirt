using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OzSapkaTShirt.Models
{
    public class Order
    {
        public long OrderId { get; set; }

        [DisplayName("Müşteri")]
        public string CustomerId { get; set; }

        [DisplayName("Sipariş Tarihi")]
        public DateTime OrderDate { get; set; }

        [DisplayName("Sipariş Teslim Tarihi")]
        public DateTime? CompletionDate { get; set; }

        [DisplayName("Ödeme Tipi")]
        public byte PaymentType { get; set; }

        [DisplayName("Müşteri")]
        [ForeignKey("CustomerId")]
        public ApplicationUser? User { get; set; }

        [DisplayName("İndirim")]
        public float? Discount { get; set; }

        [DisplayName("Toplam Fiyat")]
        public float PaymentTotal { get; set; }

        [DisplayName("Kargo Şirketi")]
        public int CargoCompany { get; set; }

        [DisplayName("Kargo Tutarı")]
        public float ShipmentPrice { get; set; }

        [DisplayName("Durum")]
        public byte Status { get; set; }

        public List<OrderProduct>? OrderProducts { get; set; }
    }
}
