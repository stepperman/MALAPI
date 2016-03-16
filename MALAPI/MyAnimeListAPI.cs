using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MALAPI
{
    public class MyAnimeListAPI
    {
        Parser parser = new Parser();

        /// <summary>
        /// Gets an anime
        /// </summary>
        /// <param name="id">ID of the anime</param>
        /// <returns>an anime object</returns>
        public async Task<Anime> GetAnime(string id) => await parser.GetAnime(id);
        
        //public async Task<Anime> GetAnimeFromMalLink(string malLink)
    }
}
