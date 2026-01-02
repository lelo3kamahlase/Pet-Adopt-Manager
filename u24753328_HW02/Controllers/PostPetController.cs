using u24753328_HW02.Models;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using static u24753328_HW02.Data.IPetRespository;
using System.IO;
using System.Web;
using System;

public class PostPetController : Controller
{
    private readonly IPetRepository _repository;

    public PostPetController(IPetRepository repository)
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

        ViewBag.TypeList = new SelectList(_repository.GetDistinctTypes());
        ViewBag.BreedList = new SelectList(_repository.GetDistinctBreeds());
        ViewBag.LocationList = new SelectList(_repository.GetDistinctLocations());
        ViewBag.GenderList = new SelectList(new List<string> { "Male", "Female" });
    }

    public ActionResult Index()
    {
        PopulateDropDowns();
        return View(new PET());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Index(PET pet, HttpPostedFileBase PetImageFile, string PhoneNumber)
    {
        if (string.IsNullOrEmpty(PhoneNumber))
        {
            ModelState.AddModelError("PhoneNumber", "Please select your phone number.");
        }
        
        if (PetImageFile != null && PetImageFile.ContentLength > 0)
        {
            try
            {
                string folderPath = Server.MapPath("~/Images/Pets/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileName = Path.GetFileNameWithoutExtension(PetImageFile.FileName);
                string extension = Path.GetExtension(PetImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                string serverPath = Path.Combine(folderPath, fileName);
                PetImageFile.SaveAs(serverPath);

                pet.ImagePath = "~/Images/Pets/" + fileName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ImagePath", "Image upload failed: " + ex.Message);
                pet.ImagePath = null; 
            }
        }
        else if (string.IsNullOrEmpty(pet.ImagePath))
        {
            pet.ImagePath = "~/Images/default_pet.jpg";
        }

        if (ModelState.IsValid)
        {
            pet.Status = "Available";

            _repository.InsertPet(pet);

            TempData["SuccessMessage"] = $"Successfully posted {pet.Name} for adoption!";
            return RedirectToAction("Index", "Pets");
        }
        PopulateDropDowns();
        return View(pet);
    }
}
