using u24753328_HW02.Models;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using static u24753328_HW02.Data.IPetRespository;

public class PetsController : Controller
{
    private readonly IPetRepository _repository;

    public PetsController(IPetRepository repository)
    {
        _repository = repository;
    }

    public ActionResult Index(string type, string breed, string location)
    {
        
        ViewBag.Types = new SelectList(_repository.GetDistinctTypes().Prepend("All"), type);

        ViewBag.Breeds = new SelectList(_repository.GetDistinctBreeds(type).Prepend("All"), breed);

        ViewBag.Locations = new SelectList(_repository.GetDistinctLocations().Prepend("All"), location);

        IEnumerable<PET> pets = _repository.GetPets(type, breed, location);

        ViewBag.SuccessMessage = TempData["SuccessMessage"];

        return View(pets);
    }
    public JsonResult GetBreedsByType(string type)
    {
        var breeds = _repository.GetDistinctBreeds(type);
        return Json(breeds, JsonRequestBehavior.AllowGet);
    }
}