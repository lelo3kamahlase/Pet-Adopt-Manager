using u24753328_HW02.Models;
using System.Web.Mvc;
using System.Linq;
using static u24753328_HW02.Data.IPetRespository;

public class HomeController : Controller
{
    private readonly IPetRepository _repository;

    public HomeController(IPetRepository repository)
    {
        _repository = repository;
    }

    public ActionResult Index()
    {
        ViewBag.PetsHomedCount = _repository.GetPetsHomedCount();
        var recentAdoptions = _repository.GetRecentAdoptions();

        return View(recentAdoptions);
    }
}