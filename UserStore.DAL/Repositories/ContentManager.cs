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
            string contentType =path != "" ? Path.GetExtension(path):"";
            Check localCheck = await DataBase.Checks.Where(x => x.Name == check).FirstOrDefaultAsync();
            Translator localTransletor = await DataBase.Transletors.Where(x => x.Name == transletor).FirstOrDefaultAsync();
            Language localLanguage = await DataBase.Languages.Where(x => x.Name == language).FirstOrDefaultAsync();
            List<Genre> localGenres = await DataBase.Genres.Where(x => genres.Contains(x.Name)).ToListAsync();
            List<Director> localDirectors = await DataBase.Directors.Where(x => directors.Contains(x.Name)).ToListAsync();
            List<Scenarist> localWriters = await DataBase.Writers.Where(x => writers.Contains(x.Name)).ToListAsync();
            switch (contentType.ToUpper())
            {
                case ".PDF":
                    DataBase.Contents.Add(new Book { Name = name, Path = path, Check = localCheck, Directors = localDirectors, Genres = localGenres, Images = images, Writers = localWriters, Year = year, Language = localLanguage, Transletor = localTransletor });
                    break;
                case ".MP3":
                    DataBase.Contents.Add(new Audio { Name = name, Path = path, Check = localCheck, Directors = localDirectors, Genres = localGenres, Images = images, Writers = localWriters, Year = year, Language = localLanguage, Transletor = localTransletor });
                    break;
                case ".WEBM":
                    DataBase.Contents.Add(new Video { Name = name, Path = path, Check = localCheck, Directors = localDirectors, Genres = localGenres, Images = images, Writers = localWriters, Year = year, Language = localLanguage, Transletor = localTransletor });
                    break;
                case "":
                    DataBase.Contents.Add(new Empty { Name = name, Path = path, Check = localCheck, Directors = localDirectors, Genres = localGenres, Images = images, Writers = localWriters, Year = year, Language = localLanguage, Transletor = localTransletor });
                    break;
            }

            return await DataBase.SaveChangesAsync();
        }


        public async Task<IList<AbstractContent>> GetAllContent()
        {                      
            return await  DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).ToListAsync();            
        }

        public async Task<IList<AbstractContent>> GetContent(string name)
        {
            return await DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(x => x.Name == name).ToListAsync();
        }
        public async Task<AbstractContent> GetContent(int _id)
        {
            return await DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(x=>x.Id==_id).FirstOrDefaultAsync();
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
            
            AbstractContent item = await DataBase.Contents.Where(x => x.Id == id).FirstOrDefaultAsync();
            item.Name = name;
            DataBase.Entry(item).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }

        

        public async Task UpdateGenres(int id, List<int> genres)
        {
            AbstractContent item = await DataBase.Contents.Include(x => x.Genres).Where(x => x.Id == id).FirstOrDefaultAsync();
            var list = await DataBase.Genres.Where(x => genres.Contains(x.Id)).ToListAsync();
            item.Genres = list;
            DataBase.Entry(item).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }

        public async Task UpdateCheck(int id, int checkId)
        {
            AbstractContent item = await DataBase.Contents.Include(x => x.Check).Where(x=>x.Id == id).FirstOrDefaultAsync();
            Check local = await DataBase.Checks.Where(x => x.Id == checkId).FirstOrDefaultAsync();
            item.Check = local;
            DataBase.Entry(item).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }

        public async Task DeleteContent(int id)
        {
            AbstractContent item = await DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(x => x.Id == id).FirstOrDefaultAsync();
            try
            {
                if (item != null)
                {                    
                    foreach (var li in item.Directors.ToList()) item.Directors.Remove(li);
                    foreach (var li in item.Writers.ToList()) item.Writers.Remove(li);
                    foreach (var li in item.Genres.ToList()) item.Genres.Remove(li);
                    foreach (var li in item.Images.ToList()) item.Images.Remove(li);
                    var lan = item.Language;
                    var tra = item.Transletor;
                    var che = item.Check;
                    Language L = null;
                    Translator T = null;
                    Check C = null;
                    foreach (var i in await DataBase.Votes.Where(x => x.Contents.Id == item.Id).ToListAsync()) if(i != null) DataBase.Votes.Remove(i);
                    if (lan != null)
                    {
                        L = await DataBase.Languages.Where(x => x.Id == lan.Id).FirstOrDefaultAsync();
                        L.Contents.Remove(item);
                        DataBase.Entry(L).State = EntityState.Modified;
                    }
                    if (tra != null)
                    {
                        T = await DataBase.Transletors.Where(x => x.Id == tra.Id).FirstOrDefaultAsync();
                        T.Contents.Remove(item);
                        DataBase.Entry(T).State = EntityState.Modified;
                    }
                    if (che != null)
                    {
                        C = await DataBase.Checks.Where(x => x.Id == che.Id).FirstOrDefaultAsync();
                        C.Contents.Remove(item);
                        DataBase.Entry(C).State = EntityState.Modified;
                    }
                    
                    
                    
                    
                    
                    // await DataBase.SaveChangesAsync();
                    DataBase.Contents.Remove(item);
                    await DataBase.SaveChangesAsync();
                }
            }
            catch
            {

            }
            
        }

        public async Task<AbstractContent> FindAsync(AbstractContent name)
        {
            return await DataBase.Contents.FindAsync(name);
        }

        public IQueryable<AbstractContent> GetContent(int page, int pageSize)
        {
            var list = DataBase.Contents.OrderByDescending(x => x.VoteUp).Skip((page - 1) * pageSize).Take(pageSize);



            return list;
            
        }
        
        public async Task<int> Count()
        {
            return await DataBase.Contents.CountAsync();
        }
        

        public async Task UpdateVoteUp(int id, int votes)
        {
            AbstractContent item = await DataBase.Contents.Where(x => x.Id == id).FirstOrDefaultAsync();

            item.VoteUp = votes;
            DataBase.Entry(item).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }

        public async Task UpdateVoteDown(int id, int votes)
        {
            AbstractContent item = await DataBase.Contents.Where(x => x.Id == id).FirstOrDefaultAsync();

            item.VoteDown = votes;
            DataBase.Entry(item).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }

        public IQueryable<AbstractContent> Quary(Expression<Func<AbstractContent, bool>> quary)
        {
            return DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(quary);
        }

        public IQueryable<AbstractContent> Quary(Expression<Func<AbstractContent, bool>> quary, string types)
        {
            IQueryable<AbstractContent> loc = null;
            foreach(var type in types.Split(new char[] { ';'}))
            {
                switch(type)
                {
                    case "Audio":
                        loc = loc == null ? loc = DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(quary).Where(y => y is Audio)
                            : loc.Concat(DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(quary).Where(y => y is Audio));
                        break;
                    case "Video":
                        loc = loc == null ? loc = DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(quary).Where(y => y is Video)
                            : loc.Concat(DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(quary).Where(y => y is Video));
                        break;
                    case "Book":
                        loc = loc == null ? loc = DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(quary).Where(y => y is Book)
                            : loc.Concat(DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(quary).Where(y => y is Book));
                        break;
                    case "Empty":
                        loc = loc == null ? loc = DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(quary).Where(y => y is Empty)
                            : loc.Concat(DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(quary).Where(y => y is Empty));
                        break;
                    default:
                        loc = loc == null ? loc = DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(quary).Where(y => false)
                            : loc.Concat(DataBase.Contents.Include(x => x.Images).Include(x => x.Directors).Include(x => x.Genres).Include(x => x.Writers).Include(x => x.Check).Include(x => x.Language).Include(x => x.Transletor).Where(quary).Where(y => false));
                        break;

                }
            }
            return loc;
        }

        public async Task UpdateContent(int id, string name, string year, IList<string> directors, IList<string> writers, IList<string> genres, IList<Image> list, string language, string transletor, string check)
        {            
            AbstractContent content = await DataBase.Contents.Where(x=>x.Id == id).FirstOrDefaultAsync();
            
                        
            content.Name = name;
            content.Check = await DataBase.Checks.Where(x => x.Name == check).FirstOrDefaultAsync();
            content.Directors = await DataBase.Directors.Where(x => directors.Contains(x.Name)).ToListAsync();
            content.Genres = await DataBase.Genres.Where(x => genres.Contains(x.Name)).ToListAsync();
            if(list.Count > 0)
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
