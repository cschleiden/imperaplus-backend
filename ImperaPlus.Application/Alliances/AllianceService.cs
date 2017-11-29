using System;
using System.Collections.Generic;
using AutoMapper;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO.Alliances;

namespace ImperaPlus.Application.Alliances
{
    public interface IAllianceService
    {
        /// <summary>
        /// Create new alliance
        /// </summary>
        Alliance Create(AllianceCreationOptions creationOptions);

        /// <summary>
        /// Get summary of all alliances
        /// </summary>
        IEnumerable<AllianceSummary> GetAll();

        /// <summary>
        /// Get single alliance with members
        /// </summary>
        /// <param name="allianceId">Id of alliance</param>
        Alliance Get(Guid allianceId);
    }

    public class AllianceService : BaseService, IAllianceService
    {
        private readonly Domain.Alliances.IAllianceService allianceService;

        public AllianceService(IUnitOfWork unitOfWork, IUserProvider userProvider, Domain.Alliances.IAllianceService allianceService)
            : base(unitOfWork, userProvider)
        {
            this.allianceService = allianceService;
        }

        public Alliance Create(AllianceCreationOptions creationOptions)
        {
            var alliance = this.allianceService.Create(
                this.CurrentUser, 
                creationOptions.Name, 
                creationOptions.Description);

            this.UnitOfWork.Alliances.Add(alliance);
            this.UnitOfWork.Commit();

            return Mapper.Map<DTO.Alliances.Alliance>(alliance);
        }

        public Alliance Get(Guid allianceId)
        {
            var alliance = this.allianceService.Get(allianceId);

            return Mapper.Map<Alliance>(alliance);
        }

        public IEnumerable<AllianceSummary> GetAll()
        {
            var alliances = this.allianceService.GetAll();

            return Mapper.Map<IEnumerable<AllianceSummary>>(alliances);
        }
    }
}
