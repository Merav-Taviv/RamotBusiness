using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Common;
using AutoMapper;
namespace Repository
{

        public class CustomerProfile : Profile
        {
            public CustomerProfile()
            {
                CreateMap<Customer,CustomerCommon>().ReverseMap();
                CreateMap<List<Customer>, List<CustomerCommon>>().ReverseMap();
            }
        }

    
        
}
