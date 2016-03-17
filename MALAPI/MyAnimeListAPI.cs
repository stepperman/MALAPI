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
        /// <returns>a populated anime object, or null if failed.</returns>
        public async Task<Anime> GetAnime(string id) => await parser.GetAnime(id);

        /// <summary>
        /// Get's an anime from a MAL url. (http://myanimelist.com/anime/532543/coolanimename
        /// </summary>
        /// <param name="malurl">The url of the anime webpage</param>
        /// <returns>a populated anime object, or null if failed.</returns>
        public async Task<Anime> GetAnimeMalLink(string malurl) => await parser.GetAnimeMALURL(malurl);

        public async Task<Person> FindPerson(string name) => await parser.FindPerson(name);
    }
}
