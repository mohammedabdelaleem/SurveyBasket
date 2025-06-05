using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Services;

public interface IQuestionService
{
	Task<Result<QuestionResponse>> AddAsync(int pollId,QuestionRequest request , CancellationToken cancellationToken);
}
