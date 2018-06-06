using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreCore.Models;
using static StoreCore.Controllers.CartController;

namespace StoreCore.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        storeContext storeContext = new storeContext();
        private readonly string SECRET= "ac071ee4c726509ced8adeee151092aa";
        private readonly string APPID= "wx68c8c33631a7e4fa";

        [Route("[action]")]
        public User Login(string code) {

            var url = "https://api.weixin.qq.com/sns/jscode2session?appid="+APPID+"&secret="+SECRET+"&js_code=" +
                code+"&grant_type=authorization_code";
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetStringAsync(url).Result;

            var re = JsonConvert.DeserializeObject<OpenIDResult>(result);
            User user = null;
            if(!string.IsNullOrEmpty(re.OpenID))
            {
                user = storeContext.User.FirstOrDefault(p => p.OpenId == re.OpenID);
                if (user != null) {

                }
            }
            return user;
        }

        public class OpenIDResult {
            public string OpenID { get; set; }
        }

        [Route("[action]")]
        public IEnumerable<ViewOrder> Index(int userid)
        {
            var orders = storeContext.Order.Where(p => p.UserId == userid).Select(p=> new ViewOrder{ID = p.Id,Status = Enum.Parse<Status>( p.Status.ToString()) }).ToList();

            foreach (var o in orders)
            {
                o.Skus = storeContext.Ordersku.Where(p => p.OrderId == o.ID).Select(p=>new CartViewModel
                {
                    Count = p.Count,
                    ID = p.Skuid,
                    Price = 100,
                }).ToArray();
                foreach (var item in o.Skus)
                {
                    var sku = storeContext.Sku.SingleOrDefault(p => p.Id == item.ID);
                    var product = storeContext.Product.FirstOrDefault(p => p.Id == sku.ProductId);
                    item.SKUName = sku.Name;
                    item.Price = sku.Price;
                    item.Title = product.Title;
                    item.Img = product.Img;
                }
            }

            return orders;

        }

        public class ViewOrder
        {
            public int ID { get; internal set; }
            public CartViewModel[] Skus { get; set; }
            public Status Status { get; set; }
        }

        public enum Status {
            等待发货 = 0,
            正在配送,
            已收货
        }
    }
}