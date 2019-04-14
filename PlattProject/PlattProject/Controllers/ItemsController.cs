using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlattProject.Models;

namespace PlattProject.Controllers
{
    public class ItemsController : Controller
    {
        private readonly PlattContext _context;

        public ItemsController(PlattContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {            
            return View(await _context.Items.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Cost,Weight")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Cost,Weight")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.SingleOrDefaultAsync(m => m.Id == id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }

        private void PopulateItemsTable()
        {
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('Medium Voltage Cable', '15kV, 1/C, 2 AWG, Copper MV-105 Power Distribution Cable, With Copper-Tape Shield & PVC Jacket', 5, 10)");
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('Halogen Lamp', '43W, 120V, Bulb: A19, Base: Medium Screw (E26), Lumens: 750', 2, 1)");
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('Hammer Drill', '20V, Cordless, RPM: 0 - 500/0 - 1700, BPM: 0 - 8500/0 - 29000, Max Power: 820 Unit Watts Out', 199, 5)");
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('30 Amp Fuse', 'Rejection, Brand or Series: Class RK5, Interrupting Rating: 200000 Amp VAC RMS Symmetrical, 20000 Amp VDC', 10, 1)");
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('1/2 Inch EMT Conduit', 'Material: Hot-Galvanized Steel, Length: 10 feet, Color: Metallic.', 4, 25)");
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('Finished Hex Nut', 'Size: 3/8-16 Inch.Zinc Plated Steel.Package Quantity: 50', 1, 1)");


        }
    }
}
