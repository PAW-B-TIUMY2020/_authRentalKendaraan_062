using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RentalKendaraan_062.Models
{
    public partial class Peminjaman
    {
        public Peminjaman()
        {
            Pengembalian = new HashSet<Pengembalian>();
        }
        [DisplayName("ID")]
        public int IdPeminjaman { get; set; }

        [DisplayName("Tanggal Peminjaman")]
        [Required(ErrorMessage = "Tanggal Wajib Diisi!")]
        public DateTime? TglPeminjaman { get; set; }

        [DisplayName("Biaya")]
        [Required(ErrorMessage = "Biaya tidak boleh kosong")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Hanya Boleh Diisi oleh Angka!")]
        public int? Biaya { get; set; }

        [DisplayName("Costumer")]
        [Required(ErrorMessage = "ID Costumer tidak boleh kosong")]
        public int? IdCustomer { get; set; }


        [DisplayName("Kendaraan")]
        [Required(ErrorMessage = "ID Kendaraan tidak boleh kosong")]
        public int? IdKendaraan { get; set; }

        [DisplayName("Jaminan")]
        [Required(ErrorMessage = "ID Jaminan tidak boleh kosong")]
        public int? IdJaminan { get; set; }

        [DisplayName("Customer")]
        public Customer IdCustomerNavigation { get; set; }

        [DisplayName("Jaminan")]
        public Jaminan IdJaminanNavigation { get; set; }

        [DisplayName("Kendaraan")]
        public Kendaraan IdKendaraanNavigation { get; set; }
        public ICollection<Pengembalian> Pengembalian { get; set; }
    }
}
