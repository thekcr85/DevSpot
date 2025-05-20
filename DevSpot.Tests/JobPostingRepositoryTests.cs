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
				Title = "Test Title Adding",
				Description = "Test Description",
				PostedDate = DateTime.Now,
				Company = "Test Company",
				Location = "Test Location",
				UserId = "TestUserId",
			};

			await repository.AddAsync(jobPosting);

			var result = db.JobPostings.Find(jobPosting.Id);

			Assert.NotNull(result);
			Assert.Equal("Test Description", result.Description);
		}

		[Fact]
		public async Task GetByIdAsync_ShouldReturnJobPosting()
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

			await db.JobPostings.AddAsync(jobPosting);
			await db.SaveChangesAsync();

			var result = await repository.GetByIdAsync(jobPosting.Id);

			Assert.NotNull(result);
			Assert.Equal("Test Title", result.Title);
		}

		[Fact]
		public async Task GetByIdAsync_ShouldThrowKeyNotFoundExeption()
		{
			var db = CreateDbContext();
			var repository = new JobPostingRepository(db);

			await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.GetByIdAsync(999));
		}

		[Fact]
		public async Task GetAllAsync_ShouldReturnAllJobPostings()
		{
			var db = CreateDbContext();
			var repository = new JobPostingRepository(db);
			var jobPosting1 = new JobPosting
			{
				Title = "Test Title 1",
				Description = "Test Description 1",
				PostedDate = DateTime.Now,
				Company = "Test Company 1",
				Location = "Test Location 1",
				UserId = "TestUserId 1",
			};
			var jobPosting2 = new JobPosting
			{
				Title = "Test Title 2",
				Description = "Test Description 2",
				PostedDate = DateTime.Now,
				Company = "Test Company 2",
				Location = "Test Location 2",
				UserId = "TestUserId 2",
			};

			await db.JobPostings.AddRangeAsync(jobPosting1, jobPosting2);
			await db.SaveChangesAsync();

			var result = await repository.GetAllAsync();
			Assert.NotNull(result);
			Assert.True(result.Count() >= 2); 
		}

		[Fact]
		public async Task UpdateAsync_ShouldUpdateJobPosting()
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
			await db.JobPostings.AddAsync(jobPosting);
			await db.SaveChangesAsync();
			jobPosting.Title = "Updated Title";
			await repository.UpdateAsync(jobPosting);
			var result = await db.JobPostings.FindAsync(jobPosting.Id);
			Assert.NotNull(result);
			Assert.Equal("Updated Title", result.Title);
		}

		[Fact]
		public async Task DeleteAsync_ShouldDeleteJobPosting()
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
			await db.JobPostings.AddAsync(jobPosting);
			await db.SaveChangesAsync();
			await repository.DeleteAsync(jobPosting.Id);
			var result = await db.JobPostings.FindAsync(jobPosting.Id);
			Assert.Null(result);
		}
	}
}
