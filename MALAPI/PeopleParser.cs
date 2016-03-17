using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.IO;

namespace MALAPI
{
    public sealed partial class Parser
    {
        public async Task<Person> ParsePerson(Stream html, string personurl)
        {
            Person person = new Person();

            //Load document
            HtmlDocument doc = new HtmlDocument();
            doc.Load(html);

            //Get person name
            var x = doc.DocumentNode.SelectSingleNode(@"//h1[@class=""h1""]");
            person.Name = x.InnerText;

            //Get the more thing
            x = doc.DocumentNode.SelectSingleNode(@"//div[@class=""people-informantion-more js-people-informantion-more""]");
            person.Information = x.InnerText;

            //Get the beautiful picture of the probably Japanese anime team person
            x = doc.DocumentNode.SelectSingleNode($@"//img[@alt=""{person.Name}""]");
            if(x != null)
                person.PersonImage = x.Attributes["src"].Value;

            person.MalLink = personurl;
            await Task.Delay(0);
            return person;
        }

        public async Task<Person> FindPerson(string name)
        {
            string malurl = $"http://myanimelist.net/people.php?q={Uri.EscapeDataString(name)}";
            var wr = WebRequest.Create(malurl);
            var response = wr.GetResponse();
            Stream str = wr.GetResponse().GetResponseStream();

            if (response.ResponseUri.LocalPath != "/people.php")
            {
                string personurl = $"http://myanimelist.net/{response.ResponseUri.LocalPath}";
                wr = WebRequest.Create(personurl);
                str = wr.GetResponse().GetResponseStream();
                return await ParsePerson(str, personurl);
            }
            
            HtmlDocument doc = new HtmlDocument();
            doc.Load(str);

            var extracted = doc.DocumentNode.SelectSingleNode("//td[text()=\"No results returned\"]");
            if (extracted != null)
                return null;

            extracted = doc.DocumentNode.SelectSingleNode("//tr/td/a");

            string retValue = $"http://myanimelist.net/{extracted.Attributes["href"].Value}";
            Stream personPage = WebRequest.Create(retValue).GetResponse().GetResponseStream();

            return await ParsePerson(personPage, retValue);
        }

    }
}
