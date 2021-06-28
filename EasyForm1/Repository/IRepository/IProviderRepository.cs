using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public interface IProviderRepository
    {
        void AddProvider(Provider provider);
        void UpdateProvider(Provider provider);
        void DeleteProvider(int providerId);
        List<ProviderCommon> ProvidersByCategory(int providerId);
    }
}
