

using SurveyBasket.API.Contracts.Abswers;
using SurveyBasket.API.Contracts.Questions;
using SurveyBasket.API.Contracts.Users;

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
			.Map(dest => dest.Answers, src => src.Answers.Select(answer => new AnswerResponse(answer.Id, answer.Content))); // super : Thanks Allah


		config.NewConfig<RegisterRequest, ApplicationUser>()
			.Map(dest => dest.UserName, src => src.Email);


		config.NewConfig<(ApplicationUser user, IList<string> userRoles), UserResponse>()
			.Map(dest => dest, src => src.user)
			.Map(dest => dest.Roles, src => src.userRoles);


		config.NewConfig<CreateUserRequest, ApplicationUser>()
			.Map(dest => dest.UserName, src => src.Email)
			.Map(dest => dest.EmailConfirmed, src => true); // we confirmed email directly , we choose the easy way ===> we can ignore password from createUserRequest and send email confirmation then recive the code , email and password then set the password to the confirmed user 

		config.NewConfig<UpdateUserRequest, ApplicationUser>()
			.Map(dest => dest.UserName, src => src.Email)
			.Map(dest => dest.NormalizedUserName, src => src.Email.ToUpper()); // update need this  

	}
}