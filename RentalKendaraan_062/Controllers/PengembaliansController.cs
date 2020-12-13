using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalKendaraan_062.Models;

namespace RentalKendaraan_062.Controllers
{
    public class PengembaliansController : Controller
    {
        private readonly RentalkendaraanContext _context;

        public PengembaliansController(RentalkendaraanContext context)
        {
            _context = context;
        }

        // GET: Pengembalians
        public async Task<IActionResult> Index(string ktsd, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            //buat list untuk menyimpan ketersediaan
            var ktsdList = new List<string>();

            //query mengambil data
            var ktsdQuery = from d in _context.Pengembalian orderby d.TglPengembalian select d.TglPengembalian.ToString();

            ktsdList.AddRange(ktsdQuery.Distinct());

            //untuk menampilkan di view
            ViewBag.ktsd = new SelectList(ktsdList);

            //panggil db context
            var menu = from m in _context.Pengembalian.Include(p => p.IdKondisiNavigation).Include(p => p.IdPeminjamanNavigation) select m;

            //untuk memilih dropdownlist ketersediaan
            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.TglPengembalian.ToString() == ktsd);
            }

            //untuk search data
            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.Denda.ToString().Contains(searchString));
            }

            //mmebuat pagedlist
            ViewData["CurrentShort"] = sortOrder;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            //definisi jumlah data pada halaman
            int pageSize = 5;

            //untuk sorting
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.IdKondisiNavigation.NamaKondisi);
                    break;
                case "Date":
                    menu = menu.OrderBy(s => s.TglPengembalian);
                    break;
                case "date_desc":
                    menu = menu.OrderByDescending(s => s.TglPengembalian);
                    break;
                default: //name ascending
                    menu = menu.OrderBy(s => s.IdKondisiNavigation.NamaKondisi);
                    break;
            }

            return View(await PaginatedList<Pengembalian>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

       

        // GET: Pengembalians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalian
                .Include(p => p.IdKondisiNavigation)
                .Include(p => p.IdPeminjamanNavigation)
                .FirstOrDefaultAsync(m => m.IdPengembalian == id);
            if (pengembalian == null)
            {
                return NotFound();
            }

            return View(pengembalian);
        }

        // GET: Pengembalians/Create
        public IActionResult Create()
        {
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjaman, "IdPeminjaman", "IdPeminjaman");
            ViewData["IdPengembalian"] = new SelectList(_context.KondisiKendaraan, "IdKondisi", "IdKondisi");
            return View();
        }

        // POST: Pengembalians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPengembalian,TglPengembalian,IdPeminjaman,IdKondisi,Denda")] Pengembalian pengembalian)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pengembalian);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjaman, "IdPeminjaman", "IdPeminjaman", pengembalian.IdPeminjaman);
            ViewData["IdPengembalian"] = new SelectList(_context.KondisiKendaraan, "IdKondisi", "IdKondisi", pengembalian.IdPengembalian);
            return View(pengembalian);
        }

        // GET: Pengembalians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalian.FindAsync(id);
            if (pengembalian == null)
            {
                return NotFound();
            }
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjaman, "IdPeminjaman", "IdPeminjaman", pengembalian.IdPeminjaman);
            ViewData["IdPengembalian"] = new SelectList(_context.KondisiKendaraan, "IdKondisi", "IdKondisi", pengembalian.IdPengembalian);
            return View(pengembalian);
        }

        // POST: Pengembalians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPengembalian,TglPengembalian,IdPeminjaman,IdKondisi,Denda")] Pengembalian pengembalian)
        {
            if (id != pengembalian.IdPengembalian)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pengembalian);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PengembalianExists(pengembalian.IdPengembalian))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjaman, "IdPeminjaman", "IdPeminjaman", pengembalian.IdPeminjaman);
            ViewData["IdPengembalian"] = new SelectList(_context.KondisiKendaraan, "IdKondisi", "IdKondisi", pengembalian.IdPengembalian);
            return View(pengembalian);
        }

        // GET: Pengembalians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalian
                .Include(p => p.IdPeminjamanNavigation)
                .Include(p => p.IdKondisiNavigation)
                .FirstOrDefaultAsync(m => m.IdPengembalian == id);
            if (pengembalian == null)
            {
                return NotFound();
            }

            return View(pengembalian);
        }

        // POST: Pengembalians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pengembalian = await _context.Pengembalian.FindAsync(id);
            _context.Pengembalian.Remove(pengembalian);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PengembalianExists(int id)
        {
            return _context.Pengembalian.Any(e => e.IdPengembalian == id);
        }
    }
}
