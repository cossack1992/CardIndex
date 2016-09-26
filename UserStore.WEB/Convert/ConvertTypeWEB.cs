using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserStore.WEB.Models;
using UserStore.BLL.DTO;

namespace UserStore.WEB.Convert
{
    public static class ConvertTypeWEB
    {
        public static ContentDTO Convert(ContentModelInPut model, string imagePath, string path)
        {
            if (model != null)
            {
                ContentDTO newContent = new ContentDTO();
                newContent.Name = model.Name.ToLower();
                foreach(var i in model.Directors.Split(new char[] { ';', ',', '.', ':' })) newContent.Directors.Add( i.ToLower() );
                foreach (var i in model.Writers.Split(new char[] { ';',  ',', '.', ':' })) newContent.Writers.Add( i.ToLower() );
                foreach (var i in model.Genres.Split(new char[] { ';',  ',', '.', ':' })) newContent.Genres.Add(i.ToLower() );
                newContent.Id = model.Id;            
                if(imagePath != "") newContent.Images.Add(new ImageDTO { Name = model.Image.FileName.ToLower(), Path = imagePath });
                newContent.Path = path;
                newContent.Transletor =  model.Transletor.ToLower() ;
                newContent.Language =  model.Language.ToLower() ;
                newContent.Year = model.Year.ToString();
                return newContent;
            }
            else return null;
        }
        public static ContentModelOutPut Convert(ContentDTO model)
        {
            if (model != null)
            {
                ContentModelOutPut newContent = new ContentModelOutPut();
                newContent.Name = model.Name;
                foreach (var i in model.Directors)
                    newContent.Directors.Add(i);
                foreach (var i in model.Writers)
                    newContent.Writers.Add(i);
                foreach (var i in model.Genres)
                    newContent.Genres.Add(i);
                foreach (var i in model.Images)
                    newContent.Images.Add(new ImageModel { Name = i.Name, Path = i.Path });
                newContent.Id = model.Id;
                newContent.Path = model.Path;
                newContent.Type = model.Type;
                newContent.VoteUp = model.VoteUp;
                newContent.VoteDown = model.VoteDown;
                newContent.Transletor = model.Transletor;
                newContent.Language = model.Language;
                newContent.Check = model.Check;
                newContent.Year = model.Year;
                return newContent;
            }
            else return null;
        }
        public static ContentModelInPut ConvertIn(ContentDTO model)
        {
            if (model != null)
            {
                ContentModelInPut newContent = new ContentModelInPut();
                newContent.operation = "Update";
                newContent.Name = model.Name;
                foreach (var i in model.Directors) newContent.Directors += ";" + i;

                newContent.Directors = newContent.Directors.Remove(0, 1);
                foreach (var i in model.Writers) newContent.Writers += ";" + i;

                newContent.Writers = newContent.Writers.Remove(0, 1);
                foreach (var i in model.Genres) newContent.Genres += ";" + i;

                newContent.Genres = newContent.Genres.Remove(0, 1);

                newContent.Id = model.Id;
                                               
                newContent.Transletor = model.Transletor;
                newContent.Language = model.Language;
                newContent.Check = model.Check;
                
                int year;
                if (Int32.TryParse(model.Year, out year))

                    newContent.Year = year;
                else newContent.Year = 1930;
                return newContent;
            }
            else return null;
        }
        public static ContentDTO Convert(ContentModelOutPut model)
        {
            if (model != null)
            {
                ContentDTO newContent = new ContentDTO();
                newContent.Name = model.Name;
                foreach (var i in model.Directors)
                    newContent.Directors.Add(i);
                foreach (var i in model.Writers)
                    newContent.Writers.Add(i);
                foreach (var i in model.Genres)
                    newContent.Genres.Add(i);
                
                foreach (var i in model.Images)
                    if(i != null)
                    newContent.Images.Add(new ImageDTO { Name = i.Name, Path = i.Path });
                newContent.Id = model.Id;
                newContent.Path = model.Path;
                newContent.Type = model.Type;
                newContent.VoteUp = model.VoteUp;
                newContent.VoteDown = model.VoteDown;
                newContent.Transletor = model.Transletor;
                newContent.Language = model.Language;
                newContent.Check = model.Check;
                newContent.Year = model.Year;
                return newContent;
            }
            else return null;
        }
    }
}