﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using Inspinia_MVC5_SeedProject.Models;
using System.Text.RegularExpressions;
using AngleSharp;

namespace Inspinia_MVC5_SeedProject.CodeTemplates
{
    
    public class MobilesTabletsController : Controller
    {
        private Entities db = new Entities();
        public ElectronicsController electronicController = new ElectronicsController();
        // GET: /MobilesTablets/
        [Route("MobilesTablets")]
        public async Task<ActionResult> Index(string brand,string model, string category, string q = "", string tags = null, int minPrice = 0, int maxPrice = 50000,string condition = null,int? page = null)
        {
            if(category == null) { category = "Mobile Phones"; }
            string city = Session["City"] == null || Session["City"].ToString() == "All Pakistan" ? null : Session["City"].ToString();
            var result = await searchMobiles(brand,model, q, tags, minPrice, maxPrice, city, null, 50000, category, condition);
            ViewBag.category = category;
            var pager = new Pager(result.Count(), page);

            var viewModel = new ListViewModel
            {
                ItemsCount = result.Count,
                Items = result.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
            return View(viewModel);
            //return View(result);
        }
        public async Task<List<ListAdView>> searchMobiles(string brand,string model, string q = "", string tags = null, int minPrice = 0, int maxPrice = 10000, string city = null, string pp = null, int fixedMaxPrice = 10000,string category = "Mobiles", string newOrUsed = null)
        {
            if (tags != null && tags != "")
            {
                q = q + " " + tags;
            }
            if(model != null && model != "")
            {
                q = q + " " + model;
            }
            q = Regex.Replace(q, @"\b(" + string.Join("|", "sale") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "is") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "for") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "i") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "a") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "an") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "want") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "buy") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "sell") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "who") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "and") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "or") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "are") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "you") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "me") + @")\b", string.Empty, RegexOptions.IgnoreCase);
            q = Regex.Replace(q, @"\b(" + string.Join("|", "my") + @")\b", string.Empty, RegexOptions.IgnoreCase);

            string[] q_array = q.Split(' ');

         //   if (tags == null || tags == "")
            {
                //append model in query
                List<ListAdView> temp =await (from ad in db.MobileAds
                                        where ((q == null || q == "" || q_array.Any(x => ad.Ad.title.Contains(x))) && ad.Ad.subcategory.Equals(category) && (newOrUsed == null || newOrUsed == "" || newOrUsed == ad.Ad.condition) && ad.Ad.status.Equals("a") && (model == null || model == "" || model == "undefined" || true /*ad.MobileModel.model.Equals(model)*/ || ad.Ad.title.Contains(model) || ad.Ad.description.Contains(model)) && (brand == null || brand == "" || brand == "undefined" || ad.MobileModel.Mobile.brand.Equals(brand)) && (minPrice == 0 || ad.Ad.price > minPrice) && (maxPrice == 50000 || ad.Ad.price < maxPrice) && (city == null || city == "" || city == "undefined" || ad.Ad.AdsLocation.City.cityName.Equals(city) && (pp == null || pp == "" || pp == "undefined" || ad.Ad.AdsLocation.popularPlace.name.Equals(pp))))
                                        orderby ad.Ad.time descending
                                        select new ListAdView
                                        {
                                            title = ad.Ad.title,
                                            description = ad.Ad.description,
                                            Id = ad.Ad.Id,
                                            time = ad.Ad.time,
                                            category = ad.Ad.category,
                                            subcategory = ad.MobileModel.Mobile.brand,
                                            isnegotiable = ad.Ad.isnegotiable,
                                            price = ad.Ad.price,
                                            subsubcategory = ad.MobileModel.model,
                                           // reportedCount = ad.Ad.Reporteds.Count,
                                            //isReported = ad.Ad.Reporteds.Any(x => x.reportedBy == islogin),
                                            views = ad.Ad.views,
                                            condition = ad.Ad.condition,
                                           // savedCount = ad.Ad.SaveAds.Count,
                                           // MobileAd = ad,
                                            AdTags = ad.Ad.AdTags,
                                            AdImages = ad.Ad.AdImages,
                                            AdsLocation = ad.Ad.AdsLocation,
                                            bidsCount = ad.Ad.Bids.Count,
                                            maxBid = ad.Ad.Bids.OrderByDescending(x => x.price).FirstOrDefault()

                                        }).ToListAsync();
                return temp;
            }
       //     else
            {
                string[] tagsArray = null;
                if (tags != null)
                {
                    tagsArray = tags.Split(',');
                }
                List<ListAdView> ads = await (from ad in db.MobileAds
                                              where (ad.Ad.subcategory.Equals(category) && (!tagsArray.Except(ad.Ad.AdTags.Select(x => x.Tag.name)).Any()) && ad.Ad.status.Equals("a") && (model == null || model == "" || model == "undefined" || ad.MobileModel.model.Equals(model)) && (brand == null || brand == "" || brand == "undefined" || ad.MobileModel.Mobile.brand.Equals(brand)) && (minPrice == 0 || ad.Ad.price > minPrice) && (maxPrice == 50000 || ad.Ad.price < maxPrice) && (city == null || city == "" || city == "undefined" || ad.Ad.AdsLocation.City.cityName.Equals(city) && (pp == null || pp == "" || pp == "undefined" || ad.Ad.AdsLocation.popularPlace.name.Equals(pp))))
                                              orderby ad.Ad.time descending
                                              select new ListAdView
                                              {
                                                  title = ad.Ad.title,
                                                  description = ad.Ad.description,
                                                  Id = ad.Ad.Id,
                                                  time = ad.Ad.time,
                                                  category = ad.Ad.category,
                                                  subcategory = ad.MobileModel.Mobile.brand,
                                                  isnegotiable = ad.Ad.isnegotiable,
                                                  price = ad.Ad.price,
                                                  subsubcategory = ad.MobileModel.model,
                                                  // reportedCount = ad.Ad.Reporteds.Count,
                                                  //isReported = ad.Ad.Reporteds.Any(x => x.reportedBy == islogin),
                                                  views = ad.Ad.views,
                                                  condition = ad.Ad.condition,
                                                  // savedCount = ad.Ad.SaveAds.Count,
                                                  AdTags = ad.Ad.AdTags,
                                                  AdImages = ad.Ad.AdImages,
                                                  AdsLocation = ad.Ad.AdsLocation,
                                                  bidsCount = ad.Ad.Bids.Count,
                                                  maxBid = ad.Ad.Bids.OrderByDescending(x => x.price).FirstOrDefault()

                                              }).ToListAsync();
                return ads;
            }
        }
        public ActionResult data()
        {
            return View("NewMobiles");
        }
        public async Task<PartialViewResult> NewMobilePartialView(string link)
        {
            NewMobileViewModel mobile = new NewMobileViewModel();
            try
            {
                //change url in mvc controller
                var config = AngleSharp.Configuration.Default.WithDefaultLoader();
                var document = await BrowsingContext.New(config).OpenAsync("http://www.justprice.pk/" + link);

                var imageCells = document.QuerySelectorAll(".owl-lazy");
                string imageLink = imageCells.Select(m => m.GetAttribute("src")).FirstOrDefault();

                var namecells = document.QuerySelectorAll("#product-name");
                var name = namecells.Select(x => x.TextContent).FirstOrDefault();

                var storeNameCells = document.QuerySelectorAll(".pb-product-lprice-list-item a img");
                var storeImage = storeNameCells.Select(x => x.GetAttribute("src")).ToArray();

                var storePriceCells = document.QuerySelectorAll(".pb-product-lprice-list-item .c-price.s-price");
                var storePrices = storePriceCells.Select(x => x.TextContent).ToArray();

                var itemSpecCells = document.QuerySelectorAll(".c-spec");
                var storespecs = itemSpecCells.Select(x => x.InnerHtml).FirstOrDefault();

                var itemMinPriceCells = document.QuerySelectorAll(".pb-product-price span");
                var MinPrice = itemMinPriceCells.Select(x => x.InnerHtml).ToArray();
                var price = MinPrice[2];

                var warrantyCells = document.QuerySelectorAll(".pb-product-lprice-list-item .medium-3.columns:nth-child(1)");
                var warranty = warrantyCells.Select(x => x.InnerHtml).ToArray();

                MobileStoreInfo[] store = new MobileStoreInfo[storePrices.Count()];
                for (int i = 0; i < store.Count(); i++)
                {
                    store[i] = new MobileStoreInfo();
                }
                int index = 0;
                int index2 = 1;
                foreach (var st in store)
                {
                    st.price = storePrices[index];
                    st.storeImage = storeImage[index];
                    st.warranty = warranty[index2].Split(new[] { "<br>" }, StringSplitOptions.None)[0].Trim();
                    st.inStock = warranty[index2].Split(new[] { "<br>" }, StringSplitOptions.None)[2].Trim();
                    index++;
                    index2 = index2 + 2;
                }

                mobile.imageUrl = imageLink;
                mobile.storeInfo = store;
                mobile.price = price;
                mobile.spec = storespecs;
                mobile.name = name;
                //var InstockCells = document.QuerySelectorAll(".pb-product-lprice-list-item .c-price.s-price");
                //var instock = InstockCells.Select(x => x.TextContent);

                //var conditioncells = document.QuerySelectorAll(".pb-product-lprice-list-item .c-price.s-price");
                //var condition = conditioncells.Select(x => x.TextContent);

                //var deliveryCells = document.QuerySelectorAll(".pb-product-lprice-list-item .c-price.s-price");
                //var delivery = deliveryCells.Select(x => x.TextContent);

                //var phone1cells = document.QuerySelectorAll(phone1Selector);
                //string[] titles = phone1cells.Select(m => m.GetAttribute("title")).ToArray();
                return PartialView("../MobilesTablets/_NewMobileData", mobile);
                //return Json(mob, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return PartialView("../MobilesTablets/_NewMobileData", mobile);
            }
        }

        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                Ad ad = new Ad();
                return View(ad);
            }
            return RedirectToAction("Register", "Account");
        }
        public ActionResult CreateMobileAccessoriesAd()
        {
            if (Request.IsAuthenticated)
            {
                Ad ad = new Ad();
                return View(ad);
            }
            return RedirectToAction("Register", "Account");
        }
        public ActionResult EditMobileAccessoriesAd(int id)
        {
            if (Request.IsAuthenticated)
            {
                Ad ad = db.Ads.Find(id);
                if(ad.postedBy == User.Identity.GetUserId())
                {
                    return View(ad);
                }
                
            }
            return RedirectToAction("Register", "Account");
        }
        public IdStatus SaveMobileBrandModel()
        {
            string adStatus = "a";
            var company = System.Web.HttpContext.Current.Request["brand"];
            var model = System.Web.HttpContext.Current.Request["model"];
            if (company != null && company != "")
            {
                company = company.Trim();
                model = model.Trim();
            }
            if (true) //company != null
            {
                bool isOldBrand = db.Mobiles.Any(x => x.brand.Equals(company));
                
                if (!isOldBrand)
                {
                    if(company != null && company != "" && company != "undefined")
                    {
                        adStatus = "p";
                        Mobile mob = new Mobile();
                        mob.brand = company;
                        mob.addedBy = System.Web.HttpContext.Current.User.Identity.GetUserId();
                        mob.time = DateTime.UtcNow;
                        mob.status = "p";
                        db.Mobiles.Add(mob);
                        db.SaveChanges();
                        if(model != null && model != "" && model != "undefined")
                        {
                            MobileModel mod = new MobileModel();
                            mod.model = model;
                            mod.brandId = mob.Id;
                            mod.time = DateTime.UtcNow;
                            mod.status = "p";
                            mod.addedBy = System.Web.HttpContext.Current.User.Identity.GetUserId();
                            db.MobileModels.Add(mod);
                            db.SaveChanges();
                        }
                    } 
                }
                else
                {
                    bool isOldModel = db.MobileModels.Any(x => x.model.Equals(model));
                    if (!isOldModel)
                    {
                        if(model != "" && model != null && model != "undefined")
                        {
                            adStatus = "p";
                            var brandId = db.Mobiles.FirstOrDefault(x => x.brand.Equals(company));
                            MobileModel mod = new MobileModel();
                            mod.brandId = brandId.Id;
                            mod.model = model;
                            mod.status = "p";
                            mod.addedBy = System.Web.HttpContext.Current.User.Identity.GetUserId();
                            mod.time = DateTime.UtcNow;
                            db.MobileModels.Add(mod);
                            db.SaveChanges();
                        }
                    }
                }
                var mobileModel = db.MobileModels.FirstOrDefault(x => x.Mobile.brand == company && x.model == model);
                IdStatus idstatus = new IdStatus();
                idstatus.id = mobileModel.Id;
                idstatus.status = adStatus;
                return idstatus; 
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,category,subcategory,postedBy,title,description,time")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAuthenticated)
                {
                    //string tempId = Request["tempId"];
                    FileName[] fileNames = JsonConvert.DeserializeObject<FileName[]>(Request["files"]);
                    MobileAd mobileAd = new MobileAd();
                    mobileAd.sims = Request["sims"];
                    mobileAd.color = Request["color"];
                    IdStatus idstatus = SaveMobileBrandModel();
                    mobileAd.mobileId = idstatus.id;
                    ad.status = idstatus.status;
                    ad = electronicController.MyAd(ad, "Save", "Mobiles");
                    
                    electronicController.PostAdByCompanyPage(ad.Id);

                    
                    //images
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        string sbs = e.ToString();
                    }
                    //tags
                    electronicController.SaveTags(Request["tags"], ad);
                    // FileUploadHandler(ad);
                    mobileAd.Id = ad.Id;
                    db.MobileAds.Add(mobileAd);
                    //ad.MobileAd.a(mobileAd);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        string sbs = e.ToString();
                    }
                    // ReplaceAdImages(ad.Id,tempId,fileNames);
                   electronicController.ReplaceAdImages( ad, fileNames);
                    //location
                   electronicController.MyAdLocation(Request["city"], Request["popularPlace"], Request["exectLocation"], ad, "Save");
                    return RedirectToAction("Details", "Electronics", new { id = ad.Id, title = ElectronicsController.URLFriendly(ad.title) });
                }
                return RedirectToAction("Register", "Account");
            }
            return View("Create", ad);
            //ViewBag.postedBy = new SelectList(db.AspNetUsers, "Id", "Email", ad.postedBy);
            //return View(ad);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMobileAccessoriesAd([Bind(Include = "Id,category,postedBy,title,description,time")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAuthenticated)
                {
                    FileName[] fileNames = JsonConvert.DeserializeObject<FileName[]>(Request["files"]);
                    MobileAd mobileAd = new MobileAd();
                    mobileAd.color = Request["color"];
                    IdStatus idstatus = SaveMobileBrandModel();
                    mobileAd.mobileId = idstatus.id;
                    ad.status = idstatus.status;
                    ad = electronicController.MyAd(ad, "Save", "MobileAccessories");
                    
                    
                    electronicController.PostAdByCompanyPage(ad.Id);
                    //tags
                    electronicController.SaveTags(Request["tags"], ad);

                    mobileAd.Id = ad.Id;
                    db.MobileAds.Add(mobileAd);
                    db.SaveChanges();
                   electronicController.ReplaceAdImages( ad, fileNames);
                    //location
                   electronicController.MyAdLocation(Request["city"], Request["popularPlace"], Request["exectLocation"], ad, "Save");
                    return RedirectToAction("Details", "Electronics", new { id = ad.Id, title = ElectronicsController.URLFriendly(ad.title) });
                }
                return RedirectToAction("Register", "Account");
            }
            return View("Create", ad);
        }
        public ActionResult Update([Bind(Include = "Id,category,subcategory,postedBy,title,description,time")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAuthenticated)
                {
                    var ab = Request["postedBy"];
                    var iddd = User.Identity.GetUserId();
                    if (Request["postedBy"] == User.Identity.GetUserId())
                    {
                        FileName[] fileNames = JsonConvert.DeserializeObject<FileName[]>(Request["files"]);
                        MobileAd mobileAd = new MobileAd();

                        mobileAd.sims = Request["sims"];
                        mobileAd.color = Request["color"];
                        IdStatus idstatus = SaveMobileBrandModel();
                        mobileAd.mobileId = idstatus.id;
                        ad.status = idstatus.status;
                        ad = electronicController.MyAd(ad, "Update");

                        

                        //tags
                        electronicController.SaveTags(Request["tags"],  ad, "update");
                        string brand = Request["brand"];
                        string model = Request["model"];
                        var mobileModel = db.MobileModels.FirstOrDefault(x => x.Mobile.brand == brand  && x.model == model );
                        mobileAd.mobileId = mobileModel.Id;
                       
                        electronicController.PostAdByCompanyPage(ad.Id,true);
                        mobileAd.Id = ad.Id;
                        //check if mobilead is not saved.
                        var mobdata = db.MobileAds.Any(x => x.Ad.Id.Equals(ad.Id));
                        if (mobdata)
                        {
                            db.Entry(mobileAd).State = EntityState.Modified;
                        }
                        else
                        {
                            db.MobileAds.Add(mobileAd);
                        }
                        //ad.MobileAds.Add(mobileAd);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            string sss = e.ToString();
                        }
                        //location
                        electronicController.MyAdLocation(Request["city"], Request["popularPlace"], Request["exectLocation"],  ad, "Update");
                        electronicController.ReplaceAdImages( ad, fileNames);
                        return RedirectToAction("Details", "Electronics", new { id = ad.Id, title = ElectronicsController.URLFriendly(ad.title) });
                    }
                }
                return RedirectToAction("Register", "Account");
            }
            return View("Edit", ad);
        }
        public ActionResult EditAd(int id)
        {
            if (Request.IsAuthenticated)
            {
                Ad ad = db.Ads.Find(id);
                if(ad.postedBy == User.Identity.GetUserId())
                {
                    return View(ad);
                }
            }
            return RedirectToAction("Register", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMobileAccessoriesAd([Bind(Include = "Id,category,postedBy,title,description,time")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAuthenticated)
                {
                    if (Request["postedBy"] == User.Identity.GetUserId())
                    {
                        FileName[] fileNames = JsonConvert.DeserializeObject<FileName[]>(Request["files"]);
                        MobileAd mobileAd = new MobileAd();

                        mobileAd.sims = Request["sims"];
                        mobileAd.color = Request["color"];
                        IdStatus idstatus = SaveMobileBrandModel();
                        mobileAd.mobileId = idstatus.id;
                        ad.status = idstatus.status;
                        ad = electronicController.MyAd(ad, "Update");


                        //tags
                        electronicController.SaveTags(Request["tags"],  ad, "update");
                        //location
                        electronicController.PostAdByCompanyPage(ad.Id, true);
                       electronicController.MyAdLocation(Request["city"], Request["popularPlace"], Request["exectLocation"],  ad, "Update");
                        electronicController.ReplaceAdImages( ad, fileNames);

                        
                        
                        //db.Ads.Add(ad);
                        mobileAd.Id = ad.Id;
                        db.Entry(mobileAd).State = EntityState.Modified;

                        db.SaveChanges();
                        return RedirectToAction("Details", "Electronics", new { id = ad.Id, title = ElectronicsController.URLFriendly(ad.title) });
                    }

                }
                return RedirectToAction("Register", "Account");
            }
            return View("EditAd", ad);
        }
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
