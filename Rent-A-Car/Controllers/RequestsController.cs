using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Rent_A_Car.Data;
using Rent_A_Car.Models;
using Rent_A_Car.ViewModels;
using System.Security.Principal;

namespace Rent_A_Car.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            RequestsIndexViewModel data = new RequestsIndexViewModel();
            if(User.IsInRole("Administrator"))
            {
                data.AllRequests = await _context.Requests.Include(r => r.Car).Include(r => r.User).ToListAsync();
                return View(data);
            }

            else
            {
                var userRequests = _context.Requests.Where(r => r.UserID == GetUserId()).Include(r => r.Car).Include(r => r.User);
                data.ApprovedRequests = await userRequests.Where(x => x.IsApproved).ToListAsync();
                data.UnpprovedRequests = await userRequests.Where(x => !x.IsApproved).ToListAsync();
                return View(data);
            }
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        // GET: Requests/Create
        public IActionResult Create(int? carId)
        {
            if (carId.HasValue)
            {
                ViewData["CarID"] = new SelectList(_context.Cars, "Id", "Id", carId);
            }
            else
            {
                ViewData["CarID"] = new SelectList(_context.Cars, "Id", "Id");
            }

            ViewData["UserID"] = GetUserId();
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequestsViewModel requestModel)
        {
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            var allRequestedCars = _context.Requests.Where(x => x.CarID == requestModel.CarID).ToList();
            var isBookedCar = allRequestedCars.Any(x => x.RequestEnd >= requestModel.RequestStart && x.RequestStart <= requestModel.RequestStart || x.RequestEnd>=requestModel.RequestEnd && x.RequestStart <=requestModel.RequestEnd);
            
            if (!isBookedCar && requestModel.RequestStart >= now)
            {

                if (ModelState.IsValid)
                {
                    var request = new Requests();
                    request.RequestStart = requestModel.RequestStart;
                    request.RequestEnd = requestModel.RequestEnd;
                   
                    request.UserID = GetUserId();
                    request.CarID = requestModel.CarID;

                    _context.Add(request);
                    await _context.SaveChangesAsync();

                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }

            //ViewData["CarID"] = new SelectList(_context.Cars, "Id", "Id", requestModel.CarID);

        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests.FindAsync(id);
            if (requests == null)
            {
                return NotFound();
            }
            ViewData["CarID"] = new SelectList(_context.Cars, "Id", "Brand", requests.CarID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", requests.UserID);
            return View(requests);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarID,UserID,RequestStart,RequestEnd")] Requests requests)
        {
            if (id != requests.Id)
            {
                return NotFound();
            }

            var now = DateTime.Now;
            var allRequestedCars = _context.Requests.Where(x => x.CarID == requests.CarID).ToList();
            var isBookedCar = allRequestedCars.Any(x => x.RequestEnd >= requests.RequestStart && x.RequestStart <= requests.RequestStart || x.RequestEnd >= requests.RequestEnd && x.RequestStart <= requests.RequestEnd);

            if (!isBookedCar && requests.RequestStart >= now)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(requests);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RequestsExists(requests.Id))
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
            }
            else
            {
                return View("Error");
            }
            ViewData["CarID"] = new SelectList(_context.Cars, "Id", "Brand", requests.CarID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", requests.UserID);
            return View(requests);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Requests'  is null.");
            }
            var requests = await _context.Requests.FindAsync(id);
            if (requests != null)
            {
                _context.Requests.Remove(requests);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestsExists(int id)
        {
            return (_context.Requests?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public ActionResult ApproveRejectRequest(int requestId)
        {
            // Retrieve the request from the database based on the provided request id

            var request = _context.Requests.FirstOrDefault(x => x.Id == requestId);

            if (request == null)
            {
                // If the request does not exist, return a HttpNotFoundResult
                return NotFound();
            }
            else
            {
                request.IsApproved= true;
                // Save changes to the database
                _context.SaveChanges();

                // Redirect to the index page for the Requests controller
                return RedirectToAction("Index", "Requests");
            }
        }

        private string GetUserId()
        {
            var name = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(x => x.Email == name);

            return user.Id;
        }

    }
}
