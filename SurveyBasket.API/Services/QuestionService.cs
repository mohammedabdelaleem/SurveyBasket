﻿using Microsoft.Extensions.Caching.Hybrid;
using SurveyBasket.API.Contracts.Common;
using SurveyBasket.API.Contracts.Questions;
using System.Linq.Dynamic.Core;

namespace SurveyBasket.API.Services;

public class QuestionService(AppDbContext context, HybridCache hybridCache, ILogger<QuestionService> logger) : IQuestionService
{
	private readonly AppDbContext _context = context;
	private readonly HybridCache _hybridCache = hybridCache;
	private readonly ILogger<QuestionService> _logger = logger;
	private readonly string _chachePrefix = "availableQuestions";

	public async Task<Result<PaginationList<QuestionResponse>>> GetAllAsync(int pollId, RequestFilters filters, CancellationToken cancellationToken)
	{

		var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);

		if (!pollIsExists)
			return Result.Failure<PaginationList<QuestionResponse>>(PollErrors.PollNotFound);


		#region EF Tricks

		//// get all columns 
		//var questionsAtPollTarget = await _context.Questions
		//	.Include(q => q.Answers)
		//	.Where(q => q.PollId == pollId)
		//	.AsNoTracking()
		//	.ToListAsync(cancellationToken);


		//// to select specific columns
		//var questionsAtPollTarget = await _context.Questions
		//		.Include(q => q.Answers)
		//		.Where(q => q.PollId == pollId)
		//		.Select(q => new QuestionResponse( 
		//			q.Id,
		//			q.Content,
		//			q.Answers.Select(a => new AnswerResponse(a.Id, a.Content)).ToList()
		//		))
		//		.AsNoTracking()
		//		.ToListAsync(cancellationToken);

		#endregion


		//********using mapster projection****************
		var query = _context.Questions
				.Where(q => q.PollId == pollId);


		if (!string.IsNullOrEmpty(filters.SearchValue))
			query = query.Where(q => q.Content.Contains(filters.SearchValue));


		var source = query
						.OrderBy($"{filters.SortColumn} {filters.SortDirection}")
						.Include(q => q.Answers)
						.ProjectToType<QuestionResponse>()
						.AsNoTracking();

		var response = await PaginationList<QuestionResponse>.CreateAsync(source, filters.PageNumber, filters.PageSize, cancellationToken);

