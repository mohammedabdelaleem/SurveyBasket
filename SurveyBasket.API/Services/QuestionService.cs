using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Services;

public class QuestionService(AppDbContext context) : IQuestionService
{
	private readonly AppDbContext _context = context;

	public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken)
	{
		var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);

		if (!pollIsExists)
			return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);


		var isQuestionFoundWithTheSamePoll = await _context.Questions.AnyAsync(q => q.Content == request.Content && q.PollId == pollId, cancellationToken);
		if (isQuestionFoundWithTheSamePoll)
			return Result.Failure<QuestionResponse>(QuestionErrors.DuplicateContent);


		// now
		// poll is found 
		// question content is unique at poll

		var question = request.Adapt<Question>();
		question.PollId = pollId; ///

		//request.Answers.ForEach(answer =>{question.Answers.Add(new Answer { Content = answer });});

		await _context.AddAsync(question, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);


		return Result.Success(question.Adapt<QuestionResponse>());
	}
}
