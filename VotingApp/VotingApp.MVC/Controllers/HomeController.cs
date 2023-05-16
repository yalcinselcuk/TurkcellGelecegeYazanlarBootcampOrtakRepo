﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using VotingApp.Business.Services;
using VotingApp.MVC.Models;

namespace VotingApp.MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPollService _pollService;
        private readonly IUserService _userService;
        private int UserId => int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        public HomeController(IPollService pollService, IUserService userService)
        {
            _pollService = pollService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var joinedPolls = _pollService.GetJoinedPollsAsync(UserId).GetAwaiter().GetResult();
            var user = _userService.GetById(UserId).GetAwaiter().GetResult();
            var viewModel = new PollViewModel
            {
                Polls = joinedPolls,
                User = user
            };
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}