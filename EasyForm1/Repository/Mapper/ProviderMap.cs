using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Repository
{
    public static class ProviderMap
    {
        public static ProviderCommon MapProviderToProvierCommon(Provider provider)
        {
            ProviderCommon providerCommon = new ProviderCommon();
            if (provider != null)
            {
                providerCommon.ProviderName = provider.ProviderName;
                providerCommon.Neighborhood = provider.Neighborhood;
                providerCommon.Address = provider.Address;
                providerCommon.Phone = provider.Phone;
                providerCommon.Mobile = provider.Mobile;
                providerCommon.CategoryId = provider.CategoryId;
                providerCommon.Pictuer = provider.Pictuer;
            }
            return providerCommon;
        }
        public static List<ProviderCommon> MapListFilesToFileCommon(List<Provider> providerList)
        {
            List<ProviderCommon> providerCommonList = new List<ProviderCommon>();
            if (providerCommonList != null)
            {
                foreach (Provider item in providerList)
                {
                    providerCommonList.Add(MapFilesToFileCommon(item));
                }
            }
            return providerCommonList;
        }

        public static Files MapFileCommonToFiles(ProviderCommon fileCommon)
        {
            Files file = new Files();
            if (fileCommon != null)
            {
                file.FileId = fileCommon.FileId;
                file.FileName = fileCommon.FileName;
                file.FormId = fileCommon.FormId;
                file.Height = fileCommon.Height;
                file.Width = fileCommon.Width;
                file.LocalX = fileCommon.LocalX;
                file.LocalY = fileCommon.LocalY;
            }
            return file;
        }
        public static List<Files> MapListFileCommonToFiles(List<ProviderCommon> fileCommonList)
        {
            List<Files> fileList = new List<Files>();
            if (fileCommonList != null)

            {
                foreach (ProviderCommon item in fileCommonList)
                {
                    fileList.Add(MapFileCommonToFiles(item));
                }
            }
            return fileList;
        }

    }
}
