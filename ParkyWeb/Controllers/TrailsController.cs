using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly ITrailRepository trailRepository;
        private readonly INationalParkRepository nationalParkRepository;

        public TrailsController(ITrailRepository trailRepository, INationalParkRepository nationalParkRepository)
        {
            this.trailRepository = trailRepository;
            this.nationalParkRepository = nationalParkRepository;
        }

        public IActionResult Index()
        {
            return View(new Trail(){ });
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await trailRepository.GetAllAsync(SD.TrailApiPath) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await trailRepository.DeleteAsync(SD.TrailApiPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete succesful" });

            }
            return Json(new { success = false, message = "Delete not succesful" });

        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await nationalParkRepository.GetAllAsync(SD.NationalParkApiPath);
            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = npList.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }),
                Trail=new Trail()
            };
            if (id == null)
                return View(objVM);
            objVM.Trail = await trailRepository.GetAsync(SD.TrailApiPath, id.GetValueOrDefault());
            if (objVM.Trail == null)
                return NotFound();

            return View(objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
            if (ModelState.IsValid)
            {
              
                if (obj.Trail.Id == 0)
                {
                    await trailRepository.CreateAsync(SD.TrailApiPath, obj.Trail);
                }
                else
                {
                    await trailRepository.UpdateAsync(SD.TrailApiPath + obj.Trail.Id, obj.Trail);

                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> npList = await nationalParkRepository.GetAllAsync(SD.NationalParkApiPath);
                TrailsVM objVM = new TrailsVM()
                {
                    NationalParkList = npList.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }),
                    Trail = obj.Trail
                };
                return View(objVM);
            }
        }

    }
}
