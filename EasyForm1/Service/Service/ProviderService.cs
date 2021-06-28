using AutoMapper;
using Common;
using Repository;
using Repository.Models;
using Service.Iservice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    class ProviderService : IProviderService
    {
        IMapper mapper;
        IProviderRepository repository;
        public ProviderService(IProviderRepository repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }
        public bool AddProvider(ProviderCommon providerCommon)
        {
            var provider = mapper.Map<Provider>(providerCommon);
            return repository.AddProvider(provider);

        }
        public bool UpdateProvider(ProviderCommon providerCommon)
        {
            var provider = mapper.Map<Provider>(providerCommon);
            return repository.UpdateProvider(provider);
        }
        public bool DeleteProvider(int providerId)
        {

            return repository.DeleteProvider(providerId);
        }
        public List<ProviderCommon> GetProvidersByCategory(int categoryId)
        {
            return repository.GetProvidersByCategory(categoryId);
        }

    }
}
