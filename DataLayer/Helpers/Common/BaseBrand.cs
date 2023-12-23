using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Base
{
    public static class BaseBrand
    {
        public static string GetBrand(this Product product)
        {
            if (product.BrandId !=null)
            {
                return product.ProductBrand.Name;
            }
            else
                return null;
        }


    }
}
