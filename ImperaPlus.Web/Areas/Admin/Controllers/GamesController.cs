using System;
using System.Linq;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using ImperaPlus.Application.Games;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    public class GamesController : BaseAdminController
    {
        private IGameService gameService;

        public GamesController(IUnitOfWork unitOfWork, IGameService gameService)
            : base(unitOfWork)
        {
            this.gameService = gameService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult View(long gameId)
        {
            var game = this.gameService.Get(gameId);

            this.AddLookups();

            return View(game);
        }

        [HttpPost]
        public DataTablesJsonResult Data(IDataTablesRequest request)
        {
            var data = this.unitOfWork.Games.Query();

            if (request.Search != null && !string.IsNullOrWhiteSpace(request.Search.Value))
            {
                long searchId = long.Parse(request.Search.Value);
                data = data.Where(x => x.Id == searchId);
            }

            var dataPage = data
                .OrderBy(x => x.Id)
                .Skip(request.Start)
                .Take(request.Length)
                .Select(g => new
                {
                    g.Id,
                    Type = g.Type.ToString(),                    
                    g.Name,
                    State = g.State.ToString(),
                    PlayState = g.PlayState.ToString(),
                    g.MapTemplateName,
                    CreatedBy = g.CreatedBy.UserName,
                    CurrentPlayer = "" // g.CurrentPlayer != null ? g.CurrentPlayer.User.UserName : ""
                });

            var response = DataTablesResponse.Create(request, data.Count(), data.Count(), dataPage);

            return new DataTablesJsonResult(response, true);
        }
    }
}