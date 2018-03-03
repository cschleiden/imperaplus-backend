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
            var ladders = this.ladderService.GetAllFull();

            this.AddLookups();

            return View(ladders);
        }

        public ActionResult Create()
        {
            this.AddLookups();

            var ladder = new DTO.Ladder.Ladder();
            ladder.Options = new DTO.Games.GameOptions();
            ladder.MapTemplates = new string[0];

            return View(ladder);
        }

        [HttpPost]
        public ActionResult PostUpdate(DTO.Ladder.Ladder ladder)
        {
            this.ladderService.ToggleActive(ladder.Id, ladder.IsActive);
            this.ladderService.UpdateGameOptions(ladder.Id, ladder.Options);
            this.ladderService.UpdateMapTemplates(ladder.Id, ladder.MapTemplates);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PostCreate(DTO.Ladder.Ladder ladder)
        {
            var summary = this.ladderService.Create(new DTO.Ladder.Admin.CreationOptions
            {
                Name = ladder.Name,
                NumberOfPlayers = ladder.Options.NumberOfPlayersPerTeam,
                NumberOfTeams = ladder.Options.NumberOfTeams
            });

            this.ladderService.UpdateGameOptions(summary.Id, ladder.Options);
            this.ladderService.UpdateMapTemplates(summary.Id, ladder.MapTemplates);
            this.ladderService.ToggleActive(summary.Id, ladder.IsActive);

            return this.RedirectToAction("Index");
        }        

        [HttpPost]
        public ActionResult PostDelete(Guid id)
        {
            this.ladderService.Delete(id);

            return this.RedirectToAction("Index");
        }
    }
}