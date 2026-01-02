using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using u24753328_HW02.Models;

namespace u24753328_HW02.Data
{
    public class IPetRespository : Controller
    {
        public interface IPetRepository
        {
            IEnumerable<USER> GetAllUsers();
            IEnumerable<DONATION> GetDistinctDonorRecords();

            
            IEnumerable<PET> GetPets(string Type, string Breed, string Location);
            PET GetPetByID(int id);
            IEnumerable<string> GetDistinctTypes();
            IEnumerable<string> GetDistinctBreeds(string Type = null); 
            IEnumerable<string> GetDistinctLocations();

            
            decimal GetDonationGoal();
            decimal GetTotalRaised();
            

            int GetPetsHomedCount();
            IEnumerable<ADOPTION_RECORD> GetRecentAdoptions();

            
            void InsertPet(PET pet);
            void InsertDonation(DONATION donation);
            void InsertAdoptionRecord(int PetID, string PetName, string UserName);
            void InsertDonationRecord(string DonorName, string PhoneNumber, decimal Amount);
            void UpdatePetStatus(int PetID, string status);
        }
    }
}