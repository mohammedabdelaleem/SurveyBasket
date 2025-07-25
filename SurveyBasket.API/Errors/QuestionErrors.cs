﻿namespace SurveyBasket.API.Errors;


public record QuestionErrors
{

	public static readonly Error QuestionNotFound
		= new Error("Question.NotFound", "Question With Given Id Not Found", StatusCodes.Status404NotFound);


	public static readonly Error QuestionsEmpty
		= new Error("Question.Empty", "No Question Content Are Found", StatusCodes.Status404NotFound);


	public static readonly Error SaveError
		= new Error("Question.SaveError", "Question While Saving", StatusCodes.Status500InternalServerError);


	public static readonly Error DuplicateContent
		= new Error("Question.DuplicateTitle", "Question With The Same Content Value Is Found At The Same Poll, Please Enter Another One", StatusCodes.Status409Conflict);




}
