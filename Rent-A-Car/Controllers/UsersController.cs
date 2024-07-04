using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.Data;
using Rent_A_Car.Models;

namespace Rent_A_Car.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Users'  is null.");
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var Users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Users == null)
            {
                return NotFound();
            }

            return View(Users);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Password,FirstName,LastName,PIN,PhoneNumber,Email")] ApplicationUser Users)
        {
           
            if (ModelState.IsValid )
            {
                _context.Add(Users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Users);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var User = await _context.Users.FindAsync(id);
            if (User == null)
            {
                return NotFound();
            }
            return View(User);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var editedUser = await _context.Users.FindAsync(user.Id);
            if (editedUser != null)
            {
                if (ModelState.IsValid)
                {
                    editedUser.LastName = user.LastName; editedUser.FirstName=user.FirstName;
                    editedUser.Email = user.Email;
                    editedUser.UserName = user.UserName;
                    editedUser.PhoneNumber = user.PhoneNumber;
                    editedUser.PIN = user.PIN;

                    _context.Update(editedUser);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var Users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Users == null)
            {
                return NotFound();
            }

            return View(Users);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var Users = await _context.Users.FindAsync(id);
            if (Users != null)
            {
                _context.Users.Remove(Users);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
