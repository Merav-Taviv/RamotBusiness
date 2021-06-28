using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
      
         RamotBusinessContext context;
       
        public CategoryRepository(RamotBusinessContext context)
        {
            this.context = context;
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

    }
}



