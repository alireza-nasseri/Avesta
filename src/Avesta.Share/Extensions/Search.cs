﻿using Avesta.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avesta.Share.Extensions
{
    public static class SearchExtension
    {
        public static void GetSearchParam(this string search, out List<string> searchParams)
        {
            searchParams = new List<string>();
            searchParams.AddRange(search.Split(Character.SearchSpliterChar).ToList());
        }



        //TODO : complete this function 
        //public static T IsItInTimeInterval<T>(this T data,DateTime? startDate = null, DateTime? endDate = null) where T : BaseEntity


    }

}
