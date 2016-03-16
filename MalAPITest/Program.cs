using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MALAPI;

namespace MalAPITest
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().Wait();
        }

        static async Task Run()
        {
            MyAnimeListAPI malap = new MyAnimeListAPI();
            Anime anime = await malap.GetAnime("31043");

            Console.WriteLine(anime.Title[0]);
            Console.WriteLine(anime.Synonyms[0]);
            Console.WriteLine(anime.EnglishTitle[0]);

            Console.ReadLine();
        }
    }
}
