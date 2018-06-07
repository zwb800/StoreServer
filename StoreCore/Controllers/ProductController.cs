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

        public object ID { get; private set; }

        public IEnumerable<ProductListViewModel> Get(int page = 1)
        {
            var list = storeContext.Product.OrderByDescending(p => p.Id).Skip(page - 1).Take(8).Select(p=> new ProductListViewModel { ID=p.Id,Name = p.Name,Img = p.Img }).ToList();

            foreach (var item in list)
            {
                var sku = storeContext.Sku.Where(p => p.ProductId == item.ID).OrderBy(p=>p.Price).FirstOrDefault();
                if (sku != null)
                    item.Price = sku.Price;
            }

            return list;
        }
        public class ProductListViewModel
        {
            public string Img { get; internal set; }
            public string Title { get; internal set; }
            public int ID { get; internal set; }
            public decimal Price { get; internal set; }
            public string Name { get; internal set; }
        }

        [Route("[action]")]
        public string ShippingOrder([FromBody] OrderData orderData ) {
            var address = orderData.address;
            address.UserId = orderData.UserID;
            if (!storeContext.Address.Any(p => 
            p.UserId == address.UserId&&
            p.UserName == address.UserName && 
            p.TelNumber == address.TelNumber &&
             p.DetailInfo == address.DetailInfo && 
             p.CountyName == address.CountyName && 
             p.CityName == address.CityName && 
             p.ProvinceName == address.ProvinceName)){
                storeContext.Address.Add(address);
                storeContext.SaveChanges();
            }

            var order = new Order()
            {
                CreateTime = DateTime.Now,
                UserId = orderData.UserID,
                AddressId = address.Id,
            };
            storeContext.Order.Add(order);
            storeContext.SaveChanges();
            foreach (var sku in orderData.skus)
            {
                storeContext.Ordersku.Add(new Ordersku() {
                    Skuid = sku.ID,
                    Count = sku.Count,
                    OrderId = order.Id,

                });
            }
            storeContext.SaveChanges();
            return "success";
        }

        public class OrderData {
            public Address address { get; set; }
            public SkuData[] skus { get; set; }
            public int UserID { get; set; }
        }

        public class SkuData {
            public int ID { get; set; }
            public int Count { get; set; }
        }



        [Route("[action]")]
        public ConfirmViewModel Confirm(int userid,string skuid) {

            var confirm = new ConfirmViewModel()
            {
                Address = storeContext.Address.FirstOrDefault(p=>p.UserId == userid),
                Skus = GetBySKUID(skuid.Split(",").Where(p=>!string.IsNullOrEmpty(p)).Select(p=> int.Parse(p)).ToArray()),
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
            public int ProductID { get; internal set; }
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