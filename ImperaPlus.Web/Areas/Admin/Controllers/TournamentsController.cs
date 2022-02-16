using System;
using System.Collections.Generic;
using ImperaPlus.Application.Tournaments;
using ImperaPlus.Backend.Areas.Admin.Helpers;
using ImperaPlus.Domain.Repositories;
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
            AddLookups();

            var tournaments = tournamentService.GetRecentFull();

            return View(tournaments);
        }

        public ActionResult Show(Guid tournamentId)
        {
            AddLookups();

            var tournament = this.tournamentService.Get(tournamentId, false, false);

            return View(tournament);
        }

        public ActionResult Create()
        {
            AddLookups();

            var tournament = new DTO.Tournaments.Tournament
            {
                StartOfRegistration = DateTime.UtcNow.AddDays(1),
                StartOfTournament = DateTime.UtcNow.AddDays(2),

                MapTemplates = new string[0],
                Options = new DTO.Games.GameOptions { NumberOfTeams = 2 }
            };
            GameOptionsHelper.SetDefaultGameOptions(tournament.Options);

            ViewBag.FixedOptions = new HashSet<string> { "NumberOfTeams" };

            return View(tournament);
        }

        [HttpPost]
        public ActionResult PostCreate(DTO.Tournaments.Tournament tournament)
        {
            tournamentService.Create(tournament);

            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            tournamentService.Delete(id);

            return Redirect("Index");
        }
    }
}
