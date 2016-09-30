using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using System.Security.Claims;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;

namespace UserStore.BLL.Interfaces
{
    public interface IContentService : IDisposable
    {
        /// <summary>
        /// Save new content to DataBase
        /// </summary>
        /// <param name="contentDTO"></param>
        /// <exception cref="DataAcssessException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        Task<OperationDetails> CreateContent(ContentDTO contentDTO);
        /// <summary>
        /// Return content by id
        /// </summary>
        /// <returns>Operation Details</returns>        
        Task<ContentDTO> GetContent(int id);
        /// <summary>
        /// Get "pageSizi" contents for "page" page with "filter" for "value"
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <param name="value"></param>
        /// <exception cref="DataAcssessException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>content</returns>
        Task<List<ContentDTO>> GetContent(int page, int pageSize, string filter, string value, string types);
        /// <summary>
        /// Udate contnent
        /// </summary>
        /// <param name="contentDTO"></param>
        /// <exception cref="DataAcssessException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        Task<OperationDetails> UpdateContent(ContentDTO contentDTO);
        /// <summary>
        /// Delete content
        /// </summary>
        /// <param name="contentDTO"></param>
        /// <exception cref="DataAcssessException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>list of contents</returns>
        Task<OperationDetails> DelateContent(int id);
        /// <summary>
        /// Update Check for content with id
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="DataAcssessException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>Operation Details</returns>
        Task<OperationDetails> CheckContent(int id, int check);
        /// <summary>
        /// Initialize new Data for DataBase
        /// </summary>
        /// <param name="firstDTO"></param>
        /// <exception cref="DataAcssessException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Operation Details</returns>
        Task SetInitialContentData(ContentDTO firstDTO);
        /// <summary>
        /// Get all genres from DataBsae
        /// </summary>
        /// /// <exception cref="DataAcssessException"></exception>
        /// <returns>list of genres</returns>
        Task<List<string>> GetAllGenres();
        /// <summary>
        /// Get count of contents in DataBase for filter and value
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="value"></param>
        /// /// <exception cref="DataAcssessException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>count of contents</returns>
        Task<int> GetContentCount(string filter, string value, string types);
        /// <summary>
        ///  Update vote for content with id for user with Name 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDTO"></param>
        /// <param name="vote"></param>
        /// /// <exception cref="DataAcssessException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>Operation details</returns>
        Task<OperationDetails> MakeVote(int id, string user, int? vote);
        
    } 
}
