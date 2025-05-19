using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevSpot.Tests
{
	public class JobPostingRepositoryTests
	{
		private readonly DbContextOptions<ApplicationDbContext> _options;

		public JobPostingRepositoryTests()
		{
			_options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("JobPostingDb")
				.Options;
		}

		private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);

		[Fact]
		public async Task AddAsync_ShouldAddJobPosting()
		{
			var db = CreateDbContext();
			var repository = new JobPostingRepository(db);
			var jobPosting = new JobPosting
			{
				Title = "Test Title",
				Description = "Test Description",
				PostedDate = DateTime.Now,
				Company = "Test Company",
				Location = "Test Location",
				UserId = "TestUserId",
			};

			await repository.AddAsync(jobPosting);

			var result = await db.JobPostings.SingleOrDefaultAsync(j => j.Title == "Test Title");

			Assert.NotNull(result);
			Assert.Equal("Test Description", result.Description);
		}
	}
}
