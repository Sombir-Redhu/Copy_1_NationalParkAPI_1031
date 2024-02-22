﻿using Microsoft.AspNetCore.Mvc;
using Copy_1_NationalParkWebAPI_1031.Models;
using Copy_1_NationalParkWebAPI_1031.Models.ViewModels;
using Copy_1_NationalParkWebAPI_1031.Repository.IRepository;
using System.Diagnostics;
using Copy_1_NationalParkWebAPI_1031;

namespace Copy_1_NationalParkWebAPI_1031.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;

        public HomeController(ILogger<HomeController> logger,INationalParkRepository nationalParkRepository ,ITrailRepository trailRepository)
        {
            _logger = logger;
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM indexVM = new IndexVM()
            {
                NationalParksList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath),
                TrailList = await _trailRepository.GetAllAsync(SD.TrailAPIPath)
            };
            return View(indexVM);   
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
