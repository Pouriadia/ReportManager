using AutoMapper;
using ReportManager.Application.DTOs;
using ReportManager.Domain.Entities;

namespace ReportManager.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain → DTO
            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Reporter, ReporterDto>().ReverseMap();
        }
    }
}