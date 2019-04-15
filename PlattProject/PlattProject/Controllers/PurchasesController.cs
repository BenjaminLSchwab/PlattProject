using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlattProject.Models;
using PlattProject.ViewModels;

namespace PlattProject.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly PlattContext _context;

        public PurchasesController(PlattContext context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            var plattContext = _context.Purchases.Include(p => p.Item).Include(p => p.User);
            return View(await plattContext.ToListAsync());
        }

        // GET: Purchases/Manage
        public ActionResult Manage()
        {
            return View();
        }

        // GET: Purchases/Analytics
        public ActionResult Analytics()
        {
            var model = new AnalyticsVm();

            var bestCustomers = new List<BestCustomerVm>();
            foreach (var purchase in _context.Purchases)
            {
                var item = _context.Items.Find(purchase.ItemId);
                _context.Users.Find(purchase.UserId);

                //if user is already on list, add purchase stats to their entry
                if (bestCustomers.Where(c => c.UserId == purchase.UserId).Count() > 0)
                {
                    var customer = bestCustomers.Where(c => c.UserId == purchase.UserId).FirstOrDefault();
                    customer.NumberOfOrders++;
                    customer.AmountSpent += item.Cost * purchase.NumberPurchased;
                }
                else
                {
                    var customer = new BestCustomerVm();
                    customer.UserId = purchase.UserId;
                    customer.Name = purchase.User.Name;
                    customer.Email = purchase.User.Email;
                    customer.AmountSpent = item.Cost * purchase.NumberPurchased;
                    customer.NumberOfOrders = 1;
                    bestCustomers.Add(customer);
                }
            }
            model.BestCustomerVms = bestCustomers.OrderBy(x => x.AmountSpent).Reverse().ToList();

            var topSellingItems = new List<ItemsSoldVm>();
            foreach (var purchase in _context.Purchases)
            {
                var PurchaseItem = _context.Items.Find(purchase.ItemId);
                if (topSellingItems.Where(i => i.Id == purchase.ItemId).Count() > 0)
                {
                    var item = topSellingItems.Where(i => i.Id == purchase.ItemId).FirstOrDefault();
                    item.NumberSold += purchase.NumberPurchased;
                }
                else
                {
                    var item = new ItemsSoldVm();
                    item.Id = purchase.ItemId;
                    item.Name = PurchaseItem.Name;
                    item.Cost = PurchaseItem.Cost;
                    item.Description = PurchaseItem.Description;
                    item.NumberSold = purchase.NumberPurchased;
                    topSellingItems.Add(item);
                }
            }
            model.ItemsSoldVms = topSellingItems.OrderBy(x => x.NumberSold).Reverse().ToList();

            var largestOrders = new List<LargestOrderVm>();
            foreach (var purchase in _context.Purchases)
            {
                var item = _context.Items.Find(purchase.ItemId);
                var user = _context.Users.Find(purchase.UserId);
                var order = new LargestOrderVm();
                order.ItemName = item.Name;
                order.Purchase = purchase;
                order.UserName = user.Name;
                largestOrders.Add(order);
            }
            model.LargestOrders = largestOrders.OrderBy(x => x.Purchase.NumberPurchased).Reverse().ToList();

            var itemsNeedRestock = new List<ItemRestockVm>();
            foreach (var itemStock in _context.ItemStocks)
            {
                var item = _context.Items.Find(itemStock.ItemId);
                var warehouse = _context.Warehouses.Find(itemStock.WarehouseId);
                var itemRestock = new ItemRestockVm();
                itemRestock.ItemStock = itemStock;
                itemRestock.ItemName = item.Name;
                itemRestock.WarehouseAddress = warehouse.Address;
                itemsNeedRestock.Add(itemRestock);
            }
            model.ItemRestocks = itemsNeedRestock.OrderBy(x => x.ItemStock.ItemCount).ToList();

            return View(model);
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Item)
                .Include(p => p.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchases/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,UserId,NumberPurchased,PurchaseDate,ShipDate,WarehouseId")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", purchase.ItemId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", purchase.UserId);
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases.SingleOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", purchase.ItemId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", purchase.UserId);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemId,UserId,NumberPurchased,PurchaseDate,ShipDate,WarehouseId")] Purchase purchase)
        {
            if (id != purchase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(purchase.Id))
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
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", purchase.ItemId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", purchase.UserId);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Item)
                .Include(p => p.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchases.SingleOrDefaultAsync(m => m.Id == id);
            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public PartialViewResult BestCustomers()
        {
            var Customers = new List<BestCustomerVm>();
            var billy = new BestCustomerVm();
            billy.UserId = 1;
            billy.Email = "a";
            billy.Name = "Billy";
            billy.AmountSpent = 1;
            billy.NumberOfOrders = 1;

            Customers.Add(billy);

            return PartialView("_BestCustomers",Customers);
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchases.Any(e => e.Id == id);
        }

        public ActionResult PopulateAllTables()
        {
            PopulateUsersTable();
            PopulateItemsTable();
            PopulateWarehousesTable();
            PopulateItemStocksTable();
            PopulatePurchasesTable();

            return Redirect("/Purchases/Analytics");

        }

        public ActionResult ClearAllTables()
        {
            _context.Database.ExecuteSqlCommand("DELETE FROM Users");
            _context.Database.ExecuteSqlCommand("DELETE FROM Items");
            _context.Database.ExecuteSqlCommand("DELETE FROM Warehouses");
            _context.Database.ExecuteSqlCommand("DELETE FROM ItemStocks");
            _context.Database.ExecuteSqlCommand("DELETE FROM Purchases");

            return Redirect("/Purchases/Manage");
        }

        public void PopulateItemsTable()
        {
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('Medium Voltage Cable', '15kV, 1/C, 2 AWG, Copper MV-105 Power Distribution Cable, With Copper-Tape Shield & PVC Jacket', 5, 10)");
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('Halogen Lamp', '43W, 120V, Bulb: A19, Base: Medium Screw (E26), Lumens: 750', 2, 1)");
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('Hammer Drill', '20V, Cordless, RPM: 0 - 500/0 - 1700, BPM: 0 - 8500/0 - 29000, Max Power: 820 Unit Watts Out', 199, 5)");
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('30 Amp Fuse', 'Rejection, Brand or Series: Class RK5, Interrupting Rating: 200000 Amp VAC RMS Symmetrical, 20000 Amp VDC', 10, 1)");
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('1/2 Inch EMT Conduit', 'Material: Hot-Galvanized Steel, Length: 10 feet, Color: Metallic.', 4, 25)");
            _context.Database.ExecuteSqlCommand("INSERT Items (Name, Description, Cost, Weight) VALUES ('Finished Hex Nut', 'Size: 3/8-16 Inch.Zinc Plated Steel.Package Quantity: 50', 1, 1)");
        }

        public void PopulateUsersTable()
        {
            _context.Database.ExecuteSqlCommand("INSERT Users (Email, Name) VALUES ('Ben@gmail.com', 'Ben Schwab')");
            _context.Database.ExecuteSqlCommand("INSERT Users (Email, Name) VALUES ('Mike@gmail.com', 'Mike Smith')");
            _context.Database.ExecuteSqlCommand("INSERT Users (Email, Name) VALUES ('Sarah@gmail.com', 'Sarah Vanwyhe')");
            _context.Database.ExecuteSqlCommand("INSERT Users (Email, Name) VALUES ('Martin@yahoo.com', 'Martin Goldsmith')");
            _context.Database.ExecuteSqlCommand("INSERT Users (Email, Name) VALUES ('Terry@yahoo.com', 'Terry Weible')");
            _context.Database.ExecuteSqlCommand("INSERT Users (Email, Name) VALUES ('Alexa@yahoo.com', 'Alexa Bezos')");
            _context.Database.ExecuteSqlCommand("INSERT Users (Email, Name) VALUES ('John@hotmail.com', 'John McAllister')");
            _context.Database.ExecuteSqlCommand("INSERT Users (Email, Name) VALUES ('Abby@hotmail.com', 'Abby Marsh')");
            _context.Database.ExecuteSqlCommand("INSERT Users (Email, Name) VALUES ('Dane@hotmail.com', 'Dane Matthews')");
            _context.Database.ExecuteSqlCommand("INSERT Users (Email, Name) VALUES ('Trevor@hotmail.com', 'Trevor Grant')");
        }

        public void PopulateWarehousesTable()
        {
            _context.Database.ExecuteSqlCommand("INSERT Warehouses (Address) VALUES ('8364 N. Holly Ave. Glen Allen, VA 23059')");
            _context.Database.ExecuteSqlCommand("INSERT Warehouses (Address) VALUES ('8666 Joy Ridge St. Amarillo, TX 79106')");
            _context.Database.ExecuteSqlCommand("INSERT Warehouses (Address) VALUES ('8847 Wagon St. Palm Coast, FL 32137')");

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

        public void PopulatePurchasesTable()
        {
            var random = new Random();
            var Items = _context.Items.ToList();
            var Users = _context.Users.ToList();
            var Warehouses = _context.Warehouses.ToList();


            for (int i = 0; i < 30; i++)
            {
                var purchase = new Purchase();

                var randomNum = random.Next(0, Items.Count());
                purchase.ItemId = Items[randomNum].Id;

                randomNum = random.Next(0, Users.Count());
                purchase.UserId = Users[randomNum].Id;

                randomNum = random.Next(0, Warehouses.Count());
                purchase.WarehouseId = Warehouses[randomNum].Id;

                randomNum = random.Next(5, 500);
                var PurchaseDate = DateTime.Now;
                PurchaseDate = PurchaseDate.AddDays(-1 * randomNum);
                purchase.PurchaseDate = PurchaseDate;

                purchase.ShipDate = PurchaseDate.AddDays(4);

                randomNum = random.Next(1, 10);
                purchase.NumberPurchased = randomNum;

                _context.Purchases.Add(purchase);


            }
            _context.SaveChanges();
        }

    }
}
