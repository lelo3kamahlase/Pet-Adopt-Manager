using u24753328_HW02.Models;
using System.Web.Mvc;
using System.Linq;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using static u24753328_HW02.Data.IPetRespository;
using System;


public class AdoptController : Controller
{
    private readonly IPetRepository _repository;

    public AdoptController(IPetRepository repository)
    {
        _repository = repository;
    }

    private void PopulateDropDowns()
    {
        var users = _repository.GetAllUsers().ToList();

        ViewBag.FullNameList = new SelectList(users, "FullName", "FullName");

        var phoneNumbersMap = users.ToDictionary(u => u.FullName, u => u.PhoneNumber);
        var serializer = new JavaScriptSerializer();
        ViewBag.PhoneNumbersJson = serializer.Serialize(phoneNumbersMap);
    }

  
    public ActionResult Details(int id)
    {
        var pet = _repository.GetPetByID(id);

        if (pet == null || pet.Status != "Available")
        {
            TempData["ErrorMessage"] = "This pet is no longer available for adoption.";
            return RedirectToAction("Index", "Pets");
        }

        PopulateDropDowns();
        return View(pet);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("AdoptNow")]
    public ActionResult ProcessAdoption(int PetID, string FullName, string PhoneNumber)
    {
        var pet = _repository.GetPetByID(PetID);

        if (pet == null || pet.Status != "Available")
        {
            TempData["ErrorMessage"] = "Pet status changed. Adoption failed.";
            return RedirectToAction("Index", "Pets");
        }

        if (string.IsNullOrWhiteSpace(FullName) || FullName == "-- Select Your Name --")
        {
            ModelState.AddModelError("FullName", "Please select your Full Name to proceed with adoption.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _repository.InsertAdoptionRecord(PetID, pet.Name, FullName);

                TempData["SuccessMessage"] = $"Congratulations! You have adopted {pet.Name}!";
                return RedirectToAction("Index", "Pets");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage = ex.InnerException.Message;
                }
                ModelState.AddModelError("", "Adoption failed due to a system error. Error: " + errorMessage);
            }
        }
        PopulateDropDowns();
        var users = _repository.GetAllUsers().ToList();
        ViewBag.FullNameList = new SelectList(users, "FullName", "FullName", FullName);
        return View("Details", pet);
    }
}