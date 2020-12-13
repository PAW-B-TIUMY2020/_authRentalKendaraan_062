using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalKendaraan_062.Models
{
    public partial class Pengembalian
    {
        public int IdPengembalian { get; set; }

        [Required(ErrorMessage = "Tanggal Wajib Diisi!")]
        public DateTime? TglPengembalian { get; set; }


        public int? Denda { get; set; }
        public int? IdPeminjaman { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Hanya boleh diisi angka")]
        public int? IdKondisi { get; set; }

        public KondisiKendaraan IdKondisiNavigation { get; set; }
        public Peminjaman IdPeminjamanNavigation { get; set; }
        //public Pengembalian IdPengembalianNavigation { get; set; }
    }
}
