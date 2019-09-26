using Microsoft.AspNetCore.Mvc;

namespace AgileVentureFirenzePoll.Areas.Controllers
{
    public class VotazioniController : Controller
    {
        public IActionResult Vota(string key)
        {
            
            return View("Bottoni",key);
        }
    }
}