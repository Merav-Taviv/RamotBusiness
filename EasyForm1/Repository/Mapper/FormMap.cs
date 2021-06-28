using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Repository
{
    public static class FormMap
    {
        public static FormCommon MapFormsToFormCommon(Forms form)
        {
            FormCommon formCommon = new FormCommon();
            if (form != null)
            {
                formCommon.FormId = form.FormId;
                formCommon.FormName = form.FormName;
                formCommon.AmountOfUsing = form.AmountOfUsing;
                formCommon.LastUsing = form.LastUsing;
                formCommon.Sharing = form.Sharing;
                formCommon.UserId = form.UserId;
                formCommon.ImagePath = form.ImagePath;
            }
            return formCommon;
        }
        public static List<FormCommon> MapListFormsToFormCommon(List<Forms> formList)
        {
            List<FormCommon> formCommonList = new List<FormCommon>();
            if (formList != null)
            {
                foreach (Forms item in formList)
                {
                    formCommonList.Add(MapFormsToFormCommon(item));
                }
            }
            return formCommonList;
        }

        public static Forms MapFormCommonToForms(FormCommon formCommon)
        {
            Forms form = new Forms();
            if (formCommon != null)
            {
                form.FormId = formCommon.FormId;
                form.FormName = formCommon.FormName;
                form.AmountOfUsing = formCommon.AmountOfUsing;
                form.LastUsing = formCommon.LastUsing;
                form.Sharing = formCommon.Sharing;
                form.UserId = formCommon.UserId;
                form.ImagePath = form.ImagePath;
            }
            return form;
        }
        public static List<Forms> MapListFormCommonToForms(List<FormCommon> formCommonList)
        {
            List<Forms> formList = new List<Forms>();
            if (formCommonList != null)
            {
                foreach (FormCommon item in formCommonList)
                {
                    formList.Add(MapFormCommonToForms(item));
                }
            }
            return formList;
        }

    }
}
