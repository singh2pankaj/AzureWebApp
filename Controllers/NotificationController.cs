using Microsoft.AspNetCore.Mvc;

public class NotificationController : Controller
{
    private readonly ServiceBusSenderService _senderService;

    public NotificationController(ServiceBusSenderService senderService)
    {
        _senderService = senderService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Send(string message)
    {
        await _senderService.SendMessageAsync(message);
        ViewBag.Message = "Message sent to queue!";
        return View("Index");
    }
}