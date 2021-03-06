﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MALAPI
{
    public sealed partial class Parser
    {
        /// <summary>
        /// Parse HTML link and crawl it.
        /// </summary>
        /// <param name="html"></param>
        /// <returns>an anime object populated with the details of that page.</returns>
        private async Task<Anime> AnimeParser(string html)
        {
            Anime anime = new Anime();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            //Get left column (stuff including the title, and stuff)
            var leftColumn = htmlDoc.DocumentNode.SelectSingleNode("//div[@id=\"content\"]/table/tr/td[@class=\"borderClass\"]");

            //Get and set title. (One on the top of the page.)
            var extracted = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class=\"h1\"]/span[@itemprop=\"name\"]");
            var title = extracted.InnerText;
            anime.Title = title;

            //Get English Title.
            extracted = leftColumn.SelectSingleNode("//span[text()=\"English:\"]");
            if (extracted != null)
            {
                extracted = extracted.NextSibling;
                var englishTitle = extracted.InnerText.Replace("\n", "").Split(',');
                anime.EnglishTitle = englishTitle;
                for (int i = 0; i < englishTitle.Length; i++)
                {
                    englishTitle[i] = englishTitle[i].Trim();
                }
            }

            //Get Synonyms
            extracted = leftColumn.SelectSingleNode("//span[text()=\"Synonyms:\"]");
            if (extracted != null)
            {
                extracted = extracted.NextSibling;
                var synonyms = extracted.InnerText.Replace("\n", "").Split(',');
                for (int i = 0; i < synonyms.Length; i++)
                {
                    synonyms[i] = synonyms[i].Trim();
                }
                anime.Synonyms = synonyms;
            }

            //Get synopsis
            extracted = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop=\"description\"]");
            anime.Synopsis = extracted.InnerText;

            //Get Poster
            extracted = leftColumn.SelectSingleNode("//td/div/div/a/img");
            anime.PosterLink = extracted.Attributes["src"].Value;

            //Get Score
            extracted = leftColumn.SelectSingleNode("//span[@itemprop=\"ratingValue\"]");
            anime.Score = double.Parse(extracted.InnerText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);

            //Get Studios
            extracted = leftColumn.SelectSingleNode("//span[text()=\"Studios:\"]/..");
            var studioNodes = extracted.SelectNodes("./a");
            var studios = new List<string>();

            for (int i = 0; i < studioNodes.Count(); i++)
            {
                studios.Add(studioNodes[i].InnerText);
            }
            anime.Studios = studios.ToArray();
            studios = null;
            studioNodes = null;

            //Get Source
            extracted = leftColumn.SelectSingleNode("//span[text()=\"Source:\"]").NextSibling;
            anime.Source = extracted.InnerText;

            //Get Genres
            extracted = leftColumn.SelectSingleNode("//span[text()=\"Genres:\"]/..");
            var genreNodes = extracted.SelectNodes("./a");
            List<string> genres = new List<string>();

            for (int i = 0; i < genreNodes.Count(); i++)
            {
                genres.Add(genreNodes[i].InnerText);
            }

            anime.Genres = genres.ToArray();

            //Get episodes
            extracted = leftColumn.SelectSingleNode("//span[text()=\"Episodes:\"]").NextSibling;
            ushort.TryParse(extracted.InnerText.Trim(), out anime.Episodes);

            //Get Aired Time
            extracted = leftColumn.SelectSingleNode("//span[text()=\"Aired:\"]").NextSibling;
            anime.Aired = extracted.InnerText.Trim();

            //Get Type
            extracted = leftColumn.SelectSingleNode("//span[text()=\"Type:\"]/../a");
            anime.Type = extracted.InnerText.Trim();
            
            await Task.Delay(0);
            return anime;
        }
        
        /// <summary>
        /// Gets an anime
        /// </summary>
        /// <param name="id">ID of the anime</param>
        /// <returns>an anime object</returns>
        public async Task<Anime> GetAnime(string id)
        {
            return await AnimeParser(NetHelper.Get($"http://myanimelist.net/anime/{id}.php"));
        }

        public async Task<Anime> GetAnimeMALURL(string url)
        {
            url = Regex.Match(url, "myanimelist.net/anime/[0-9]{1,9}").ToString();
            return await AnimeParser(NetHelper.Get($"http://{url}"));
        }
    }
}
