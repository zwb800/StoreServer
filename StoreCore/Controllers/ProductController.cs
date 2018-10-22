using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using StoreCore.Models;

namespace StoreCore.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        public ProductController(IHttpContextAccessor accessor)
        {
            Accessor = accessor;
        }

        storeContext storeContext = new storeContext();

        public object ID { get; private set; }
        public IHttpContextAccessor Accessor { get; set; }

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
            SaveAddress(address);

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

        [Route("[action]")]
        public Unified WxPayOrder([FromBody] OrderData orderData)
        {
            var address = orderData.address;
            address.UserId = orderData.UserID;
            SaveAddress(address);

            var order = new Order()
            {
                CreateTime = DateTime.Now,
                UserId = orderData.UserID,
                AddressId = address.Id,
            };
            storeContext.Order.Add(order);
            storeContext.SaveChanges();

            decimal sumPrice = 0;

            var skus = storeContext.Sku.Where(p => orderData.skus.Select(p1 => p1.ID).Contains(p.Id)).ToArray();

            foreach (var sku in orderData.skus)
            {
                sumPrice += (skus.Single(p => p.Id == sku.ID).Price * sku.Count);

                storeContext.Ordersku.Add(new Ordersku()
                {
                    Skuid = sku.ID,
                    Count = sku.Count,
                    OrderId = order.Id,

                });


            }
            storeContext.SaveChanges();

    
            return WxPay("",(int)(sumPrice*100),"");
        }

        private void SaveAddress(Address address) {
            if (!storeContext.Address.Any(p =>
            p.UserId == address.UserId &&
            p.UserName == address.UserName &&
            p.TelNumber == address.TelNumber &&
             p.DetailInfo == address.DetailInfo &&
             p.CountyName == address.CountyName &&
             p.CityName == address.CityName &&
             p.ProvinceName == address.ProvinceName))
            {
                storeContext.Address.Add(address);
                storeContext.SaveChanges();
            }
        }


        private TenPayV3Info TenPayV3Info = new TenPayV3Info("wxd386c7dc2ba5ab39", "5068dce882d9eb962bdeb1fd0ad32d33", "1499945642", "wLClWzYk5v6hTCLvmXHUmHVQRNwzAMdD", "https://www.xianpinduo.cn/Product/Notify");

        [Route("[action]")]
        public Unified WxPay(string body,int price,string openId) {
            var timeStamp = TenPayV3Util.GetTimestamp();
            var nonceStr = TenPayV3Util.GetNoncestr();

            //生成订单10位序列号，此处用时间和随机数生成，商户根据自己调整，保证唯一
            var sp_billno = string.Format("{0}{1}{2}", TenPayV3Info.MchId/*10位*/, DateTime.Now.ToString("yyyyMMddHHmmss"),
                TenPayV3Util.BuildRandomStr(6));
            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(
                TenPayV3Info.AppId, 
                TenPayV3Info.MchId, 
                body, 
                sp_billno, 
                price,
                Accessor.HttpContext.UserHostAddress().ToString(), 
                TenPayV3Info.TenPayV3Notify, 
                TenPayV3Type.JSAPI, 
                openId, 
                TenPayV3Info.Key, 
                nonceStr);

            var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口
                                                            //JsSdkUiPackage jsPackage = new JsSdkUiPackage(TenPayV3Info.AppId, timeStamp, nonceStr,);
                                                           
            var package = string.Format("prepay_id={0}", result.prepay_id);

            return new Unified {
                timeStamp = timeStamp,
                nonceStr = nonceStr,
                package = package,
                paySign = TenPayV3.GetJsPaySign(TenPayV3Info.AppId, timeStamp, nonceStr, package, TenPayV3Info.Key)
            };
        }

        [Route("[action]")]
        public ActionResult Notify() {
            ResponseHandler resHandler = new ResponseHandler(HttpContext);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");

            string res = null;

            resHandler.SetKey(TenPayV3Info.Key);
            //验证请求是否从微信发过来（安全）
            if (resHandler.IsTenpaySign() && return_code.ToUpper() == "SUCCESS")
            {
                res = "success";//正确的订单处理
                                //直到这里，才能认为交易真正成功了，可以进行数据库操作，但是别忘了返回规定格式的消息！
            }
            else
            {
                res = "wrong";//错误的订单处理
            }

            string xml = string.Format(@"<xml>
                                        <return_code><![CDATA[{0}]]></return_code>
                                        <return_msg><![CDATA[{1}]]></return_msg>
                                        </xml>", return_code, return_msg);
            return Content(xml, "text/xml");
        }



        public class Unified {
            public string timeStamp { get; set; }
            public string nonceStr { get; set; }

            public string package { get; set; }

            public string paySign { get; set; }
            public UnifiedorderResult result { get; internal set; }
            public int price { get; internal set; }
            public string body { get; internal set; }
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