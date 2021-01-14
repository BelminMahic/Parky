using ParkyAPI.DataFolder;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext context;

        public NationalParkRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            context.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            context.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return context.NationalParks.Where(x => x.Id == nationalParkId).FirstOrDefault();
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return context.NationalParks.OrderBy(x => x.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            var value = context.NationalParks.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
           return context.NationalParks.Any(x => x.Id==id);
        }

        public bool Save()
        {
            return context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            context.NationalParks.Update(nationalPark);
            return Save();
        }

        
    }
}
