using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterBook2.Data;
using TwitterBook2.Domain;

namespace TwitterBook2.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _dataContext.Posts.AddAsync(post);
            int created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);
            if (post == null)
                return false;

            _dataContext.Posts.Remove(post);
            int deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.SingleOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _dataContext.Posts.ToListAsync();
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            _dataContext.Posts.Update(postToUpdate);
            int updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(p => p.Id == postId);
            if (post == null || post.UserId != userId)
                return false;

            return true;
        }
    }
}
