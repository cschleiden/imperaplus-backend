using System;
using ImperaPlus.Application.Ladder;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    public class LaddersController : BaseAdminController
    {
        private ILadderService ladderService;

        public LaddersController(IUnitOfWork unitOfWork, ILadderService ladderService)
            : base(unitOfWork)
        {
            this.ladderService = ladderService;
        }

        public ActionResult Index()
        {
            var ladders = ladderService.GetAllFull();

            AddLookups();

            return View(ladders);
        }

        public ActionResult Create()
        {
            AddLookups();

            var ladder = new DTO.Ladder.Ladder();
            ladder.Options = new DTO.Games.GameOptions();
            ladder.MapTemplates = new string[0];

            return View(ladder);
        }

        [HttpPost]
        public ActionResult PostUpdate(DTO.Ladder.Ladder ladder)
        {
            ladderService.UpdateName(ladder.Id, ladder.Name);
            ladderService.ToggleActive(ladder.Id, ladder.IsActive);
            ladderService.UpdateGameOptions(ladder.Id, ladder.Options);
            ladderService.UpdateMapTemplates(ladder.Id, ladder.MapTemplates);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PostCreate(DTO.Ladder.Ladder ladder)
        {
            var summary = ladderService.Create(new DTO.Ladder.Admin.CreationOptions
            {
                Name = ladder.Name,
                NumberOfPlayers = ladder.Options.NumberOfPlayersPerTeam,
                NumberOfTeams = ladder.Options.NumberOfTeams
            });

            ladderService.UpdateGameOptions(summary.Id, ladder.Options);
            ladderService.UpdateMapTemplates(summary.Id, ladder.MapTemplates);
            ladderService.ToggleActive(summary.Id, ladder.IsActive);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PostDelete(Guid id)
        {
            ladderService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
