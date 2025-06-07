using SurveyBasket.API.Contracts.Abswers;
using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Services;

public class QuestionService(AppDbContext context) : IQuestionService
{
	private readonly AppDbContext _context = context;

	public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken)
	{

		var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);

		if (!pollIsExists)
			return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);


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
		var questionsAtPollTarget = await _context.Questions
				.Where(q => q.PollId == pollId)
				.Include(q => q.Answers)
				.ProjectToType<QuestionResponse>()
				.AsNoTracking()
				.ToListAsync(cancellationToken);

		if (questionsAtPollTarget.Count == 0)
			return Result.Failure<IEnumerable<QuestionResponse>>(QuestionErrors.QuestionsEmpty);


		return Result.Success(questionsAtPollTarget.Adapt<IEnumerable<QuestionResponse>>());

	}

	public async Task<Result<QuestionResponse>> GetAsync(int pollId, int questionId, CancellationToken cancellationToken)
	{
		
		var questionIsExists = await _context.Questions
			.AnyAsync(q => q.PollId == pollId && q.Id == questionId);

		if(!questionIsExists)
			return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);


		var questionResponse = await _context.Questions
			.Include(q => q.Answers)
			.Where(q => q.PollId == pollId && q.Id == questionId)
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


		return Result.Success(question.Adapt<QuestionResponse>());
	}

	
}
