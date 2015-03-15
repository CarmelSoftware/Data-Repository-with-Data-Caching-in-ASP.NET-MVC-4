using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RepositoryWithCaching.Models;

namespace RepositoryWithCaching.Models
{
    public class MyDataRepository
    {
        #region Context
        private MyDataEntities _Ctx;
        public MyDataEntities Ctx
        {
            get
            {
                if (_Ctx == null)
                {
                    _Ctx = new MyDataEntities();
                }
                return _Ctx;
            }

        }
        private RepositoryCaching _Cache;

        public RepositoryCaching Cache
        {
            get
            {
                if (_Cache == null)
                {
                    _Cache = new RepositoryCaching();
                }
                return _Cache;
            }

        }

        #endregion

        public IQueryable<Comment> RetrieveComments()
        {
            if (Cache.IsInMemory("Comments"))
            {
                return Cache.FetchData<Comment>("Comments");
            }
            else
            {
                IQueryable<Comment> data = Ctx.Comments;
                Cache.Add("Comments", data, 60);
                return data;
            }
        }

        public Comment RetrieveComment(int Id)
        {
            return RetrieveComments().SingleOrDefault(c => c.CommentID == Id);
        }

        public void AddComment(Comment comment)
        {
            Ctx.Comments.Add(comment);
            Cache.Remove("Comments");
        }

        public void UpdateComment(Comment comment)
        {
            Ctx.Comments.Attach(comment);
            Ctx.Entry(comment).State = System.Data.EntityState.Modified;
            Cache.Remove("Comments");
        }

        public void DeleteComment(Comment comment)
        {
            Ctx.Comments.Remove(comment);
            Cache.Remove("Comments");
        }

        public void Save()
        {
            Ctx.SaveChanges();
        }

        public IQueryable<Blog> RetrieveBlogs()
        {
            if (Cache.IsInMemory("Blogs"))
            {
                return Cache.FetchData<Blog>("Blogs") as IQueryable<Blog>;
            }
            else
            {
                IQueryable<Blog> data = Ctx.Blogs;
                Cache.Add("Blogs",data,60);
                return data;
            }

        }
    }
}
