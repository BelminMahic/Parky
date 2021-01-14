using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails(); //get all
        ICollection<Trail> GetTrailsInNationalPark(int nationalParkId);
        Trail GetTrail(int trailId); // get single national park
        bool TrailExists(string name);
        bool TrailExists(int id);
        bool CreateTrail(Trail nationalPark);
        bool UpdateTrail(Trail nationalPark);
        bool DeleteTrail(Trail nationalPark);
        bool Save();
    }
}
