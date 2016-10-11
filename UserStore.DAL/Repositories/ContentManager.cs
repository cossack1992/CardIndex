using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;
using UserStore.DAL.EF;
using System.Linq.Expressions;
using System.Data.Entity;
using System.IO;

namespace UserStore.DAL.Repositories
{

    class ContentManager : IContentManager
    {
        public ApplicationContext DataBase { get; set; }
        public ContentManager(ApplicationContext db)
        {
            DataBase = db;
        }
        public async Task<int> CreateContent(string name, string path, string year, IList<string> directors, IList<string> writers, IList<string> genres, IList<Image> images, string language, string transletor, string check)
        {
            string contentType = path != "" ? Path.GetExtension(path) : "";
            Check localCheck = await DataBase.Checks.Where(x => x.Name == check).FirstOrDefaultAsync();
            Translator localTransletor = await DataBase.Transletors.Where(x => x.Name == transletor).FirstOrDefaultAsync();
            Language localLanguage = await DataBase.Languages.Where(x => x.Name == language).FirstOrDefaultAsync();
            List<Genre> localGenres = await DataBase.Genres.Where(x => genres.Contains(x.Name)).ToListAsync();
            List<Director> localDirectors = await DataBase.Directors.Where(x => directors.Contains(x.Name)).ToListAsync();
            List<Scenarist> localWriters = await DataBase.Writers.Where(x => writers.Contains(x.Name)).ToListAsync();
            switch (contentType.ToUpper())
            {
                case ".PDF":
                    DataBase.Contents.Add(new Book
                    {
                        Name = name,
                        Path = path,
                        Check = localCheck,
                        Directors = localDirectors,
                        Genres = localGenres,
                        Images = images,
                        Writers = localWriters,
                        Year = year,
                        Language = localLanguage,
                        Transletor = localTransletor
                    });
                    break;
                case ".MP3":
                    DataBase.Contents.Add(new Audio
                    {
                        Name = name,
                        Path = path,
                        Check = localCheck,
                        Directors = localDirectors,
                        Genres = localGenres,
                        Images = images,
                        Writers = localWriters,
                        Year = year,
                        Language = localLanguage,
                        Transletor = localTransletor
                    });
                    break;
                case ".WEBM":
                    DataBase.Contents.Add(new Video
                    {
                        Name = name,
                        Path = path,
                        Check = localCheck,
                        Directors = localDirectors,
                        Genres = localGenres,
                        Images = images,
                        Writers = localWriters,
                        Year = year,
                        Language = localLanguage,
                        Transletor = localTransletor
                    });
                    break;
                case "":
                    DataBase.Contents.Add(new Empty
                    {
                        Name = name,
                        Path = path,
                        Check = localCheck,
                        Directors = localDirectors,
                        Genres = localGenres,
                        Images = images,
                        Writers = localWriters,
                        Year = year,
                        Language = localLanguage,
                        Transletor = localTransletor
                    });
                    break;
            }

            return await DataBase.SaveChangesAsync();
        }


        public async Task<IList<AbstractContent>> GetAllContent()
        {
            return await DataBase.Contents.ToListAsync();
        }

