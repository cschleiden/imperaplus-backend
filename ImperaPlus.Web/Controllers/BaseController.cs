using AutoMapper;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        protected readonly IMapper Mapper;

        protected BaseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            Mapper = mapper;
        }

        protected IActionResult Map<T>(object source)
        {
            return Ok(Mapper.Map<T>(source));
        }

        protected IActionResult CommitAndMap<T>(object source)
        {
            unitOfWork.Commit();

            return Map<T>(source);
        }

        protected IActionResult Commit()
        {
            unitOfWork.Commit();
            return Ok();
        }
    }
}
