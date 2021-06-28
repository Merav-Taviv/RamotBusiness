using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class FileRepository : IFileRepository
    {

        private Context1 context;
        public void MyProperty()
        {
        }
        public FileRepository(Context1 context)
        {
            this.context = context;
        }

        public FileRepository()
        {
        }

        public void AddFile(Files file)
        {
            context.Files.Add(file);
            context.SaveChanges();
        }

        public void UpdateFile(Files file)
        {
            Files x = context.Files.Where(a => a.FileId == file.FileId).First();
            x.FileId = file.FileId;
            x.FileName = file.FileName;
            x.LocalX = file.LocalX;
            x.LocalY = file.LocalY;
            x.Width = file.Width;
            x.Height = file.Height;
            context.Files.Update(x);
            context.SaveChanges();
        }


        public void DeleteFile(int formID)
        {

            foreach (Files item in context.Files)
            {
                if (item.FormId == formID)
                {
                    context.Files.Remove(item);
                }
            }
            context.SaveChanges();

        }
        public List<FileCommon> GetFilesByForm(int formID)
        {
            List<Files> FileName = new List<Files>();
            foreach (Files item in context.Files)
            {
                if (item.FormId == formID)
                {
                    FileName.Add(item);
                }
            }
            return FileMap.MapListFilesToFileCommon( FileName);
        }
    }
}