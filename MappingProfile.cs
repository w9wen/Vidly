using AutoMapper;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //// Domain to Dto
            CreateMap<Customer, CustomerDto>();
            CreateMap<Movie, MovieDto>();
            CreateMap<MembershipType, MembershipTypeDto>();
            CreateMap<Genre, GenreDto>();

            //// Dto to Domain
            CreateMap<CustomerDto, Customer>();
            CreateMap<MovieDto, Movie>();  


            // CreateMap<CustomerDto, Customer>()
            //     .ForMember(c => c.Id, opt => opt.Ignore());
            // CreateMap<MovieDto, Movie>()
            //     .ForMember(c => c.Id, opt => opt.Ignore());

        }
    }
}