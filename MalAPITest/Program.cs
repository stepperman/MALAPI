﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MALAPI;
using System.Diagnostics;

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

            var person = await malap.GetAnimeMalLink("http://myanimelist.net/anime/21/One_Piece"); 

            Console.ReadLine();
        }
    }
}
