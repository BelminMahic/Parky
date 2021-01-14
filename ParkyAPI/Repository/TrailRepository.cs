using Microsoft.EntityFrameworkCore;
using ParkyAPI.DataFolder;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {

        private readonly ApplicationDbContext context;

        public TrailRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool CreateTrail(Trail Trail)
        {
            context.Trails.Add(Trail);
            return Save();
        }

        public bool DeleteTrail(Trail Trail)
        {
            context.Trails.Remove(Trail);
            return Save();
        }

        public Trail GetTrail(int TrailId)
        {
            return context.Trails.Include(x => x.NationalPark).Where(x => x.Id == TrailId).FirstOrDefault();
        }

        public ICollection<Trail> GetTrails()
        {
            return context.Trails.Include(x => x.NationalPark).OrderBy(x => x.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            var value = context.Trails.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return context.Trails.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail Trail)
        {
            context.Trails.Update(Trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {
            return context.Trails.Include(x => x.NationalPark)
                    .Where(x=>x.NationalParkId==nationalParkId)
                    .ToList();
        }
    }
}
