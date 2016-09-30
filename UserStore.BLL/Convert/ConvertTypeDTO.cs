using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.DTO;

namespace UserStore.BLL.Convert
{
    public static class ConvertTypeDTO
    {


        public static IList<ImageDTO> Convert(IList<Image> name)
        {
            if (name != null)
            {
                try
                {


                    List<ImageDTO> list = new List<ImageDTO>();
                    foreach (var li in name)
                        list.Add(new ImageDTO { Name = li.Name, Path = li.Path });
                    return list;
                }
                catch (Exception)
                {
                    throw new ConvertDTOException();
                }
            }
            else throw new ArgumentNullException();
        }
        public static ImageDTO Convert(Image name)
        {
            if (name != null) return new ImageDTO { Id = name.Id, Name = name.Name, Path = name.Path };
            else return null;
        }
        public static ContentDTO Convert(AbstractContent name)
        {
            if (name != null)
            {
                try
                {

                    ContentDTO content = new ContentDTO();
                    content.Id = name.Id;
                    content.Name = name.Name;
                    content.Path = name.Path;
                    content.Type = name.ToString().Split(new char[] { '.' }).Last().Split(new char[] { '_' }).First();
                    content.VoteUp = name.VoteUp;
                    content.VoteDown = name.VoteDown;
                    content.Language = name.Language != null ? name.Language.Name : "";
                    content.Transletor = name.Transletor != null ? name.Transletor.Name : "";
                    content.Year = name.Year;
                    content.Check = name.Check.Name;
                    content.Images = name.Images != null ? Convert(name.Images) : new List<ImageDTO>();
                    if (name.Writers != null) foreach (var li in name.Writers) content.Writers.Add(li.Name);
                    else content.Writers.Add("");
                    if (name.Directors != null) foreach (var li in name.Directors) content.Directors.Add(li.Name);
                    else content.Directors.Add("");
                    if (name.Genres != null) foreach (var li in name.Genres) content.Genres.Add(li.Name);
                    else content.Genres.Add("");
                    return content;
                }
                catch (Exception)
                {
                    throw new ConvertDTOException();
                }
            }
            else throw new ArgumentNullException(); ;
        }
        public static AbstractContent Convert(ContentDTO name)
        {
            if (name != null)
            {
                try
                {

                    AbstractContent content = new Video();
                    content.Id = name.Id;
                    content.Name = name.Name;
                    content.Path = name.Path;
                    content.VoteUp = name.VoteUp;
                    content.VoteDown = name.VoteDown;
                    content.Language = new Language { Name = name.Language };
                    content.Transletor = new Translator { Name = name.Transletor };
                    content.Year = name.Year;
                    content.Check = new Check { Name = name.Check };
                    content.Images = Convert(name.Images);
                    foreach (var li in name.Writers) content.Writers.Add(new Scenarist { Name = li });
                    foreach (var li in name.Directors) content.Directors.Add(new Director { Name = li });
                    foreach (var li in name.Genres) content.Genres.Add(new Genre { Name = li });
                    return content;
                }
                catch (Exception)
                {
                    throw new ConvertDTOException();
                }
            }
            else throw new ArgumentNullException();
        }

        public static IList<Image> Convert(IList<ImageDTO> name)
        {
            if (name != null)
            {
                try
                {

                    List<Image> list = new List<Image>();
                    foreach (var li in name)
                        list.Add(new Image { Name = li.Name, Path = li.Path });
                    return list;
                }
                catch (Exception)
                {
                    throw new ConvertDTOException();
                }
            }
            else throw new ArgumentNullException();
        }

    }
}
