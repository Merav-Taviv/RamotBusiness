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

        private RamotBusinessContext context;
        public void MyProperty()
        {
        }
        public ProviderRepository(RamotBusinessContext context)
        {
            this.context = context;
        }

        public ProviderRepository()
        {
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


        public void DeleteProvider(int providerId)
        {

            foreach (Provider item in context.Provider)
            {
                if (item.ProviderId == providerId)
                {
                    context.Provider.Remove(item);
                }
            }
            context.SaveChanges();

        }
        public List<ProviderCommon> GetProviderByCategory(int categoryId)
        {
            List<Provider> ProvidersByCategory = new List<Provider>();
            foreach (Provider item in context.Provider)
            {
                if (item.CategoryId == categoryId)
                {
                    ProvidersByCategory.Add(item);
                }
            }
            return ProviderMap.MapListFilesToFileCommon(ProvidersByCategory);
        }

        void IProviderRepository.AddProvider(Provider provider)
        {
            throw new NotImplementedException();
        }

        public List<ProviderCommon> ProvidersByCategory(int providerId)
        {
            throw new NotImplementedException();
        }
    }
}