using System;
using System.Collections.Generic;
using System.Linq;
using UserStore.DAL.Entities;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace UserStore.DAL.Interfaces
{
    public interface IContentManager : IDisposable
    {
        /// <summary>
        /// Create and save to DataBase new Content for next properties:
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="year"></param>
        /// <param name="directors"></param>
        /// <param name="writers"></param>
        /// <param name="genres"></param>
        /// <param name="images"></param>
        /// <param name="language"></param>
        /// <param name="transletor"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        Task<int> CreateContent(string name, string path, string year, IList<string> directors, IList<string> writers, IList<string> genres, IList<Image> images, string language, string transletor, string check);
        /// <summary>
        /// Get content from DataBase with id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        Task<AbstractContent> GetContent(int _id);
        /// <summary>
        /// Get content from DataBase with name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IList<AbstractContent>> GetContent(string name);
        /// <summary>
        /// Get content for page from DataBase
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<AbstractContent> GetContent(int page, int pageSize);
        /// <summary>
        /// Get all contents
        /// </summary>
        /// <returns></returns>
        Task<IList<AbstractContent>> GetAllContent();
        /// <summary>
        /// Get content for expression
        /// </summary>
        /// <param name="quary"></param>
        /// <returns></returns>
        IQueryable<AbstractContent> Quary( Expression<Func<AbstractContent, bool>> quary);
        /// <summary>
        /// Get content with types for expression
        /// </summary>
        /// <param name="quary"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IQueryable<AbstractContent> Quary(Expression<Func<AbstractContent, bool>> quary, string types);

        /// <summary>
        /// Get count of contents
        /// </summary>
        /// <returns></returns>
        Task<int> Count();
        /// <summary>
        /// Check on exists of content
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<AbstractContent> FindAsync(AbstractContent content);
        /// <summary>
        /// Update name for content with id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task UpdateName(int id, string name);
        /// <summary>
        ///  Update genres for content with id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="genres"></param>
        /// <returns></returns>
        Task UpdateGenres(int id, List<int> genres);
        /// <summary>
        /// Update check for content with id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="checkId"></param>
        /// <returns></returns>
        Task UpdateCheck(int id, int checkId);
        /// <summary>
        /// Delete content from DataBase with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteContent(int id);
        /// <summary>
        /// Update value for Vote Up for content with id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="votes"></param>
        /// <returns></returns>
        Task UpdateVoteUp(int id, int votes);
        /// <summary>
        /// Update value for Vote Down for content with id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="votes"></param>
        /// <returns></returns>
        Task UpdateVoteDown(int id, int votes);
        /// <summary>
        /// Update content with id 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="year"></param>
        /// <param name="directors"></param>
        /// <param name="writers"></param>
        /// <param name="genres"></param>
        /// <param name="list"></param>
        /// <param name="language"></param>
        /// <param name="transletor"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        Task UpdateContent(int id, string name, string path, string year, IList<string> directors, IList<string> writers, IList<string> genres, IList<Image> list, string language, string transletor, string check);
    }
}