		return Result.Success(response);

	}


	public async Task<Result<PaginationList<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, RequestFilters filters, CancellationToken cancellationToken)
	{

		var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId &&
				p.IsPublished &&
				DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt &&
				DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt,
			cancellationToken);

		if (!pollIsExists)
			return Result.Failure<PaginationList<QuestionResponse>>(PollErrors.PollNotFound);


		var userIsExists = await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
		if (!userIsExists)
			return Result.Failure<PaginationList<QuestionResponse>>(UserErrors.UserNotFound);


		var isVoted = await _context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId, cancellationToken);
		if (isVoted)
			return Result.Failure<PaginationList<QuestionResponse>>(VoteErrors.DuplicateVote);


		// Distributed Caching 
		var cacheKey = $"{_chachePrefix}-{pollId}";

		var availabeQuestions = await _hybridCache.GetOrCreateAsync<IEnumerable<QuestionResponse>>(
			cacheKey,
			async cacheEntry =>
			{
				_logger.LogInformation("Cache miss for key {CacheKey}, querying DATABASEEEEEEEEEEEEEEEEEEEE...", cacheKey);

				var result = await _context.Questions
				   .Where(q => q.PollId == pollId && q.IsActive)
				   .Include(q => q.Answers)
				   .ProjectToType<QuestionResponse>()
				   .AsNoTracking()
				   .ToListAsync(cancellationToken);

				return result;
			},
			cancellationToken: cancellationToken);

		// get the data from cache then pagination
		// Now paginate manually (since already in memory)
		var paginated = PaginationList<QuestionResponse>.Create(
				availabeQuestions.Where(q => string.IsNullOrEmpty(filters.SearchValue) || q.Content.ToLower().Contains(filters.SearchValue.ToLower())),
				filters.PageNumber, filters.PageSize);

		return Result.Success(paginated);
	}
	public async Task<Result<QuestionResponse>> GetAsync(int pollId, int questionId, CancellationToken cancellationToken)
	{

		var questionIsExists = await _context.Questions
			.AnyAsync(q => q.PollId == pollId && q.Id == questionId,
					cancellationToken);

		if (!questionIsExists)
			return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);


		var questionResponse = await _context.Questions
			.Include(q => q.Answers)
			.Where(q => q.PollId == pollId)
			.ProjectToType<QuestionResponse>()
			.SingleOrDefaultAsync(cancellationToken);

		return Result.Success(questionResponse!);
	}


	// this question follows any poll 
	public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken)
	{
		var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);

		if (!pollIsExists)
			return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);


		var isQuestionFoundWithTheSamePoll = await _context.Questions.AnyAsync(q => q.Content == request.Content
						&& q.PollId == pollId, cancellationToken);

		if (isQuestionFoundWithTheSamePoll)
			return Result.Failure<QuestionResponse>(QuestionErrors.DuplicateContent);


		// now
		// poll is found 
		// question content is unique at poll === No Duplicate

		var question = request.Adapt<Question>();
		question.PollId = pollId; ///Don't forget this 

		//request.Answers.ForEach(answer =>{question.Answers.Add(new Answer { Content = answer });});

		await _context.AddAsync(question, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		await _hybridCache.RemoveAsync($"{_chachePrefix}-{pollId}", cancellationToken);

		return Result.Success(question.Adapt<QuestionResponse>());
	}


	public async Task<Result> UpdateAsync(int pollId, int questionId, QuestionRequest request, CancellationToken cancellationToken)
	{
		var questionIsExists = await _context.Questions.AnyAsync(
			q => q.PollId == pollId &&
			q.Content == request.Content &&
			q.Id != questionId, // send the same question with another answers || send the same question with the same answers
			cancellationToken);


		if (questionIsExists)
			return Result.Failure(QuestionErrors.DuplicateContent);


		var questionInDB = await _context.Questions
			.Include(q => q.Answers)
			.SingleOrDefaultAsync(
				q => q.PollId == pollId && q.Id == questionId,
			cancellationToken);

		if (questionInDB == null)
			return Result.Failure(QuestionErrors.QuestionNotFound);

		questionInDB.Content = request.Content;

		// Answers DB ===> as list<string> 
		var answersDB = questionInDB.Answers.Select(a => a.Content).ToList();


		// add new answers  
		var newAnswers = request.Answers.Except(answersDB).ToList();  // A-B

		newAnswers.ForEach(newAnswer =>
			questionInDB.Answers.Add(new Answer { Content = newAnswer })
		); //  i think we can also use Set DS Which Is Collect Distincts Only 


		// update the current state simtenously
		questionInDB.Answers.ToList().ForEach(answer =>
				answer.IsActive = request.Answers.Contains(answer.Content));


		await _context.SaveChangesAsync(cancellationToken);

		await _hybridCache.RemoveAsync($"{_chachePrefix}-{pollId}", cancellationToken);

		return Result.Success();
	}


	public async Task<Result> ToggleStatusAsync(int pollId, int questionId, CancellationToken cancellationToken)
	{
		var qusetionInDB = await _context.Questions.SingleOrDefaultAsync(q => q.PollId == pollId && q.Id == questionId, cancellationToken);

		if (qusetionInDB is null)
			return Result.Failure(QuestionErrors.QuestionNotFound);

		qusetionInDB.IsActive = !qusetionInDB.IsActive;
		await _context.SaveChangesAsync(cancellationToken);

		await _hybridCache.RemoveAsync($"{_chachePrefix}-{pollId}", cancellationToken);

		return Result.Success();
	}


}
