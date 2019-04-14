using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlattProject.Models;
using System.Data.SqlClient;

namespace PlattProject.Controllers
{
    public class ItemStocksController : Controller
    {
        private readonly PlattContext _context;

        public ItemStocksController(PlattContext context)
        {
            _context = context;
        }

        // GET: ItemStocks
        public async Task<IActionResult> Index()
        {
            var plattContext = _context.ItemStocks.Include(i => i.Item).Include(i => i.Warehouse);
            return View(await plattContext.ToListAsync());
        }

        // GET: ItemStocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemStock = await _context.ItemStocks
                .Include(i => i.Item)
                .Include(i => i.Warehouse)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (itemStock == null)
            {
                return NotFound();
            }

            return View(itemStock);
        }

        // GET: ItemStocks/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Id");
            return View();
        }

        // POST: ItemStocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,WarehouseId,ItemCount")] ItemStock itemStock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemStock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", itemStock.ItemId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Id", itemStock.WarehouseId);
            return View(itemStock);
        }

        // GET: ItemStocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemStock = await _context.ItemStocks.SingleOrDefaultAsync(m => m.Id == id);
            if (itemStock == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", itemStock.ItemId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Id", itemStock.WarehouseId);
            return View(itemStock);
        }

        // POST: ItemStocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemId,WarehouseId,ItemCount")] ItemStock itemStock)
        {
            if (id != itemStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemStock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemStockExists(itemStock.Id))
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
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", itemStock.ItemId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Id", itemStock.WarehouseId);
            return View(itemStock);
        }

        // GET: ItemStocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemStock = await _context.ItemStocks
                .Include(i => i.Item)
                .Include(i => i.Warehouse)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (itemStock == null)
            {
                return NotFound();
            }

            return View(itemStock);
        }

        // POST: ItemStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemStock = await _context.ItemStocks.SingleOrDefaultAsync(m => m.Id == id);
            _context.ItemStocks.Remove(itemStock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemStockExists(int id)
        {
            return _context.ItemStocks.Any(e => e.Id == id);
        }

        public void PopulateItemStocksTable()
        {
            var Random = new Random();
            var Warehouses = _context.Warehouses;
            var Items = _context.Items;
            foreach (var Warehouse in Warehouses)
            {
                var WarehouseId = Warehouse.Id;
                foreach (var Item in Items)
                {
                    var RandomNumber = Random.Next(1, 200);
                    var ItemId = Item.Id;
                    string sqlCommand = "INSERT ItemStocks (ItemId, WarehouseId, ItemCount) VALUES (@ItemId, @WarehouseId, @ItemCount)";
                    var SqlParamItemId = new SqlParameter("@ItemId", ItemId);
                    var SqlParamWarehouseId = new SqlParameter("@WarehouseId", WarehouseId);
                    var SqlParamItemCount = new SqlParameter("@ItemCount", RandomNumber);
                    _context.Database.ExecuteSqlCommand(sqlCommand, SqlParamItemId, SqlParamWarehouseId, SqlParamItemCount);

                }
            }
        }
    }
}
