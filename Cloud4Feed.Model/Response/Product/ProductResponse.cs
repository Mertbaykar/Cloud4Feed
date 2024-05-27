﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud4Feed.Model.Response.Product
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get;  set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public ProductCategoryResponse Category { get; set; }
    }

    public class ProductCategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
