namespace CompanySystem.Server.DataTransferModels.BirthdayPresentEvent
{
    using Common.Mappings.Contracts;
    using Data.Models.Models;
    using System;
    using System.Collections.Generic;
    using Votes;
    using AutoMapper;

    public class BirthdayPresentEventDataTransferModel : IMapFrom<BirthdayPresentEvent>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public DateTime BirthdayDate { get; set; }

        public bool IsActive { get; set; }

        public string CreatorUsername { get; set; }

        public string BirthdayGuyUsername { get; set; }

        public ICollection<VoteDetailsDataTransferModel> Votes { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<BirthdayPresentEvent, BirthdayPresentEventDataTransferModel>()
                .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes))
                .ForMember(dest => dest.CreatorUsername, opt => opt.MapFrom(src => src.Creator.UserName))
                .ForMember(dest => dest.BirthdayGuyUsername, opt => opt.MapFrom(src => src.BirthdayGuy.UserName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BirthdayDate, opt => opt.MapFrom(src => src.BirthdayDate))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
