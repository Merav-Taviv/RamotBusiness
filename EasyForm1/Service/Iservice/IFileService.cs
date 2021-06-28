using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Iservice
{
    public interface IFileService
    {
        void AddFile(ProviderCommon fileCommon);
        List<ProviderCommon> GetFilesByForm(int ID);
    }
}
