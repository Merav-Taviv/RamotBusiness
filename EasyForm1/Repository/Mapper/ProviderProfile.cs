using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Common;
using AutoMapper;

namespace Repository
{
    public class ProviderProfile : Profile
    {
        public ProviderProfile()
        {
            CreateMap<Provider, ProviderCommon>().ReverseMap();
            CreateMap<List<Provider>, List<ProviderCommon>>().ReverseMap();
        }
    }
}
