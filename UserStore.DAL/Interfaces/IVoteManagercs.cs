using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Identity;
using Microsoft.AspNet.Identity;
using UserStore.DAL.Entities;

namespace UserStore.DAL.Interfaces
{
    public interface IVoteManagercs : IDisposable
    {
        /// <summary>
        /// Save to DataBase new Vote
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="contentId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        Task NewVote(string userId, int contentId, int? vote);
        /// <summary>
        /// Get count of votes
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        Task<int> GetVotes(int contentId, int vote);
    }
}
