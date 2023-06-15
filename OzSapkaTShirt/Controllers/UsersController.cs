using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OzSapkaTShirt.Data;
using OzSapkaTShirt.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace OzSapkaTShirt.Controllers
{
    public class ForgetModel
    {
        [DisplayName("E-posta")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(256, MinimumLength = 5, ErrorMessage = "En fazla 256, en az 5 karakter")]
        [EmailAddress(ErrorMessage = "Geçersiz format")]
        public string EMail { get; set; }
    }
    public class ResetModel
    {
        [Required]
        [StringLength(256, MinimumLength = 5)]
        [EmailAddress]
        public string EMail { get; set; }

        [Required]
        public string Code { get; set; }

        [DisplayName("Parola")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "En fazla 128, en az 8 karakter")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [DisplayName("Parola (tekrar)")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "En fazla 128, en az 8 karakter")]
        [DataType(DataType.Password)]
        [Compare("PassWord", ErrorMessage = "Parola eşleşme başarısız")]
        public string ConfirmPassWord { get; set; }
    }
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;


        public UsersController(UserManager<ApplicationUser> userManager, ApplicationContext context, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _emailSender = emailSender;

        }

        // GET: Userss/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            ApplicationUser? user;

            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            user = await _userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            SelectList genders = new SelectList(_context.Genders, "Id", "Name");
            SelectList citys = new SelectList(_context.Citys, "PlateCode", "Name");

            ViewData["Genders"] = genders;
            ViewData["Citys"] = citys;
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SurName,Corporate,Address,Gender,BirthDate,UserName,City,Email,PhoneNumber,PassWord,ConfirmPassWord")] ApplicationUser user)
        { 
            IdentityResult? identityResult;

            if (ModelState.IsValid)
            {
                identityResult = _userManager.CreateAsync(user, user.PassWord).Result;
                if (identityResult == IdentityResult.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("PassWord", "Geçersiz şifre");
            }
            SelectList genders = new SelectList(_context.Genders, "Id", "Name");
            SelectList citys = new SelectList(_context.Citys, "PlateCode", "Name");

            ViewData["Genders"]= genders;
            ViewData["Citys"]= citys;
            return View(user);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {

            SelectList genders;
            SelectList citys;
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            genders = new SelectList(_context.Genders, "Id", "Name", user.Gender);
            citys = new SelectList(_context.Citys, "PlateCode", "Name", user.CityCode);
            ViewData["Genders"] = genders;
            ViewData["Citys"]=citys;

            return View(user.Trim());
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,SurName,Corporate,Address,Gender,BirthDate,UserName,City,Email,PhoneNumber")] ApplicationUser user)
        {
            IdentityResult? identityResult;
            SelectList genders;
            SelectList citys;
            ApplicationUser existingUser;



            if (id != user.Id)
            {
                return NotFound();
            }
            

            ModelState["PassWord"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            ModelState["ConfirmPassWord"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            if (ModelState.IsValid)
            {

                try
                {
                    existingUser = _userManager.FindByIdAsync(id).Result;
                    existingUser.Name = user.Name;
                    existingUser.SurName = user.SurName;
                    existingUser.Corporate = user.Corporate;
                    existingUser.Address = user.Address;
                    existingUser.Gender = user.Gender;
                    existingUser.BirthDate = user.BirthDate;
                    existingUser.UserName = user.UserName;
                    existingUser.Email = user.Email;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.CityCode = user.CityCode;
                    identityResult = _userManager.UpdateAsync(existingUser).Result;
                    if (identityResult == IdentityResult.Success)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    foreach (IdentityError error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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

            genders = new SelectList(_context.Genders, "Id", "Name", user.Gender);
            citys = new SelectList(_context.Citys, "PlateCode", "Name", user.City);


            ViewData["Genders"] = genders;
            ViewData["Citys"] = citys;

            return View(user);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.Include(u => u.GenderType).Include(u => u.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_userManager.Users == null)
            {
                return Problem("Entity set 'ApplicationContext.Product'  is null.");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return (_userManager.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,PassWord")] ApplicationUser user)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult;

            if (ModelState["UserName"].ValidationState == ModelValidationState.Valid)
            {
                if (ModelState["PassWord"].ValidationState == ModelValidationState.Valid)
                {
                    signInResult = _signInManager.PasswordSignInAsync(user.UserName, user.PassWord, false, false).Result;
                    if (signInResult.Succeeded == true)
                    {
                        return RedirectToAction("Index","Home");
                    }


                }

            }

            return View(user);

        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Users/ChangePassword
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string passWord, string newPassword)
        {
            string userIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ApplicationUser existingUser = _userManager.FindByIdAsync(userIdentity).Result;
            IdentityResult identityResult;
            existingUser.PassWord = passWord;
            existingUser.UserName=existingUser.UserName.Trim();

            identityResult = await _userManager.ChangePasswordAsync(existingUser, passWord , newPassword).ConfigureAwait(false);
            //identity result success'se şifre değişti aksi halde tekrar şifre değiştirme ekranı

            if (identityResult.Succeeded == true)
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }

        public ViewResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult ForgetPassword(ForgetModel forgetModel)
        {
            ApplicationUser applicationUser;
            string resetToken;
            if(ModelState.IsValid)
            {
                applicationUser = _userManager.FindByEmailAsync(forgetModel.EMail).Result;
                    if(applicationUser!= null) 
                {
                    resetToken = _userManager.GeneratePasswordResetTokenAsync(applicationUser).Result;
                    ////send resetToken to forgetModel.Emaiş
                    ViewData["eMail"] = forgetModel.EMail;
                    return View("ResetPassword");
                }
                
            }
            return View();
        }

        public ViewResult ResetPassword(ResetModel resetModel)
        {
            ApplicationUser applicationUser;
            if(ModelState.IsValid)
            {
                applicationUser = _userManager.FindByEmailAsync(resetModel.EMail).Result;
                _userManager.ResetPasswordAsync(applicationUser, resetModel.Code,resetModel.PassWord);
            }
            return View();
        }
    }
}
