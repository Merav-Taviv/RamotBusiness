using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Repository
{
    public static class FileMap
    {
        public static FileCommon MapFilesToFileCommon(Files file)
        {
            FileCommon fileCommon = new FileCommon();
            if (file != null)
            {
                fileCommon.FileId = file.FileId;
                fileCommon.FileName = file.FileName;
                fileCommon.FormId = file.FormId;
                fileCommon.Height = file.Height;
                fileCommon.Width = file.Width;
                fileCommon.LocalX = file.LocalX;
                fileCommon.LocalY = file.LocalY;
            }
            return fileCommon;
        }
        public static List<FileCommon> MapListFilesToFileCommon(List<Files> fileList)
        {
            List<FileCommon> fileCommonList = new List<FileCommon>();
            if (fileCommonList != null)
            {
                foreach (Files item in fileList)
                {
                    fileCommonList.Add(MapFilesToFileCommon(item));
                }
            }
            return fileCommonList;
        }

        public static Files MapFileCommonToFiles(FileCommon fileCommon)
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
        public static List<Files> MapListFileCommonToFiles(List<FileCommon> fileCommonList)
        {
            List<Files> fileList = new List<Files>();
            if (fileCommonList != null)

            {
                foreach (FileCommon item in fileCommonList)
                {
                    fileList.Add(MapFileCommonToFiles(item));
                }
            }
            return fileList;
        }

    }
}
