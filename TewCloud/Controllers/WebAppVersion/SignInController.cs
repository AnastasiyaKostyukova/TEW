﻿using System.Web.Http;
using Domain.Entities;
using Domain.RepositoryFactories;
using TewCloud.Auth;

namespace TewCloud.Controllers.WebAppVersion
{
	public class SignInController : ApiController
	{
		private readonly IRepositoryFactory _repositoryFactory;

		public SignInController(IRepositoryFactory repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		[HttpPost]
		public IHttpActionResult LogIn([FromBody] User user)
		{
			var userProvider = new UserProvider(_repositoryFactory);
			var validatedUser = userProvider.ValidateUser(user.Email, user.Password);

			return Json(validatedUser);
		}
	}
}
