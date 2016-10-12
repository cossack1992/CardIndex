using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStore.BLL.DTO;
using System.Collections.Generic;
using UserStore.DAL.Entities;
using UserStore.WEB.Convert;
using UserStore.BLL.Convert;
using UserStore.WEB.Models;

namespace UserStore.UT
{
    [TestClass]
    public class UnitTest
    {
        Genre genreComedy ;
        Language languageEnglish ;
        Translator transletor ;
        Director director ;
        Scenarist scenarist ;
        Check check ;
        Image image1 ;
        AbstractContent content;
        ContentDTO contentDTO;
        
        ContentModelOutPut contentWEBOUT;
       [TestInitialize]
        public void TestInitialize()
        {
             genreComedy = new Genre { Id = 1, Name = "comedy" };
             languageEnglish = new Language { Id = 1, Name = "english" };
             transletor = new Translator { Id = 1, Name = "transletor" };
             director = new Director { Id = 1, Name = "director" };
             scenarist = new Scenarist { Id = 1, Name = "scenarist" };
             check = new Check { Id = 1, Name = "checked" };
             image1 = new Image { Id = 1, Name = "image1", Path = @"e:\imagepath1" };
             content = new Video {
                 Id = 1,
                 Name = "content1",
                 Path = @"e:\contentpath",
                 Year = 2016.ToString(),
                 VoteUp = 2,
                 VoteDown = 1,
                 Check = check,
                 Directors = new List<Director>() { director},
                 Genres = new List<Genre>() { genreComedy },
                 Images = new List<Image>() { image1},
                 Language = languageEnglish,
                 Transletor = transletor,
                 Writers = new List<Scenarist>() { scenarist}
             };
            contentDTO = new ContentDTO {
                Id = 1,
                Name = "content1",
                Path = @"e:\contentpath",
                Year = 2016.ToString(),
                VoteUp = 2,
                VoteDown = 1,
                Check = "checked",
                Directors = new List<string>() { "director" },
                Genres = new List<string>() { "comedy" },
                Images = new List<ImageDTO>() { new ImageDTO { Id = 1, Name = "image1", Path = @"e:\imagepath1" } },
                Language = "english",
                Translator = "transletor",
                Writers = new List<string>() { "scenarist" },
                Type = "Video" };            
            contentWEBOUT = new ContentModelOutPut {
                Id = 1,
                Name = "content1",
                Path = @"e:\contentpath",
                Year = 2016.ToString(),
                VoteUp = 2,
                VoteDown = 1,
                Check = "checked",
                Directors = new List<string>() { "director" },
                Genres = new List<string>() { "comedy" },
                Images = new List<ImageModel>() { new ImageModel { Name = "image1", Path = @"e:\imagepath1" } },
                Language = "english",
                Translator = "transletor",
                Writers = new List<string>() { "scenarist" },
                Type = "Video" };
        }
        [TestMethod]
        public void ContentDAL_ConvertTo_ContentDTO()
        {

            Assert.IsTrue(CompareDTO(contentDTO, ConvertTypeDTO.Convert(content)));
        }
        [TestMethod]
        public void ContentDTO_ConvertTo_ContentWEBOUT()
        {

            Assert.IsTrue(CompareWEB(contentWEBOUT, ConvertTypeWEB.Convert(contentDTO)));
        }        
        private bool CompareDTO(ContentDTO conten1, ContentDTO content2)
        {
            if (conten1.Id != content2.Id) return false;
            if (conten1.Name != content2.Name) return false;
            if (conten1.Path != content2.Path) return false;
            if (conten1.VoteUp != content2.VoteUp) return false;
            if (conten1.VoteDown != content2.VoteDown) return false;
            if (conten1.Year != content2.Year) return false;
            if (conten1.Type != content2.Type) return false;
            if (conten1.Translator != content2.Translator) return false;
            if (conten1.Language != content2.Language) return false;
            foreach (var li in conten1.Genres) if (!content2.Genres.Contains(li)) return false;           
            foreach (var li in conten1.Directors) if (!content2.Directors.Contains(li)) return false;           
            foreach (var li in conten1.Writers) if (!content2.Writers.Contains(li)) return false;
            return true;
        }
        private bool CompareWEB(ContentModelOutPut conten1, ContentModelOutPut content2)
        {
            if (conten1.Id != content2.Id) return false;
            if (conten1.Name != content2.Name) return false;
            if (conten1.Path != content2.Path) return false;
            if (conten1.VoteUp != content2.VoteUp) return false;
            if (conten1.VoteDown != content2.VoteDown) return false;
            if (conten1.Year != content2.Year) return false;
            if (conten1.Type != content2.Type) return false;
            if (conten1.Translator != content2.Translator) return false;
            if (conten1.Language != content2.Language) return false;
            foreach (var li in conten1.Genres) if (!content2.Genres.Contains(li)) return false;
            foreach (var li in conten1.Directors) if (!content2.Directors.Contains(li)) return false;
            foreach (var li in conten1.Writers) if (!content2.Writers.Contains(li)) return false;
            return true;
        }
    }
}
