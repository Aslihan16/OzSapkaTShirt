using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OzSapkaTShirt.Data;
using OzSapkaTShirt.Models;

namespace OzSapkaTShirt.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GetReportsController : Controller
    {
        private readonly ApplicationContext _context;
        public class ReportModel
        {
            public DateTime? Start { get; set; }
            public DateTime? End { get; set; }
            public string? UserId { get; set; }
            public long? ProductId { get; set; }
        }

        public GetReportsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Admin/GetReports
        public async Task<IActionResult> Index(string userId=null)
        {
            SelectList reportu;
            SelectList reportp;
            reportu = new SelectList(_context.Users,"Id","Name");
            reportp = new SelectList(_context.Products, "Id", "Name");

            ViewData["ReportUser"] = reportu;
            ViewData["ReportProduct"] = reportp;

            return View();
        }

        public async Task<IActionResult> Report([Bind("Start,End,UserId,ProductId")] ReportModel reportModel)
        {
            IQueryable<Order> orders = _context.Orders;
            if (reportModel.Start != null)
            {
                orders = orders.Where(o => o.OrderDate >= reportModel.Start.Value);
            }
            if (reportModel.End != null)
            {
                orders = orders.Where(o => o.OrderDate <= reportModel.End.Value);
            }
            if (reportModel.UserId != null)
            {
                orders = orders.Where(o => o.CustomerId == reportModel.UserId);
            }
            //if (reportModel.ProductId != null)
            //{
            //    orders = orders.Where(o => o.UserId == reportModel.UserId);
            //}
            orders = orders.Include(o => o.OrderProducts).ThenInclude(op => op.Product);
            return View(orders.ToList());
        }
    }
}
