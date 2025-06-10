

using SurveyBasket.API.Contracts.Abswers;
using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Mapping;

public class MappingConfigurations : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		////////////////////////
		//config.NewConfig<QuestionRequest, Question>()
		//.Ignore(nameof(Question.Answers));

		config.NewConfig<QuestionRequest, Question>()
			.Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer { Content = answer })); // super
	
		////////////////////////
	
	config.NewConfig<Question, QuestionResponse>()
			.Map(dest => dest.Answers,
			src => src.Answers.Where(a => a.IsActive));


		config.NewConfig<Question, QuestionResponse>()
			.Map(dest => dest.Answers, src => src.Answers.Select(answer => new AnswerResponse (answer.Id, answer.Content))); // super : Thanks Allah

	}
}