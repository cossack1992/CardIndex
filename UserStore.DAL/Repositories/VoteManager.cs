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

namespace UserStore.DAL.Repositories
{
    public class VoteManager : IVoteManagercs
    {
        public ApplicationContext DataBase { get; set; }
        public VoteManager(ApplicationContext db)
        {
            DataBase = db;
        }
        public async Task<int> GetVotes(int id, int vote)
        {
            return await DataBase.Votes.Where(x => x.Contents.Id == id).Where(x => x.UpDown.Id == vote).CountAsync();
        }

        public async  Task NewVote(string userEmail, int contentId, int? vote)
        {

            var user = await DataBase.Users.Where(x => x.Email == userEmail).FirstOrDefaultAsync();
            var content = await DataBase.Contents.Where(x => x.Id == contentId).FirstOrDefaultAsync();
            var Vote = await DataBase.UpDowns.Where(x => x.Vote == vote).FirstOrDefaultAsync();
            Vote newVote = new Vote { User = user, Contents = content, UpDown = Vote };
            if (user != null && content != null && Vote != null)
            {
                if ((await DataBase.Votes.Where(x => x.User.Id == user.Id && x.Contents.Id == content.Id).FirstOrDefaultAsync()) == null) DataBase.Votes.Add(newVote);
                await DataBase.SaveChangesAsync();
            }
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
    }
}
