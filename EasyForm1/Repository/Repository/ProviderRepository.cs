using AutoMapper;
using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class ProviderRepository : IProviderRepository
    {
        IMapper mapper;
        RamotBusinessContext context;
        public ProviderRepository(RamotBusinessContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public Boolean AddProvider(Provider provider)
        {
            if (context.Provider.Contains(provider))
            {
                return false;
            }

            context.Provider.Add(provider);
            return context.SaveChanges() > 0;
        }
        public void UpdateProvider(Provider provider)
        {
            Provider p = context.Provider.Where(a => a.ProviderId == provider.ProviderId).First();
            p.ProviderName = provider.ProviderName;
            p.Neighborhood = provider.Neighborhood;
            p.Address = provider.Address;
            p.Phone = provider.Phone;
            p.Mobile = provider.Mobile;
            p.CategoryId = provider.CategoryId;
            p.Pictuer = provider.Pictuer;
            context.Provider.Update(p);
            context.SaveChanges();
        }
        public bool DeleteProvider(int providerId)
        {

            foreach (Provider item in context.Provider)
            {
                if (item.ProviderId == providerId)
                {
                    context.Provider.Remove(item);
                }
            }
            return context.SaveChanges() > 0;

        }
        public List<ProviderCommon> GetProvidersByCategory(int categoryId)
        {
            List<Provider> listProvidersByCategory = new List<Provider>();
            foreach (Provider item in context.Provider)
            {
                if (item.CategoryId == categoryId)
                {
                    listProvidersByCategory.Add(item);
                }
            }
            return mapper.Map<List<ProviderCommon>>(listProvidersByCategory);
        }
        public List<ProviderCommon> GetProvidersByNeighborhood(string neighborhood)
        {
            List<Provider> listProviderByNeighborhood = new List<Provider>();
            foreach (Provider item in context.Provider)
            {
                if (item.Neighborhood.Equals(neighborhood))
                {
                    listProviderByNeighborhood.Add(item);
                }
            }
            return mapper.Map<List<ProviderCommon>>(listProviderByNeighborhood);
        }

    }
}