        public async Task<IList<AbstractContent>> GetContent(string name)
        {
            return await DataBase.Contents.Where(x => x.Name == name).ToListAsync();
        }
        public async Task<AbstractContent> GetContent(int _id)
        {
            return await DataBase.Contents.Where(x => x.Id == _id).FirstOrDefaultAsync();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DataBase.Dispose();
                }
                this.disposed = true;
            }
        }

        public async Task UpdateName(int id, string name)
        {

            AbstractContent content = await DataBase.Contents.Where(x => x.Id == id).FirstOrDefaultAsync();
            content.Name = name;
            DataBase.Entry(content).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }



        public async Task UpdateGenres(int id, List<int> genres)
        {
            AbstractContent content = await DataBase.Contents.Include(x => x.Genres).Where(x => x.Id == id).FirstOrDefaultAsync();
            var genresList = await DataBase.Genres.Where(x => genres.Contains(x.Id)).ToListAsync();
            content.Genres = genresList;
            DataBase.Entry(content).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }

        public async Task UpdateCheck(int id, int checkId)
        {
            AbstractContent content = await DataBase.Contents.Include(x => x.Check).Where(x => x.Id == id).FirstOrDefaultAsync();
            Check localCheck = await DataBase.Checks.Where(x => x.Id == checkId).FirstOrDefaultAsync();
            content.Check = localCheck;
            DataBase.Entry(content).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }

        public async Task DeleteContent(int id)
        {
            AbstractContent localContent = await DataBase.Contents.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (localContent != null)
            {
                foreach (var director in localContent.Directors.ToList()) localContent.Directors.Remove(director);
                foreach (var scenarist in localContent.Writers.ToList()) localContent.Writers.Remove(scenarist);
                foreach (var genre in localContent.Genres.ToList()) localContent.Genres.Remove(genre);
                foreach (var image in localContent.Images.ToList()) localContent.Images.Remove(image);
                var languageForLocalContent = localContent.Language;
                var translatorForLocalContent = localContent.Transletor;
                var checkForLocalContent = localContent.Check;
                Language language = null;
                Translator translator = null;
                Check check = null;
                foreach (var i in await DataBase.Votes.Where(x => x.Contents.Id == localContent.Id).ToListAsync())
                    if (i != null) DataBase.Votes.Remove(i);
                if (languageForLocalContent != null)
                {
                    language = await DataBase.Languages.Where(x => x.Id == languageForLocalContent.Id).FirstOrDefaultAsync();
                    language.Contents.Remove(localContent);
                    DataBase.Entry(language).State = EntityState.Modified;
                }
                if (translatorForLocalContent != null)
                {
                    translator = await DataBase.Transletors.Where(x => x.Id == translatorForLocalContent.Id).FirstOrDefaultAsync();
                    translator.Contents.Remove(localContent);
                    DataBase.Entry(translator).State = EntityState.Modified;
                }
                if (checkForLocalContent != null)
                {
                    check = await DataBase.Checks.Where(x => x.Id == checkForLocalContent.Id).FirstOrDefaultAsync();
                    check.Contents.Remove(localContent);
                    DataBase.Entry(check).State = EntityState.Modified;
                }
                DataBase.Contents.Remove(localContent);
                await DataBase.SaveChangesAsync();
            }
        }

        public async Task<AbstractContent> FindAsync(AbstractContent name)
        {
            return await DataBase.Contents.FindAsync(name);
        }

        public IQueryable<AbstractContent> GetContent(int page, int pageSize)
        {
            return DataBase.Contents.OrderByDescending(x => x.VoteUp).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<int> Count()
        {
            return await DataBase.Contents.CountAsync();
        }


        public async Task UpdateVoteUp(int id, int votes)
        {
            AbstractContent content = await DataBase.Contents.Where(x => x.Id == id).FirstOrDefaultAsync();

            content.VoteUp = votes;
            DataBase.Entry(content).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }

        public async Task UpdateVoteDown(int id, int votes)
        {
            AbstractContent content = await DataBase.Contents.Where(x => x.Id == id).FirstOrDefaultAsync();

            content.VoteDown = votes;
            DataBase.Entry(content).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }

        public IQueryable<AbstractContent> Query(Expression<Func<AbstractContent, bool>> query)
        {
            return DataBase.Contents.Where(query);
        }

        public IQueryable<AbstractContent> Query(Expression<Func<AbstractContent, bool>> query, string types)
        {
            IQueryable<AbstractContent> loc = loc = DataBase.Contents.Where(query);
            string[] baseTypes = { "Book", "Audio", "Video", "Empty" };
            foreach (var type in baseTypes)
            {
                if (!types.Split(new char[] { ';' }).Contains(type))
                {
                    switch (type)
                    {
                        case "Audio":
                            loc = loc.Where(y => !(y is Audio));

                            break;
                        case "Video":
                            loc = loc.Where(y => !(y is Video));
                            break;
                        case "Book":
                            loc = loc.Where(y => !(y is Book));
                            break;
                        case "Empty":
                            loc = loc.Where(y => !(y is Empty));
                            break;
                        default:
                            loc = loc.Where(y => false); ;
                            break;

                    }
                }
            }
            return loc;
        }

        public async Task UpdateContent(
            int id,
            string name,
            string year,
            IList<string> directors,
            IList<string> writers,
            IList<string> genres,
            IList<Image> list,
            string language,
            string transletor,
            string check)
        {
            AbstractContent content = await DataBase.Contents.Where(x => x.Id == id).FirstOrDefaultAsync();


            content.Name = name;
            content.Check = await DataBase.Checks.Where(x => x.Name == check).FirstOrDefaultAsync();
            content.Directors = await DataBase.Directors.Where(x => directors.Contains(x.Name)).ToListAsync();
            content.Genres = await DataBase.Genres.Where(x => genres.Contains(x.Name)).ToListAsync();
            if (list.Count > 0)
                content.Images = list;
            content.Writers = await DataBase.Writers.Where(x => writers.Contains(x.Name)).ToListAsync();
            content.Year = year;
            content.Language = await DataBase.Languages.Where(x => x.Name == language).FirstOrDefaultAsync();
            content.Transletor = await DataBase.Transletors.Where(x => x.Name == transletor).FirstOrDefaultAsync();
            DataBase.Entry(content).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }
    }
}
