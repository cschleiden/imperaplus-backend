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
            ViewBag.Logins60 = this.unitOfWork.Users.Query().Count(x => x.LastLogin >= DateTime.UtcNow.AddMinutes(-60));
            ViewBag.Logins120 = this.unitOfWork.Users.Query().Count(x => x.LastLogin >= DateTime.UtcNow.AddMinutes(-120));
            ViewBag.Games120 = this.unitOfWork.Games.Query().Count(x => x.LastTurnStartedAt >= DateTime.UtcNow.AddMinutes(-120));
            ViewBag.ActiveGames = this.unitOfWork.Games.Query().Count(x => x.State == GameState.Active);

            ViewBag.Signedup7d = this.unitOfWork.Users.Query().Where(x => x.CreatedAt >= DateTime.UtcNow.AddDays(-7))
                .GroupBy(x => x.CreatedAt.Date)
                .Select(x => new
                {
                    Count = x.Count(),
                    Confirmed = x.Sum(y => y.EmailConfirmed ? 1 : 0),
                    Day = x.Key
                })
                .ToList();

            ViewBag.UnconfirmedUsers = this.unitOfWork.Users.Query().Count(x => !x.EmailConfirmed);

            return View();
        }
    }
}