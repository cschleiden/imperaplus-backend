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
            this.Mapper = mapper;
        }

        protected IActionResult Map<T>(object source)
        {
            return this.Ok(this.Mapper.Map<T>(source));
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