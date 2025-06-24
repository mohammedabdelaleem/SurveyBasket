using SurveyBasket.API.Contracts.Common;
using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Services;

public interface IQuestionService
{
	Task<Result<PaginationList<QuestionResponse>>> GetAllAsync(int pollId, RequestFilters filters, CancellationToken cancellationToken);
	Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId,string userId, CancellationToken cancellationToken);
	Task<Result<QuestionResponse>> GetAsync(int pollId, int questionId, CancellationToken cancellationToken);
	Task<Result<QuestionResponse>> AddAsync(int pollId,QuestionRequest request , CancellationToken cancellationToken);
	Task<Result> UpdateAsync(int pollId, int questionId, QuestionRequest request, CancellationToken cancellationToken);
	Task<Result> ToggleStatusAsync(int pollId, int questionId, CancellationToken cancellationToken);
}
