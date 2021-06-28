using Common;
using Repository;
using Repository.Models;
using Service.Iservice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    class FileService :IFileService
    {
        private IFileRepository repository;

        public FileService(IFileRepository repository)
        {
            this.repository = repository;
        }
        public void AddFile(FileCommon fileCommon)
        {
            repository.AddFile(FileMap.MapFileCommonToFiles( fileCommon));
        }
        public List<FileCommon> GetFilesByForm(int ID)
        {
            return repository.GetFilesByForm(ID);
        }






    }
}
