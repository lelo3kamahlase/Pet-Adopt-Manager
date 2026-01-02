using u24753328_HW02.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;
using static u24753328_HW02.Data.IPetRespository;

public class PetRespository : IPetRepository
{
    private const decimal _donationGoal = 100000;

    public IEnumerable<USER> GetAllUsers()
    {
        using (var db = new RescuePetDBEntities2())
        {
            return db.USERs.ToList();
        }
    }

    public IEnumerable<PET> GetPets(string Type, string Breed, string Location)
    {
        using (var db = new RescuePetDBEntities2())
        {
            var query = db.PETs.AsQueryable();

            if (!string.IsNullOrEmpty(Type) && Type != "All")
                query = query.Where(p => p.Type == Type);
            if (!string.IsNullOrEmpty(Breed) && Breed != "All")
                query = query.Where(p => p.Breed == Breed);
            if (!string.IsNullOrEmpty(Location) && Location != "All")
                query = query.Where(p => p.Location == Location);

            return query.ToList();
        }
    }

    public PET GetPetByID(int id)
    {
        using (var db = new RescuePetDBEntities2())
        {
            return db.PETs.FirstOrDefault(p => p.PetID == id);
        }
    }

    public IEnumerable<string> GetDistinctTypes()
    {
        using (var db = new RescuePetDBEntities2())
        {
            return db.PETs.Select(p => p.Type).Distinct().ToList();
        }
    }

    public IEnumerable<string> GetDistinctBreeds(string Type = null)
    {
        using (var db = new RescuePetDBEntities2())
        {
            var query = db.PETs.AsQueryable();
            if (!string.IsNullOrEmpty(Type))
                query = query.Where(p => p.Type == Type);

            return query.Select(p => p.Breed).Distinct().ToList();
        }
    }

    public IEnumerable<string> GetDistinctLocations()
    {
        using (var db = new RescuePetDBEntities2())
        {
            return db.PETs.Select(p => p.Location).Distinct().ToList();
        }
    }

    public decimal GetDonationGoal() => _donationGoal;

    public decimal GetTotalRaised()
    {
        using (var db = new RescuePetDBEntities2())
        {
            return db.DONATIONs.Sum(d => (decimal?)d.Amount) ?? 0m;
        }
    }

    public int GetPetsHomedCount()
    {
        using (var db = new RescuePetDBEntities2())
        {
            return db.ADOPTION_RECORD.Count();
        }
    }

    public IEnumerable<ADOPTION_RECORD> GetRecentAdoptions()
    {
        using (var db = new RescuePetDBEntities2())
        {
            return db.ADOPTION_RECORD
                     .OrderByDescending(r => r.AdoptionDate)
                     .Take(10)
                     .ToList();
        }
    }

    public void InsertPet(PET pet)
    {
        using (var db = new RescuePetDBEntities2())
        {
            db.PETs.Add(pet);
            db.SaveChanges();
        }
    }
    public IEnumerable<DONATION> GetDistinctDonorRecords()
    {
        using (var db = new RescuePetDBEntities2())
        {
            var rawData = db.DONATIONs.ToList(); 
            var distinctDonors = rawData
                .GroupBy(d => new { d.DonorName, d.PhoneNumber })
                .Select(g => g.First())
                .Select(d => new DONATION { DonorName = d.DonorName, PhoneNumber = d.PhoneNumber })
                .ToList();
            return distinctDonors;
        }
    }
    public void InsertDonationRecord(string DonorName, string PhoneNumber, decimal Amount)
    {
        using (var db = new RescuePetDBEntities2())
        {
            var donationRecord = new DONATION
            {
                DonorName = DonorName, 
                PhoneNumber = PhoneNumber,
                Amount = Amount,
            };

            db.DONATIONs.Add(donationRecord);
            db.SaveChanges();
        }
    }
    public void InsertDonation(DONATION donation)
    {
        using (var db = new RescuePetDBEntities2())
        {
            db.DONATIONs.Add(donation);
            db.SaveChanges();
        }
    }

    public void InsertAdoptionRecord(int PetID, string PetName, string UserName)
    {
        using (var db = new RescuePetDBEntities2())
        {
            if (string.IsNullOrWhiteSpace(PetName) || string.IsNullOrWhiteSpace(UserName))
            {
                throw new ArgumentException("PetName and UserName cannot be empty or whitespace.");
            }
            var adoptionRecord = new ADOPTION_RECORD
            {
                PetID = PetID,
                PetName = PetName.Trim(), 
                UserName = UserName.Trim(),
                AdoptionDate = DateTime.UtcNow
            };

            db.ADOPTION_RECORD.Add(adoptionRecord);

            var petToUpdate = db.PETs.FirstOrDefault(p => p.PetID == PetID);
            if (petToUpdate != null)
            {
                petToUpdate.Status = "Adopted";
            }

            try
            {
                db.SaveChanges(); 
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var validationErrors = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => $"{x.PropertyName}: {x.ErrorMessage}");

                var fullErrorMessage = string.Join("; ", validationErrors);

                throw new Exception($"Entity Validation Failed: {fullErrorMessage}", ex);
            }
        }
    }

    public void UpdatePetStatus(int PetID, string status)
    {
        using (var db = new RescuePetDBEntities2())
        {
            var pet = db.PETs.FirstOrDefault(p => p.PetID == PetID);
            if (pet != null)
            {
                pet.Status = status;
                db.SaveChanges();
            }
        }
    }
}
