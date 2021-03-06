﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Inspinia_MVC5_SeedProject.Models;
using Microsoft.AspNet.Identity;
using System.Net.Mail;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class SearchController : ApiController
    {
        private Entities db = new Entities();

       


        public async Task<IHttpActionResult> GetTrendingAds()
        {
            var temp1 = from ad in db.Ads
                        where ad.AdImages.Count >0
                        orderby ad.time descending
                        select new
                        {
                            title = ad.title,
                            postedById = ad.AspNetUser.Id,
                            postedByName = ad.AspNetUser.Email,
                            description = ad.description,
                            id = ad.Id,
                            time = ad.time,
                            isNegotiable = ad.isnegotiable,
                            price = ad.price,
                            reportedCount = ad.Reporteds.Count,
                            category = ad.category,
                            subcategory = ad.subcategory,
                            views = ad.views,
                            condition = ad.condition,
                            savedCount = ad.SaveAds.Count,
                            adTags = from tag1 in ad.AdTags.ToList()
                                     select new
                                     {
                                         id = tag1.tagId,
                                         name = tag1.Tag.name,
                                         //followers = tag.Tag.FollowTags.Count(x => x.tagId.Equals(tag.Id)),
                                         //info = tag.Tag.info,
                                     },
                            bid = from biding in ad.Bids.ToList()
                                  select new
                                  {
                                      price = biding.price,
                                  },
                            adImages = (from image in ad.AdImages
                                       select new
                                       {
                                           imageExtension = image.imageExtension,
                                       }).FirstOrDefault(),
                            location = new
                            {
                                cityName = ad.AdsLocation.City.cityName,
                                cityId = ad.AdsLocation.cityId,
                                popularPlaceId = ad.AdsLocation.popularPlaceId,
                                popularPlace = ad.AdsLocation.popularPlace.name,
                                exectLocation = ad.AdsLocation.exectLocation,
                            },

                        };
            return Ok(temp1.Take(6));
        }
        public async Task<IHttpActionResult> SearchAds(string tags, string title, int minPrice, int maxPrice, string city, string pp,string category, string subcategory)
        {
            if (subcategory.Equals("Books "))
            {
                subcategory = "Books & Study Material";
            }
            else if (subcategory == "Gym ")
            {
                subcategory = "Gym & Fitness";
            }
            string islogin = "";
            if (User.Identity.IsAuthenticated)
            {
                islogin = User.Identity.GetUserId();
            }
            if (tags == null)
            {
                var temp1 = from ad in db.Ads
                            where (ad.category.Equals(category) && (subcategory == null || subcategory == "undefined" || ad.subcategory.Equals(subcategory) )  && ad.status.Equals("a")  && (minPrice == 0 || ad.price > minPrice) && (maxPrice == 50000 || ad.price < maxPrice) && (city == null || city == "undefined" || ad.AdsLocation.City.cityName.Equals(city) && (pp == null || pp == "undefined" || ad.AdsLocation.popularPlace.name.Equals(pp))))
                            orderby ad.time descending
                            select new
                            {
                                title = ad.title,
                                postedById = ad.AspNetUser.Id,
                                postedByName = ad.AspNetUser.Email,
                                description = ad.description,
                                id = ad.Id,
                                time = ad.time,
                                islogin = islogin,
                                isNegotiable = ad.isnegotiable,
                                price = ad.price,
                                reportedCount = ad.Reporteds.Count,
                                isReported = ad.Reporteds.Any(x => x.reportedBy == islogin),
                                category = ad.category,
                                subcategory = ad.subcategory,
                                views = ad.views,
                                condition = ad.condition,
                                savedCount = ad.SaveAds.Count,
                                adTags = from tag1 in ad.AdTags.ToList()
                                         select new
                                         {
                                             id = tag1.tagId,
                                             name = tag1.Tag.name,
                                             //followers = tag.Tag.FollowTags.Count(x => x.tagId.Equals(tag.Id)),
                                             //info = tag.Tag.info,
                                         },
                                bid = from biding in ad.Bids.ToList()
                                      select new
                                      {
                                          price = biding.price,
                                      },
                                adImages = from image in ad.AdImages.ToList()
                                           select new
                                           {
                                               imageExtension = image.imageExtension,
                                           },
                                location = new
                                {
                                    cityName = ad.AdsLocation.City.cityName,
                                    cityId = ad.AdsLocation.cityId,
                                    popularPlaceId = ad.AdsLocation.popularPlaceId,
                                    popularPlace = ad.AdsLocation.popularPlace.name,
                                    exectLocation = ad.AdsLocation.exectLocation,
                                },

                            };
                return Ok(temp1);
            }
            string[] tagsArray = null;
            if (tags != null)
            {
                tagsArray = tags.Split(',');
            }



            var temp = from ad in db.Ads
                       where (ad.category.Equals(category) && (subcategory == null || subcategory == "undefined" || ad.subcategory.Equals(subcategory)) && (!tagsArray.Except(ad.AdTags.Select(x => x.Tag.name)).Any()) && ad.status.Equals("a") && (minPrice == 0 || ad.price > minPrice) && (maxPrice == 50000 || ad.price < maxPrice) && (city == null || city == "undefined" || ad.AdsLocation.City.cityName.Equals(city) && (pp == null || pp == "undefined" || ad.AdsLocation.popularPlace.name.Equals(pp))))
                        
                       orderby ad.time descending
                        select new
                        {
                            title = ad.title,
                            postedById = ad.AspNetUser.Id,
                            postedByName = ad.AspNetUser.Email,
                            description = ad.description,
                            id = ad.Id,
                            time = ad.time,
                            islogin = islogin,
                            isNegotiable = ad.isnegotiable,
                            price = ad.price,
                            reportedCount = ad.Reporteds.Count,
                            isReported = ad.Reporteds.Any(x => x.reportedBy == islogin),
                            category = ad.category,
                            subcategory = ad.subcategory,
                            views = ad.views,
                            condition = ad.condition,
                            savedCount = ad.SaveAds.Count,
                            adTags = from tag1 in ad.AdTags.ToList()
                                     select new
                                     {
                                         id = tag1.tagId,
                                         name = tag1.Tag.name,
                                         //followers = tag.Tag.FollowTags.Count(x => x.tagId.Equals(tag.Id)),
                                         //info = tag.Tag.info,
                                     },
                            bid = from biding in ad.Bids.ToList()
                                  select new
                                  {
                                      price = biding.price,
                                  },
                            adImages = from image in ad.AdImages.ToList()
                                       select new
                                       {
                                           imageExtension = image.imageExtension,
                                       },
                            location = new
                            {
                                cityName = ad.AdsLocation.City.cityName,
                                cityId = ad.AdsLocation.cityId,
                                popularPlaceId = ad.AdsLocation.popularPlaceId,
                                popularPlace = ad.AdsLocation.popularPlace.name,
                                exectLocation = ad.AdsLocation.exectLocation,
                            },

                        };
            return Ok(temp);
        }
        public static void sendEmail(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new System.Net.Mail.MailAddress("dealkar.pk@gmail.com");
            // mail.From = new System.Net.Mail.MailAddress("notification@dealkar.pk");
            // The important part -- configuring the SMTP client
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;   // [1] You can try with 465 also, I always used 587 and got success 587
            smtp.EnableSsl = true; //commented for godaddy
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // [2] Added this
            smtp.UseDefaultCredentials = false; // [3] Changed this
            smtp.Credentials = new NetworkCredential("dealkar.pk@gmail.com", "birthday2Wishirfan");  // [4] Added this. Note, first parameter is NOT string.
                                                                                                     // smtp.Credentials = new NetworkCredential("notification@dealkar.pk", "birthdaywish");
            smtp.Host = "smtp.gmail.com";
            // smtp.Host = "relay-hosting.secureserver.net";
            //recipient address
            mail.To.Add(new MailAddress(to));
            mail.Subject = subject;
            //Formatted mail body
            mail.IsBodyHtml = true;
            // string st = "Test";

            mail.Body = body;
            smtp.Send(mail);
            //try
            //{
            //    smtp.Send(mail);
            //}
            //catch (Exception e)
            //{
            //    string s = e.ToString(); 
            //}
        }
        [HttpGet]
        public async Task<IHttpActionResult> SimilarData(int id, string tags, string category, string subcategory, string subsubcategory,string city)
        {
            
            string pp = null;
            if (subcategory == "Books ")
            {
                subcategory = "Books & Study Material";
            }
            else if (subcategory == "Others in Education ") {
                subcategory = "Others in Education & Learning";
            }
            else if (subcategory == "Gym ")
            {
                subcategory = "Gym & Fitness";
            }
            string islogin = "";
            if (User.Identity.IsAuthenticated)
            {
                islogin = User.Identity.GetUserId();
            }
            if (tags == null || tags == "null" || tags == "undefined")
            {
                var temp1 = from ad in db.Ads //db.ad_Search(null)
                            where ( ad.Id != id && ad.category.Equals(category) && (subcategory == null || subcategory == "null" || subcategory == "undefined" || ad.subcategory.Equals(subcategory)) && ad.status.Equals("a") && (city == null || city == "All Pakistan" || city == "undefined" || ad.AdsLocation.City.cityName.Equals(city) && (pp == null || pp == "undefined" || ad.AdsLocation.popularPlace.name.Equals(pp))))
                            orderby ad.time descending
                            select new
                            {
                                title = ad.title,
                                postedById = ad.AspNetUser.Id,
                                postedByName = ad.AspNetUser.Email,
                                description = ad.description,
                                id = ad.Id,
                                time = ad.time,
                                islogin = islogin,
                                isNegotiable = ad.isnegotiable,
                                price = ad.price,
                                reportedCount = ad.Reporteds.Count,
                                isReported = ad.Reporteds.Any(x => x.reportedBy == islogin),
                                category = ad.category,
                                subcategory = ad.subcategory,
                                views = ad.views,
                                condition = ad.condition,
                                savedCount = ad.SaveAds.Count,
                                adTags = from tag1 in ad.AdTags.ToList()
                                         select new
                                         {
                                             id = tag1.tagId,
                                             name = tag1.Tag.name,
                                             //followers = tag.Tag.FollowTags.Count(x => x.tagId.Equals(tag.Id)),
                                             //info = tag.Tag.info,
                                         },
                                bid = from biding in ad.Bids.ToList()
                                      select new
                                      {
                                          price = biding.price,
                                      },
                                adImages = (from image in ad.AdImages.ToList()
                                           select new
                                           {
                                               imageExtension = image.imageExtension,
                                           }).FirstOrDefault(),
                                location = new
                                {
                                    cityName = ad.AdsLocation.City.cityName,
                                    cityId = ad.AdsLocation.cityId,
                                    popularPlaceId = ad.AdsLocation.popularPlaceId,
                                    popularPlace = ad.AdsLocation.popularPlace.name,
                                    exectLocation = ad.AdsLocation.exectLocation,
                                },

                            };
                return Ok(temp1.Take(8));
            }
            string[] tagsArray = null;
            if (tags != null)
            {
                tagsArray = tags.Split(',');
            }



            var temp = from ad in db.Ads
                       where (ad.category.Equals(category) && (subcategory == null || subcategory == "undefined" || ad.subcategory.Equals(subcategory)) && (!tagsArray.Except(ad.AdTags.Select(x => x.Tag.name)).Any()) && ad.status.Equals("a") && (city == null || city == "undefined" || ad.AdsLocation.City.cityName.Equals(city) && (pp == null || pp == "undefined" || ad.AdsLocation.popularPlace.name.Equals(pp))))

                       orderby ad.time descending
                       select new
                       {
                           title = ad.title,
                           postedById = ad.AspNetUser.Id,
                           postedByName = ad.AspNetUser.Email,
                           description = ad.description,
                           id = ad.Id,
                           time = ad.time,
                           islogin = islogin,
                           isNegotiable = ad.isnegotiable,
                           price = ad.price,
                           reportedCount = ad.Reporteds.Count,
                           isReported = ad.Reporteds.Any(x => x.reportedBy == islogin),
                           category = ad.category,
                           subcategory = ad.subcategory,
                           views = ad.views,
                           condition = ad.condition,
                           savedCount = ad.SaveAds.Count,
                           adTags = from tag1 in ad.AdTags.ToList()
                                    select new
                                    {
                                        id = tag1.tagId,
                                        name = tag1.Tag.name,
                                        //followers = tag.Tag.FollowTags.Count(x => x.tagId.Equals(tag.Id)),
                                        //info = tag.Tag.info,
                                    },
                           bid = from biding in ad.Bids.ToList()
                                 select new
                                 {
                                     price = biding.price,
                                 },
                           adImages = from image in ad.AdImages.ToList()
                                      select new
                                      {
                                          imageExtension = image.imageExtension,
                                      },
                           location = new
                           {
                               cityName = ad.AdsLocation.City.cityName,
                               cityId = ad.AdsLocation.cityId,
                               popularPlaceId = ad.AdsLocation.popularPlaceId,
                               popularPlace = ad.AdsLocation.popularPlace.name,
                               exectLocation = ad.AdsLocation.exectLocation,
                           },

                       };
            return Ok(temp.Take(8));
        }

        // GET api/Search/5
        [ResponseType(typeof(Ad))]
        public async Task<IHttpActionResult> GetAd(int id)
        {
            Ad ad = await db.Ads.FindAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            return Ok(ad);
        }

        // PUT api/Search/5
        public async Task<IHttpActionResult> PutAd(int id, Ad ad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ad.Id)
            {
                return BadRequest();
            }

            db.Entry(ad).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Search
        [ResponseType(typeof(Ad))]
        public async Task<IHttpActionResult> PostAd(Ad ad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ads.Add(ad);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ad.Id }, ad);
        }

        // DELETE api/Search/5
        [ResponseType(typeof(Ad))]
        public async Task<IHttpActionResult> DeleteAd(int id)
        {
            Ad ad = await db.Ads.FindAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            db.Ads.Remove(ad);
            await db.SaveChangesAsync();

            return Ok(ad);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AdExists(int id)
        {
            return db.Ads.Count(e => e.Id == id) > 0;
        }
    }
}