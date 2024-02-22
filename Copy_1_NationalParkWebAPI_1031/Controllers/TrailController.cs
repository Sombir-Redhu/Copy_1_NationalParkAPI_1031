using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Copy_1_NationalParkWebAPI_1031.Models;
using Copy_1_NationalParkWebAPI_1031.Models.ViewModels;
using Copy_1_NationalParkWebAPI_1031.Repository.IRepository;
using Copy_1_NationalParkWebAPI_1031;

namespace Copy_1_NationalParkWebAPI_1031.Controllers
{
    public class TrailController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly INationalParkRepository _nationalParkRepository;
        public TrailController(ITrailRepository trailRepository , INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;   
            _trailRepository = trailRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(SD.TrailAPIPath) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id) 
        {
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);   
            if(status)
                return Json(new { success = true, message = " Data delete successfully delete !!!!" });
            return Json(new { success = false, messagw = "Somthing went wrong to delete data" });
        }
        #endregion
        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalParks = await _nationalParkRepository.GetAllAsync
                (SD.NationalParkAPIPath);
            TrailVM trailVM = new TrailVM()
            {
                Trail = new Trail(),
                nationalParkList = nationalParks.Select(np => new SelectListItem()
                {
                    Text = np.Name,
                    Value= np.Id.ToString()
                })
            };
            if (id == null) return View(trailVM);
            trailVM.Trail = await _trailRepository.GetAsync
                (SD.TrailAPIPath, id.GetValueOrDefault());
            if(trailVM.Trail == null) return NotFound();
            return View(trailVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailVM trailVM)
        {
            if (ModelState.IsValid)
            {
                if (trailVM.Trail.Id == 0)
                    await _trailRepository.CreateAsync(SD.TrailAPIPath, trailVM.Trail);
                else
                    await _trailRepository.UpdateAsync(SD.TrailAPIPath, trailVM.Trail);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var nationalPark = await _nationalParkRepository.GetAllAsync
                    (SD.NationalParkAPIPath);
                trailVM = new TrailVM()
                {
                    Trail = new Trail(),
                    nationalParkList = nationalPark.Select(np => new SelectListItem()
                    {
                        Text = np.Name,
                        Value = np.Id.ToString()
                    }),
                };
                return View(trailVM);   
            }
        }
    }
}
