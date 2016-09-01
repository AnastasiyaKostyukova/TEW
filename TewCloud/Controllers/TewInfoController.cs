﻿using System.Linq;
using System.Web.Http;
using Domain.RepositoryFactories;
using TewCloud.Models;

namespace TewCloud.Controllers
{
  public class TewInfoController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public TewInfoController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    [HttpGet]
    public IHttpActionResult GetTewInfo()
    {
      var usersCount = _repositoryFactory.UserRepository.All().Count();
      var wordsCount = _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Count(r => r.IsDeleted == false);

      var infoModel = new TewInfoModel
      {
        Users = usersCount,
        Words = wordsCount
      };

      return Json(infoModel);
    }
  }
}