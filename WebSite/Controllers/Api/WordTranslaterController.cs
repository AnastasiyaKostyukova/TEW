﻿using System.Web.Http;
using Domain.RepositoryFactories;

namespace WebSite.Controllers.Api
{
  public class WordTranslaterController : ApiController
	{
		private readonly IRepositoryFactory _repositoryFactory;

		public WordTranslaterController(IRepositoryFactory repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		public IHttpActionResult GetWordTranslates(string word)
		{
			var translates = _repositoryFactory.EnRuWordsRepository.GetTranslate(word);
			return Json(translates);
		}
	}
}