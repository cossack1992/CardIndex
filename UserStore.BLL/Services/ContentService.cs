using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.Interfaces;
using UserStore.DAL.Interfaces;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Convert;
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
                    var content = await DataBase.ContentManager
                        .Query(x => x.Id == contentDTO.Id)
                        .FirstOrDefaultAsync();
                    if (content != null)
                    {
                        var check = await DataBase.CheckManager.GetCheck(1);
                        contentDTO.Check = check.Name;
                        await SetProtertiesForContent(contentDTO);
                        await DataBase.ContentManager
                            .UpdateContent(
                                contentDTO.Id,
                                contentDTO.Name,
                                contentDTO.Year,
                                contentDTO.Directors,
                                contentDTO.Writers,
                                contentDTO.Genres,
                                ConvertTypeDTO.Convert(contentDTO.Images),
                                contentDTO.Language,
                                contentDTO.Translator,
                                contentDTO.Check
                            );
                        await DataBase.SaveAsync();

                        return new OperationDetails(true, "updating succedeed ", "");
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (ArgumentOutOfRangeException )
                {
                    throw;
                }
                catch (ConvertDTOException )
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new DataAccessException("Data Layer: ", ex);
                }
            }
            else
            {
                throw new ArgumentNullException("Content does not exist");
            }
        }
        public async Task<OperationDetails> CreateContent(ContentDTO contentDTO)
        {
            if (contentDTO != null)
            {
                try
                {
                    var content = await DataBase.ContentManager
                        .Query(x => x.Name == contentDTO.Name && x.Path == contentDTO.Path)
                        .FirstOrDefaultAsync();
                    if (content == null)
                    {
                        var check = await DataBase.CheckManager.GetCheck(1);
                        contentDTO.Check = check.Name;
                        await SetProtertiesForContent(contentDTO);
                        await DataBase.ContentManager.CreateContent(
                            contentDTO.Name,
                            contentDTO.Path,
                            contentDTO.Year,
                            contentDTO.Directors,
                            contentDTO.Writers,
                            contentDTO.Genres,
                            ConvertTypeDTO.Convert(contentDTO.Images),
                            contentDTO.Language,
                            contentDTO.Translator,
                            contentDTO.Check);
                        await DataBase.SaveAsync();
                        return new OperationDetails(true, "creating is succedeed ", "");
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Content with such id does not exist");
                    }
                }
                catch (ArgumentOutOfRangeException )
                {
                    throw;
                }
                catch (ConvertDTOException )
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new DataAccessException("Data Layer: ", ex);
                }
            }
            else
            {
                throw new ArgumentNullException("Content is empty");
            }
        }

        private async Task SetProtertiesForContent(ContentDTO contentDTO)
        {
            try
            {
                foreach (var director in contentDTO.Directors)
                {
                    var directorFromDB = await DataBase.DirectorManager.Get(director);
                    if (directorFromDB == null)
                    {
                        await DataBase.DirectorManager.SetNew(director);
                    }
                }

                foreach (var genre in contentDTO.Genres)
                {
                    var genreFromDB = (await DataBase.GenreManager.Get(genre));
                    if (genreFromDB == null)
                    {
                        await DataBase.GenreManager.SetNew(genre);
                    }
                }

                foreach (var scenarist in contentDTO.Writers)
                {
                    var scenaristFromDB = await DataBase.ScenaristManager.Get(scenarist);
                    if (scenaristFromDB == null)
                    {
                        await DataBase.ScenaristManager.SetNew(scenarist);
                    }
                }

                var translator = await DataBase.TrasletorManager.Get(contentDTO.Translator);
                if (translator == null)
                {
                    await DataBase.TrasletorManager.SetNew(contentDTO.Translator);
                }
                var language = await DataBase.LanguageManager.Get(contentDTO.Language);
                if (language == null)
                {
                    await DataBase.LanguageManager.SetNew(contentDTO.Language);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Data Layer: ", ex);
            }
        }

        public async Task<OperationDetails> DelateContent(int id)
        {
            try
            {
                await DataBase.ContentManager.DeleteContent(id);
                return new OperationDetails(true, "removal succeeded ", "");

            }
            catch (Exception ex)
            {
                throw new DataAccessException("Data layer:", ex);
            }
        }

        public async Task<OperationDetails> CheckContent(int id, int check)
        {
            try
            {
                await DataBase.ContentManager.UpdateCheck(id, check);
                return new OperationDetails(true, "updating of check succeeded ", "");
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Data layer:", ex);
            }
        }

        public async Task SetInitialContentData(ContentDTO firstDTO)
        {

            await CreateContent(firstDTO);
        }

        public async Task<List<string>> GetAllGenres()
        {

            List<string> GenresList = new List<string>();
            try
            {
                var genres = await DataBase.GenreManager
                    .GetAll()
                    .ToArrayAsync();
                foreach (var genre in genres)
                    GenresList.Add(genre.Name);
                return GenresList;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Data layer:", ex);
            }
        }
        public async Task<int> GetContentCount(string filter, string value, string types)
        {
            List<string> valueList = value.Split(';').ToList();
            try
            {
                var ids = await GetListId(filter, valueList);
                var i = await DataBase.ContentManager
                    .Query(x => ids.Contains(x.Id), types)
                    .CountAsync();
                return i;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Data layer:", ex);
            }
        }
        public async Task<ContentDTO> GetContent(int id)
        {
            try
            {
                return ConvertTypeDTO.Convert(await DataBase.ContentManager.GetContent(id));
            }
            catch (ConvertDTOException )
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Data layer:", ex);
            }
        }
        public async Task<List<ContentDTO>> GetContent(int page, int pageSize, string filter, string value, string types)
        {
            List<ContentDTO> contetList = new List<ContentDTO>();
            List<string> valuesList = value.Split(new char[] { ';' }).ToList();
            try
            {
                var listId = await GetListId(filter, valuesList);
                var contentsListFromDB =
                    await DataBase.ContentManager
                    .Query(x => listId.Contains(x.Id), types)
                    .OrderByDescending(x => x.VoteUp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                foreach (var content in contentsListFromDB)
                    contetList.Add(ConvertTypeDTO.Convert(content));

                return contetList;
            }
            catch (ConvertDTOException )
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Data layer:", ex);
            }

        }

        private async Task<List<int>> GetListId(string filter, List<string> valueList)
        {
            List<int> IDlist = new List<int>();
            foreach (var li in valueList)
            {
                IDlist.AddRange(await GetContent(filter, li));
            }
            return IDlist.Distinct().ToList();
        }

        private async Task<IEnumerable<int>> GetContent(string filter, string valueForSearch)
        {
            List<int> IDlist = new List<int>();
            try
            {
                switch (filter)
                {
                    case "admin":
                        {
                            IDlist.AddRange( await DataBase.ContentManager
                                .Query(x => x.Check.Id == 1)
                                .Select(
                                    content => content.Id
                                )
                                .ToListAsync()
                                );                           
                        }
                        break;
                    case "genre":
                        {
                            IDlist.AddRange(await DataBase.GenreManager
                                .Query(x => x.Name.Contains(valueForSearch))
                                .SelectMany(
                                     genre => genre.Contents
                                     .Where(x => x.Check.Id == 2)
                                     .Select(x => x.Id)
                                )
                                .ToListAsync()
                                );
                        }
                        break;
                    case "director":
                        {
                            IDlist.AddRange(await DataBase.DirectorManager
                                .Query(x => x.Name.Contains(valueForSearch))
                                .SelectMany(
                                    director => director.Contents
                                    .Where(x => x.Check.Id == 2)
                                    .Select(x => x.Id)
                                )
                                .ToListAsync()
                                );
                        }
                        break;
                    case "scenarist":
                        {
                            IDlist.AddRange(await DataBase.ScenaristManager
                                .Query(x => x.Name.Contains(valueForSearch))
                                .SelectMany(
                                    scenarist => scenarist.Contents
                                    .Where(x => x.Check.Id == 2)
                                    .Select(x => x.Id)                                   
                                )
                                .ToListAsync()
                                );
                        }
                        break;
                    case "year":
                        {
                            IDlist.AddRange(
                                    await DataBase.ContentManager
                                    .Query(x => x.Year == valueForSearch && x.Check.Id == 2)
                                    .Select(x => x.Id)
                                    .ToListAsync()
                                );
                        }
                        break;
                    case "transletor":
                        {
                            IDlist.AddRange(
                                    await DataBase.ContentManager
                                    .Query(x => x.Transletor.Name == valueForSearch && x.Check.Id == 2)
                                    .Select(x => x.Id)
                                    .ToListAsync()
                                );

                        }
                        break;
                    case "language":
                        {
                            IDlist.AddRange(
                                    await DataBase.ContentManager
                                        .Query(x => x.Language.Name == valueForSearch && x.Check.Id == 2)
                                        .Select(x => x.Id)
                                        .ToListAsync()
                                );

                        }
                        break;
                    case "name":
                        {
                            IDlist.AddRange(
                                    await DataBase.ContentManager
                                        .Query(x => x.Name == valueForSearch && x.Check.Id == 2)
                                        .Select(x => x.Id)
                                        .ToListAsync()
                                );

                        }
                        break;
                    case "search":
                        {
                            var idListFromGenre = DataBase.GenreManager
                                .Query(x => x.Name.Contains(valueForSearch))
                                .SelectMany(
                                     genre => genre.Contents
                                     .Where(x => x.Check.Id == 2)
                                     .Select(x => x.Id)
                                );

                            var idListFromDirector = DataBase.DirectorManager
                                .Query(x => x.Name.Contains(valueForSearch))
                                .SelectMany(
                                    director => director.Contents
                                    .Where(x => x.Check.Id == 2)
                                    .Select(x => x.Id)
                                );

                            var idListFromScenarist = DataBase.ScenaristManager
                                .Query(x => x.Name.Contains(valueForSearch))
                                .SelectMany(
                                    scenarist => scenarist.Contents
                                    .Where(x => x.Check.Id == 2)
                                    .Select(x => x.Id)
                                );

                            var idListFromAbstractContent = DataBase.ContentManager
                                .Query(
                                    x => 
                                    (
                                        x.Name.Contains(valueForSearch)
                                        ||
                                        x.Transletor.Name.Contains(valueForSearch)
                                        ||
                                        x.Language.Name.Contains(valueForSearch) 
                                        ||
                                        x.Year == valueForSearch
                                    ) 
                                    &&
                                    x.Check.Id == 2
                                )
                                .Select(x => x.Id);

                            var ids = idListFromGenre
                                .Union(idListFromDirector)
                                .Union(idListFromScenarist)
                                .Union(idListFromAbstractContent);
                                
                            IDlist.AddRange(await ids.ToListAsync());
                        }
                        break;
                    default:
                        {
                            IDlist.AddRange(
                                await DataBase.ContentManager
                                .Query(x => x.Check.Id == 2)
                                .Select(x => x.Id)
                                .ToListAsync()
                                );

                        }
                        break;
                }
                return IDlist;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Data layer:", ex);
            }

        }

        public async Task<OperationDetails> MakeVote(int id, string userName, int? vote)
        {
            try
            {
                var content = await DataBase.ContentManager.GetContent(id);

                if (content != null)
                {
                    await DataBase.VoteManager
                        .NewVote(userName, id, vote);
                    await DataBase.ContentManager
                        .UpdateVoteUp(
                            id,
                            await DataBase.VoteManager.GetVotes(id, 1)
                        );
                    await DataBase.ContentManager
                        .UpdateVoteDown(
                            id,
                            await DataBase.VoteManager.GetVotes(id, 2)
                        );
                    return new OperationDetails(true, "Voting  succeeded ", "");
                }
                else
                {
                    return new OperationDetails(false, "Voting  failed ", "");
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Data layer:", ex);
            }
        }
    }
}
