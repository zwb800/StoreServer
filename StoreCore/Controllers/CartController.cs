using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreCore.Models;
using static StoreCore.Controllers.ProductController;

namespace StoreCore.Controllers
{
    [Route("[controller]")]
    public class CartController : Controller
    {
        storeContext storeContext = new storeContext();
        public class CartViewModel : SkuViewModel
        {
            public int Count { get; set; }
        }

        [Route("[action]")]
        public string DeleteCart(int userid, int skuid)
        {
            var cart = storeContext.Cart.SingleOrDefault(p => p.UserId == userid && p.Skuid == skuid);
            if (cart != null)
            {
                storeContext.Cart.Remove(cart);
                storeContext.SaveChanges();
                return "success";
            }

            return "failed";

        }

        [Route("[action]")]
        public string UpdateCart(int userid, int skuid, int count)
        {
            var cart = storeContext.Cart.SingleOrDefault(p => p.UserId == userid && p.Skuid == skuid);
            if (cart != null)
            {
                cart.Count = count;
                storeContext.SaveChanges();
                return "success";
            }

            return "failed";

        }

        [Route("[action]")]
        public IEnumerable<CartViewModel> Cart(int userid)
        {
            var carts = storeContext.Cart.Where(p => p.UserId == userid).ToList();
            var skuids = carts.Select(p => p.Skuid).ToArray();

            var list = storeContext.Sku.Where(p => skuids.Contains(p.Id)).Select(p => new CartViewModel
            {
                SKUName = p.Name,
                Price = p.Price,
                ProductID = p.ProductId,
                ID = p.Id,

            }).ToList();

            foreach (var item in list)
            {
                var product = storeContext.Product.FirstOrDefault(p => p.Id == item.ProductID);
                item.Count = carts.Single(p => p.Skuid == item.ID).Count;
                item.Title = product.Title;
                item.Img = product.Img;
            }

            return list;
        }

        [Route("[action]")]
        public string AddCart(int userid, int skuid)
        {
            var cart = storeContext.Cart.FirstOrDefault(p => p.UserId == userid && p.Skuid == skuid);

            if (cart != null)
            {
                cart.Count++;
            }
            else
            {
                storeContext.Cart.Add(new Cart()
                {
                    UserId = userid,
                    Skuid = skuid,
                    Count = 1,
                });
            }

            storeContext.SaveChanges();
            return "success";
        }

    }
}