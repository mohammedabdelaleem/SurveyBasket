using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Services;

public interface IQuestionService
{
	Task<Result<QuestionResponse>> AddAsync(int pollId,QuestionRequest request , CancellationToken cancellationToken);


	Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken);


}
