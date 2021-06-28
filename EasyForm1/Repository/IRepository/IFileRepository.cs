using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public interface IFileRepository
    {
        void AddFile(Files file);
        void UpdateFile(Files file);
        void DeleteFile(int formID);
        List<FileCommon> GetFilesByForm(int formID);
    }
}
