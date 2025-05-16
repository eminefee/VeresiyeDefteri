using Business.Configurations;
using Business.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
[Authorize]

public class EmailController : Controller
{
    private readonly AppDbContext _context;
    private readonly EmailService _emailService;

    public EmailController(AppDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        var emails = _context.SentEmails
            .Include(e => e.DebtRecord)
            .OrderByDescending(e => e.SentAt)
            .ToList();
        return View(emails);
    }

    public IActionResult Create(int id)
    {
        var record = _context.debtrecords.FirstOrDefault(x => x.Id == id);
        if (record == null) return NotFound();

        var model = new EmailFormViewModel
        {
            ToEmail = record.Email,
            Subject = "Borç Hakkında Bilgilendirme",
            DebtRecordId = id
        };

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Create(EmailFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _emailService.SendEmailAsync(model.ToEmail, model.Subject, model.Body, model.DebtRecordId);
        TempData["Message"] = "Mail başarıyla gönderildi!";
        return RedirectToAction("Index", "Debt");
    }


    // Örnek mail gönderme action
    public async Task<IActionResult> Send(int id)
    {
        var record = _context.debtrecords.FirstOrDefault(x => x.Id == id);
        if (record == null) return NotFound();

        string subject = "Borç Bilgilendirmesi";
        string body = $"Merhaba {record.CustomerName}, borcunuz: {record.Amount} TL.";

        await _emailService.SendEmailAsync(record.Email, subject, body, record.Id);

        return RedirectToAction("Index");
    }
}
