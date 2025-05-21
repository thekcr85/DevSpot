using DevSpot.Models;
using DevSpot.Repositories;
using DevSpot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;

namespace DevSpot.Controllers
{
	[Authorize]
	public class JobPostingsController : Controller
	{
		private readonly IRepository<JobPosting> _repository;
		private readonly UserManager<IdentityUser> _userManager;

		public JobPostingsController(IRepository<JobPosting> repository, UserManager<IdentityUser> userManager)
		{
			_repository = repository;
			_userManager = userManager;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var jobPostings = await _repository.GetAllAsync();
			return View(jobPostings);
		}

		[Authorize(Roles = "Admin,Employer")]
		public IActionResult Create()
		{

			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Admin,Employer")]
		public async Task<IActionResult> Create(JobPostingViewModel jobPostingViewModel)
		{
			if (ModelState.IsValid)
			{
				var jobPosting = new JobPosting
				{
					Title = jobPostingViewModel.Title,
					Description = jobPostingViewModel.Description,
					Company = jobPostingViewModel.Company,
					Location = jobPostingViewModel.Location,
					UserId = _userManager.GetUserId(User)
				};
				await _repository.AddAsync(jobPosting);
				return RedirectToAction(nameof(Index));
			}

			return View(jobPostingViewModel);
		}
	}
}

