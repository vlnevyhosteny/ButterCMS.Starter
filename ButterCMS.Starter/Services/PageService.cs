﻿using System;
using System.Threading.Tasks;
using ButterCMS.Starter.Models;
using Newtonsoft.Json.Linq;

namespace ButterCMS.Starter.Services
{
    public class PageService
    {
        private readonly ButterCMSClient client;

        private readonly string landingPageType = "landing-page";

        public PageService(ButterCMSClientService clientService)
        {
            this.client = clientService.GetClient();
        }

        public async Task<LandingPageViewModel> GetLandingPage(string pageSlug)
        {
            var response = await this.client.RetrievePageAsync<LandingPageJSONObject>(this.landingPageType, pageSlug);

            var result = new LandingPageViewModel()
            {
                SEOViewModel = new SEOViewModel()
                {
                    Title = response.Data.Fields.seo.title,
                    Description = response.Data.Fields.seo.description,
                },
            };

            foreach (var item in response.Data.Fields.body)
            {
                string scrollAnchorId = this.GetStringFieldFromJson(item, "scroll_anchor_id");

                string headline = this.GetStringFieldFromJson(item, "headline");

                switch (scrollAnchorId)
                {
                    case "home":
                        result.HeroSection = new HeroSectionViewModel()
                        {
                            ScrollAnchorId = scrollAnchorId,
                            Headline = headline,
                            ButtonLabel = this.GetStringFieldFromJson(item, "button_label"),
                            ButtonUrl = this.GetStringFieldFromJson(item, "button_url"),
                            Image = this.GetStringFieldFromJson(item, "image"),
                            SubHeadline = this.GetStringFieldFromJson(item, "subheadline"),
                        };
                        break;

                    case "about":
                        result.AboutSection = new AboutSectionViewModel()
                        {
                            ScrollAnchorId = scrollAnchorId,
                            Headline = headline,
                            ButtonLabel = this.GetStringFieldFromJson(item, "button_label"),
                            ButtonUrl = this.GetStringFieldFromJson(item, "button_url"),
                            Image = this.GetStringFieldFromJson(item, "image"),
                            SubHeadline = this.GetStringFieldFromJson(item, "subheadline"),
                            ImagePosition = this.GetStringFieldFromJson(item, "image_position")
                        };
                        break;

                    //case "tryit":
                    //    result.AboutSection = new AboutSectionViewModel()
                    //    {
                    //        ScrollAnchorId = scrollAnchorId,
                    //        Headline = headline,
                    //        ButtonLabel = this.GetStringFieldFromJson(item, "button_label"),
                    //        ButtonUrl = this.GetStringFieldFromJson(item, "button_url"),
                    //        Image = this.GetStringFieldFromJson(item, "image"),
                    //        SubHeadline = this.GetStringFieldFromJson(item, "subheadline"),
                    //        ImagePosition = this.GetStringFieldFromJson(item, "image_position")
                    //    };
                    //    break;
                }
            }

            return result;
        }

        private string GetStringFieldFromJson(JObject json, string property) => (string)json["fields"][property];
    }
}
