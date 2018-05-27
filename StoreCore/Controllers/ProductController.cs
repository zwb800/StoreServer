using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreCore.Models;

namespace StoreCore.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        storeContext storeContext = new storeContext();
        public IEnumerable<Product> Get(int page = 1)
        {
            return storeContext.Product.OrderByDescending(p => p.Id).Skip(page - 1).Take(8);
        }
        [Route("[action]")]
        public ConfirmViewModel Confirm(int userid,int skuid) {
            var confirm = new ConfirmViewModel()
            {
                Address = storeContext.Address.FirstOrDefault(p=>p.UserId == userid),
                Skus = GetBySKUID(new[] { skuid}),
            };
            return confirm;
        }

        public class ConfirmViewModel
        {
            public Address Address { get; internal set; }
            public IEnumerable<SkuViewModel> Skus { get; internal set; }
        }

        [Route("[action]")]
        public IEnumerable<SkuViewModel> GetBySKUID(int[] id)
        {
            var list = storeContext.Sku.Where(p => id.Contains(p.Id)).Select(p=> new  SkuViewModel{
                SKUName = p.Name,
                Price = p.Price,
                ProductID = p.ProductId,
                ID = p.Id,

            }).ToList();

            foreach (var item in list)
            {
                var product = storeContext.Product.FirstOrDefault(p => p.Id == item.ProductID);

                item.Title = product.Title;
                item.Img = product.Img;
            }
           

            return list;
        }

        public class SkuViewModel
        {
            public string SKUName { get; internal set; }
            public decimal Price { get; internal set; }
            public uint ProductID { get; internal set; }
            public string Title { get; internal set; }
            public string Img { get; internal set; }
            public int ID { get; internal set; }
        }

        [Route("[action]")]
        public ProductViewModel GetByID(int id)
        {
            var product = storeContext.Product.FirstOrDefault(p =>p.Id == id); 
            if(product!=null)
            {
                return new ProductViewModel() {
                    Title = product.Title,
                    Imgs = product.Imgs.Split(","),
                    SKUS = storeContext.Sku.Where(p=>p.ProductId == product.Id),
                    Detail = product.Detial,
                };
            }

            return null;
        }

        public class ProductViewModel {
            public string Title { get; internal set; }
            public string[] Imgs { get; internal set; }
            public IEnumerable<Sku> SKUS { get; internal set; }
            public string Detail { get; internal set; }
        }
    }
}