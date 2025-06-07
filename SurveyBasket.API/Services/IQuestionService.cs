using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Services;

public interface IQuestionService
{
	Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken);

	Task<Result<QuestionResponse>> GetAsync(int pollId, int questionId, CancellationToken cancellationToken);

	Task<Result<QuestionResponse>> AddAsync(int pollId,QuestionRequest request , CancellationToken cancellationToken);


}
