using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;


namespace UserStore.DAL.Interfaces
{
    public interface IContentUnitOfWork : IDisposable
    {
        IContentManager ContentManager { get; }
        IManager<Language> LanguageManager { get; }
        IManager<Translator> TrasletorManager { get; }
        IManager<Scenarist> ScenaristManager { get; }
        IImageManager ImageManager { get; }
        IManager<Genre> GenreManager { get; }
        IManager<Director> DirectorManager { get; }
        ICheckManager CheckManager { get; }
        IVoteManagercs VoteManager { get; }
        Task SaveAsync();
    }
}
