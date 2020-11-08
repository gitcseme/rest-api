using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterBook2.Contracts.V1.Responses;
using TwitterBook2.Domain;

namespace TwitterBook2.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.Tags, optiion =>
                    optiion.MapFrom(src => src.Tags.Select(p => new TagResponse { Name = p.TagName })));

            CreateMap<Tag, TagResponse>();
        }
    }
}
