using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterBook2.Domain;

namespace TwitterBook2.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync();

        Task<bool> CreatePostAsync(Post post);

        Task<Post> GetPostByIdAsync(Guid postId);

        Task<bool> UpdatePostAsync(Post postToUpdate);

        Task<bool> DeletePostAsync(Guid postId);
        Task<bool> UserOwnsPostAsync(Guid postId, string userId);
    }
}
