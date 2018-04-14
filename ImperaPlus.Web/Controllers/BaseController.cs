using AutoMapper;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    public class BaseController : Controller
    {
        private IUnitOfWork unitOfWork;

        protected BaseController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        protected IActionResult Map<T>(object source)
        {
            return this.Ok(Mapper.Map<T>(source));
        }

        protected IActionResult CommitAndMap<T>(object source)
        {
            this.unitOfWork.Commit();

            return this.Map<T>(source);
        }

        protected IActionResult Commit()
        {
            this.unitOfWork.Commit();
            return this.Ok();
        }
    }
}