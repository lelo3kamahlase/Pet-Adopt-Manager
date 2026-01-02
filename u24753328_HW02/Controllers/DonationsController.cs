using u24753328_HW02.Models;
using System.Web.Mvc;
using System.Linq;
using System.Web.Script.Serialization;
using System;
using static u24753328_HW02.Data.IPetRespository;

public class DonationsController : Controller
{
    private readonly IPetRepository _repository;

    public DonationsController(IPetRepository repository)
    {
        _repository = repository;
    }
    private void PopulateDonationMetricsAndDropdowns()
    {
        // Donation Metrics
        decimal goal = _repository.GetDonationGoal();
        decimal totalRaised = _repository.GetTotalRaised();
        ViewBag.Goal = goal;
        ViewBag.TotalRaised = totalRaised;
        ViewBag.Percentage = goal > 0 ? Math.Min(100, (int)((totalRaised / goal) * 100)) : 0;

        var donors = _repository.GetDistinctDonorRecords().ToList();
        ViewBag.DonorList = new SelectList(donors, "DonorName", "DonorName"); 
        var phoneNumbersMap = donors
             .GroupBy(d => d.DonorName)
             .ToDictionary(
                 g => g.Key,          
                 g => g.First().PhoneNumber 
             );
    }

    public ActionResult Index()
    {
        PopulateDonationMetricsAndDropdowns();
        ViewBag.Message = TempData["Message"];
        return View(new DONATION());
    }

    [HttpPost]
    public ActionResult SubmitDonation(DONATION donation)
    {
      
        if (ModelState.IsValid)
        {
            _repository.InsertDonation(donation);

            TempData["Message"] = $"Thank you, {donation.DonorName}! Your donation of R{donation.Amount:N2} was successful and saved a life!";
            return RedirectToAction("Index");
        }

        PopulateDonationMetricsAndDropdowns();
        return View("Index", donation);
    }
}