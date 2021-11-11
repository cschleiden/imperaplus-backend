using System;
using System.Linq;
using ImperaPlus.Application.News;
using ImperaPlus.Backend.Areas.Admin.Lib;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.DTO.News;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    public class StatsController : BaseAdminController
    {
        public StatsController(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public ActionResult Index()
        {
            ViewBag.Logins60 = unitOfWork.Users.Query().Count(x => x.LastLogin >= DateTime.UtcNow.AddMinutes(-60));
            ViewBag.Logins120 = unitOfWork.Users.Query().Count(x => x.LastLogin >= DateTime.UtcNow.AddMinutes(-120));
            ViewBag.Games120 = unitOfWork.Games.Query()
                .Count(x => x.LastTurnStartedAt >= DateTime.UtcNow.AddMinutes(-120));
            ViewBag.ActiveGames = unitOfWork.Games.Query().Count(x => x.State == GameState.Active);

            ViewBag.Signedup7d = unitOfWork.Users.Query().Where(x => x.CreatedAt >= DateTime.UtcNow.AddDays(-7))
                .GroupBy(x => x.CreatedAt.Date)
                .Select(x =>
                    new { Count = x.Count(), Confirmed = x.Sum(y => y.EmailConfirmed ? 1 : 0), Day = x.Key }
                        .ToExpando())
                .ToList();

            ViewBag.UnconfirmedUsers = unitOfWork.Users.Query().Count(x => !x.EmailConfirmed);

            return View();
        }
    }
}
