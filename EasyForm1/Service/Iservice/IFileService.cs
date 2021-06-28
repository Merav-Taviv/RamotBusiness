using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Iservice
{
    public interface IFileService
    {
        void AddFile(FileCommon fileCommon);
        List<FileCommon> GetFilesByForm(int ID);
    }
}
