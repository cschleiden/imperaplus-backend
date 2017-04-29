using System;
using System.Collections.Generic;
using ImperaPlus.Application.Tournaments;
using ImperaPlus.Backend.Areas.Admin.Helpers;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    public class TournamentsController : BaseAdminController
    {
        private ITournamentService tournamentService;

        public TournamentsController(IUnitOfWork unitOfWork, ITournamentService tournamentService)
            : base(unitOfWork)
        {
            this.tournamentService = tournamentService;
        }

        // GET: Admin/Tournaments
        public ActionResult Index()
        {
            this.AddLookups();

            var tournaments = this.tournamentService.GetAllFull();

            return View(tournaments);
        }

        public ActionResult Create()
        {
            this.AddLookups();

            var tournament = new DTO.Tournaments.Tournament();

            tournament.StartOfRegistration = DateTime.UtcNow.AddDays(1);
            tournament.StartOfTournament = DateTime.UtcNow.AddDays(2);

            tournament.MapTemplates = new string[0];
            tournament.Options = new DTO.Games.GameOptions
            {
                NumberOfTeams = 2
            };
            GameOptionsHelper.SetDefaultGameOptions(tournament.Options);

            ViewBag.FixedOptions = new HashSet<string> { "NumberOfTeams" };

            return View(tournament);
        }

        [HttpPost]
        public ActionResult PostCreate(DTO.Tournaments.Tournament tournament)
        {
            this.tournamentService.Create(tournament);

            return this.Redirect("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            this.tournamentService.Delete(id);

            return this.Redirect("Index");
        }
    }
}