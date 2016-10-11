using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.EF;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;

using UserStore.DAL.Identity;

namespace UserStore.DAL.Repositories
{
    public class ContentUnitOfWork : IContentUnitOfWork
    {
        private ApplicationContext db;
        
        private IContentManager contentManager;
        private ICheckManager checkManager;
        private IManager<Director> directorManager;
        private IManager<Genre> genreManager;
        private IImageManager imageManager;
        private IManager<Language> languageManager;
        private IManager<Scenarist> scenaristManager;
        private IManager<Translator> transletorManager;
        private IVoteManagercs voteManager;

        public ContentUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            AddVinding();
        }
        private void AddVinding()
        {
            
            contentManager = new ContentManager(db);
            checkManager = new CheckManager(db);
            directorManager = new DirectorManager(db);
            genreManager = new GenreManager(db);
            imageManager = new ImageManager(db);
            languageManager = new LanguageManager(db);
            scenaristManager = new ScenaristManager(db);
            transletorManager = new TranslatorManager(db);
            voteManager = new VoteManager(db);
        }
        
        public IContentManager ContentManager
        {
            get { return contentManager; }
        }
        public ICheckManager CheckManager
        {
            get { return checkManager; }
        }
        public IManager<Director> DirectorManager
        {
            get { return directorManager; }
        }
        public IManager<Genre> GenreManager
        {
            get { return genreManager; }
        }
        public IImageManager ImageManager
        {
            get { return imageManager; }
        }
        public IManager<Language> LanguageManager
        {
            get { return languageManager; }
        }
        public IManager<Scenarist> ScenaristManager
        {
            get { return scenaristManager; }
        }
        public IManager<Translator> TrasletorManager
        {
            get { return transletorManager; }
        }
        

        public IVoteManagercs VoteManager
        {
            get
            {
                return voteManager;
            }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
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
                    
                    contentManager.Dispose();
                    checkManager.Dispose();
                    directorManager.Dispose();
                    genreManager.Dispose();
                    imageManager.Dispose();
                    languageManager.Dispose();
                    scenaristManager.Dispose();
                    transletorManager.Dispose();
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
