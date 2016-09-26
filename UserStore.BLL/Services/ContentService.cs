using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.Interfaces;
using UserStore.DAL.Interfaces;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using System.Data.Entity;

namespace UserStore.BLL.Services
{
    public class ContentService : IContentService
    {
        IContentUnitOfWork DataBase { get; set; }
        public ContentService(IContentUnitOfWork uow)
        {
            DataBase = uow;
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
        public async Task<OperationDetails> UpdateContent(ContentDTO contentDTO)
        {
            if (contentDTO != null)
            {
                try
                {
                    var content = await DataBase.ContentManager.Quary(x => x.Id == contentDTO.Id).FirstOrDefaultAsync();
                    if (content != null)
                    {
                        var check = await DataBase.CheckManager.GetCheck(1);
                        contentDTO.Check = check.Name;
                        await SetProtertiesForContent(contentDTO);
                        await DataBase.ContentManager.UpdateContent(contentDTO.Id, contentDTO.Name, contentDTO.Path, contentDTO.Year, contentDTO.Directors, contentDTO.Writers, contentDTO.Genres, ConvertTypeDTO.Convert(contentDTO.Images), contentDTO.Language, contentDTO.Transletor, contentDTO.Check);
                        await DataBase.SaveAsync();
                        return new OperationDetails(true, "updating succedeed ", "");
                    }
                    else
                    {
                        return new OperationDetails(false, "Content is empty ", "");
                    }
                }
                catch
                {
                    return new OperationDetails(false, "updating failed ", "");
                }
            }
            else
            {
                return new OperationDetails(false, "Content is empty ", "");
            }
        }
        public async Task<OperationDetails> CreateContent(ContentDTO contentDTO)
        {

            if (contentDTO != null)
            {
                try
                {
                    var content = await DataBase.ContentManager.Quary(x => x.Name == contentDTO.Name && x.Path == contentDTO.Path).FirstOrDefaultAsync();
                    if (content == null)
                    {
                        var check = await DataBase.CheckManager.GetCheck(1);
                        contentDTO.Check = check.Name;
                        await SetProtertiesForContent(contentDTO);
                        await DataBase.ContentManager.CreateContent(contentDTO.Name, contentDTO.Path, contentDTO.Year, contentDTO.Directors, contentDTO.Writers, contentDTO.Genres, ConvertTypeDTO.Convert(contentDTO.Images), contentDTO.Language, contentDTO.Transletor, contentDTO.Check);
                        await DataBase.SaveAsync();
                        return new OperationDetails(true, "creating is succedeed ", "");
                    }
                    else
                    {
                        return new OperationDetails(false, "Content is already exist ", "");
                    }
                }
                catch
                {
                    return new OperationDetails(false, "Creation failed ", "");
                }
            }
            else
            {
                return new OperationDetails(false, "Content is empty ", "");
            }
        }

        private async Task SetProtertiesForContent(ContentDTO contentDTO)
        {
            foreach (var li in contentDTO.Directors)
            {
                var director = await DataBase.DirectorManager.Get(li);
                if (director == null)
                {
                    await DataBase.DirectorManager.SetNew(li);
                }
            }
            foreach (var li in contentDTO.Genres)
            {
                var genre = (await DataBase.GenreManager.Get(li));
                if (genre == null)
                {
                    await DataBase.GenreManager.SetNew(li);
                }
            }

            foreach (var li in contentDTO.Writers)
            {
                var scenarist = await DataBase.ScenaristManager.Get(li);
                if (scenarist == null)
                {
                    await DataBase.ScenaristManager.SetNew(li);
                }
            }

            //foreach (var li in contentDTO.Images)
            //{
            //    if (li != null)
            //    {
            //        await DataBase.ImageManager.SetNew(li.Name, li.Path);
            //    }
            //}
            var transletor = await DataBase.TrasletorManager.Get(contentDTO.Transletor);
            if (transletor == null)
            {
                await DataBase.TrasletorManager.SetNew(contentDTO.Transletor);
            }
            var language = await DataBase.LanguageManager.Get(contentDTO.Language);
            if (language == null)
            {
                await DataBase.LanguageManager.SetNew(contentDTO.Language);
            }
        }

        public async Task<OperationDetails> DelateContent(int id)
        {
            try
            {
                await DataBase.ContentManager.DeleteContent(id);
                return new OperationDetails(true, "removal succeeded ", "");

            }
            catch
            {
                return new OperationDetails(false, "removal failed", "");
            }
        }

        public async Task<OperationDetails> CheckContent(int id, int check)
        {
            try
            {


                await DataBase.ContentManager.UpdateCheck(id, check);

                return new OperationDetails(true, "updating of check succeeded ", "");

            }
            catch
            {
                return new OperationDetails(false, "updating of check failed", "");
            }
        }

        public async Task SetInitialContentData(ContentDTO firstDTO)
        {

            await CreateContent(firstDTO);

        }


        public async Task<List<string>> GetAllGenres()
        {

            List<string> list = new List<string>();
            try
            {
                foreach (var li in await DataBase.GenreManager.GetAll().ToListAsync()) list.Add(li.Name);
                return list;
            }
            catch
            {
                return null;
            }
        }
        public async Task<int> GetContentCount(string filter, string value, string types)
        {
            List<int> listId = new List<int>();
            List<string> valueList = value.Split(new char[] { ';' }).ToList();
            try
            {
                listId = await GetListId(filter, listId, valueList);
                var i = await DataBase.ContentManager.Quary(x => listId.Contains(x.Id), types).CountAsync();
                return i;
            }
            catch
            {
                throw;
            }
        }
        public async Task<ContentDTO> GetContent(int id)
        {
            try
            {
                return ConvertTypeDTO.Convert(await DataBase.ContentManager.GetContent(id));
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<ContentDTO>> GetContent(int page, int pageSize, string filter, string value, string types)
        {
            List<ContentDTO> list = new List<ContentDTO>();
            List<int> listId = new List<int>();
            List<string> valueList = value.Split(new char[] { ';' }).ToList();

            try
            {
                listId = await GetListId(filter, listId, valueList);


                foreach (var li in await DataBase.ContentManager.Quary(x => listId.Contains(x.Id), types).OrderByDescending(x => x.VoteUp).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync()) list.Add(ConvertTypeDTO.Convert(li));

                return list;
            }
            catch
            {
                throw;
            }

        }

        private async Task<List<int>> GetListId(string filter, List<int> listId, List<string> valueList)
        {
            foreach (var li in valueList)
            {
                listId.AddRange(await GetContent(filter, li));
            }
            return listId.Distinct().ToList();
        }

        private async Task<IEnumerable<int>> GetContent(string filter, string li)
        {
            List<int> list = new List<int>();
            try
            {
                switch (filter)
                {
                    case "admin":
                        {
                            var local = await DataBase.ContentManager.Quary(x => x.Check.Id == 1).ToListAsync();
                            if (local != null) list.AddRange(local.Select(x => x.Id));
                        }
                        break;
                    case "genre":
                        {
                            var local = await DataBase.GenreManager.Quary(x => x.Name == li).FirstOrDefaultAsync();
                            if (local != null) list.AddRange(local.Contents.Where(x => x.Check.Id == 2).Select(x => x.Id).ToList());

                        }
                        break;
                    case "director":
                        {
                            var local = await DataBase.DirectorManager.Quary(x => x.Name == li).FirstOrDefaultAsync();
                            if (local != null) list.AddRange(local.Contents.Where(x => x.Check.Id == 2).Select(x => x.Id).ToList());

                        }
                        break;
                    case "scenarist":
                        {
                            var local = await DataBase.ScenaristManager.Quary(x => x.Name == li).FirstOrDefaultAsync();
                            if (local != null) list.AddRange(local.Contents.Where(x => x.Check.Id == 2).Select(x => x.Id).ToList());

                        }
                        break;
                    case "year":
                        {
                            list.AddRange(await DataBase.ContentManager.Quary(x => x.Year == li && x.Check.Id == 2).Select(x => x.Id).ToListAsync());
                        }
                        break;
                    case "transletor":
                        {
                            list.AddRange(await DataBase.ContentManager.Quary(x => x.Transletor.Name == li && x.Check.Id == 2).Select(x => x.Id).ToListAsync());

                        }
                        break;
                    case "language":
                        {
                            list.AddRange(await DataBase.ContentManager.Quary(x => x.Language.Name == li && x.Check.Id == 2).Select(x => x.Id).ToListAsync());

                        }
                        break;
                    case "name":
                        {
                            list.AddRange(await DataBase.ContentManager.Quary(x => x.Name == li && x.Check.Id == 2).Select(x => x.Id).ToListAsync());

                        }
                        break;
                    case "search":
                        {
                            var local2 = (await DataBase.GenreManager.Quary(x => x.Name.Contains(li)).FirstOrDefaultAsync());
                            if (local2 != null) list.AddRange(local2.Contents.Where(x => x.Check.Id == 2).Select(x => x.Id).ToList());
                            var local3 = (await DataBase.DirectorManager.Quary(x => x.Name.Contains(li)).ToListAsync());
                            if (local3 != null) local3.ForEach(director => list.AddRange(director.Contents.Where(x => x.Check.Id == 2).Select(x => x.Id).ToList()));
                            var local4 = (await DataBase.ScenaristManager.Quary(x => x.Name.Contains(li)).ToListAsync());
                            if (local4 != null) local4.ForEach(scenarist => list.AddRange(scenarist.Contents.Where(x => x.Check.Id == 2)?.Select(x => x.Id).ToList()));
                            var local5 = (await DataBase.ContentManager.Quary(x => (x.Name.Contains(li) || x.Transletor.Name.Contains(li) || x.Language.Name.Contains(li) || x.Year == li) && x.Check.Id == 2).Select(x => x.Id).ToListAsync());
                            if (local5 != null) list.AddRange(local5);



                        }

                        break;
                    default:
                        {
                            list.AddRange(await DataBase.ContentManager.Quary(x => x.Check.Id == 2).Select(x => x.Id).ToListAsync());

                        }
                        break;


                }

                return list;
            }
            catch
            {
                throw;
            }

        }

        public async Task<OperationDetails> MakeVote(int id, string userName, int? vote)
        {            
            try
            {
                var content = await DataBase.ContentManager.GetContent(id);

                if ( content != null)
                {
                    await DataBase.VoteManager.NewVote(userName, id, vote);
                    await DataBase.ContentManager.UpdateVoteUp(id, await DataBase.VoteManager.GetVotes(id, 1));
                    await DataBase.ContentManager.UpdateVoteDown(id, await DataBase.VoteManager.GetVotes(id, 2));
                    return new OperationDetails(true, "Voting  succeeded ", "");
                }
                else
                {
                    return new OperationDetails(false, "Voting  failed ", "");
                }
            }
            catch
            {
                return new OperationDetails(false, "Voting  failed ", "");
            }           
        }
    }
}
