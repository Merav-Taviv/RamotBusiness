using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public interface IProviderRepository
    {
        bool AddProvider(Provider provider);
        bool UpdateProvider(Provider provider);
        bool DeleteProvider(int providerId);
        List<ProviderCommon> GetProvidersByCategory(int categoryId);
        List<ProviderCommon> GetProvidersByNeighborhood(string neighborhood);

    }
}
