namespace CompanySystem.Server.DataTransferModels.Votes
{
    using System;
    using AutoMapper;
    using CompanySystem.Data.Models.Models;
    using CompanySystem.Server.Common.Mappings.Contracts;

    public class VoteDetailsDataTransferModel : IMapFrom<Vote>, IHaveCustomMappings
    {
        public string UserVoted { get; set; }

        public string BirthdayPresentDescription { get; set; }

        public int EventId { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Vote, VoteDetailsDataTransferModel>()
                .ForMember(dest => dest.BirthdayPresentDescription, opt => opt.MapFrom(src => src.Present.Description))
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.BirthdayPresentEventId))
                .ForMember(dest => dest.UserVoted, opt => opt.MapFrom(src => src.UserVoted.UserName));
        }
    }
}
