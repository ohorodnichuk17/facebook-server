using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository.Post;
using Facebook.Application.Post.Query.GetFriendsPosts;
using Facebook.Domain.Constants.ContentVisibility;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories;

public class PostRepository(FacebookDbContext context) : Repository<PostEntity>(context), IPostRepository
{
    public new async Task<ErrorOr<IEnumerable<PostEntity>>> GetAllAsync()
    {
        try
        {
            var posts = await dbSet
                .Include(p => p.Action)
                .Include(p => p.SubAction)
                .Include(p => p.Feeling)
                .Include(p => p.Images)
                .Include(p => p.User)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return posts;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
    public async Task<ErrorOr<MediatR.Unit>> UpdatePostAsync(PostEntity post)
    {
        try
        {
            var existingPost = await context.Posts.FindAsync(post.Id);

            if (existingPost == null)
            {
                return Error.Failure("Post not found");
            }

            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.Tags = post.Tags;
            existingPost.Location = post.Location;
            existingPost.Images = post.Images;
            existingPost.IsArchive = post.IsArchive;

            context.Posts.Update(existingPost);
            await context.SaveChangesAsync();

            return MediatR.Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
    public async Task<ErrorOr<PostEntity>> GetPostByIdAsync(Guid requestPostId)
    {
        if (requestPostId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(requestPostId), "Post id cannot be null");
        }

        var post = await context.Posts.FindAsync(requestPostId);

        if (post == null)
        {
            return Error.NotFound();
        }

        return post;
    }

    public async Task<ErrorOr<List<PostEntity>>> SearchPostsByTags(string tag)
    {
        try
        {
            if (string.IsNullOrEmpty(tag))
            {
                return Error.Failure("No search criteria provided");
            }

            var posts = await context.Posts
                .Include(p => p.Action)
                .Include(p => p.SubAction)
                .Include(p => p.Feeling)
                .Include(p => p.Images)
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Where(p => p.Tags.Contains(tag))
                .ToListAsync();

            return posts;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<PaginationResponse>> GetFriendsPostsAsync(Guid userId, int pageNumber, int pageSize)
    {
        try
        {
            var user = await context.Users.FindAsync(userId);

            if (user == null)
            {
                return Error.NotFound();
            }

            var friendIds = await context.FriendRequests
                .Where(uf => (uf.ReceiverId == user.Id || uf.SenderId == user.Id) && uf.IsAccepted)
                .Select(uf => uf.ReceiverId == user.Id ? uf.SenderId : uf.ReceiverId)
                .ToListAsync();

            var totalCount = context.Posts
                .Where(p => (friendIds.Contains(p.UserId) && p.Visibility != ContentVisibility.Private) || p.UserId == userId)
                .Count();

            var posts = await context.Posts
                .Include(p => p.Action)
                .Include(p => p.SubAction)
                .Include(p => p.Feeling)
                .Include(p => p.Images)
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Where(p => (friendIds.Contains(p.UserId) && p.Visibility != ContentVisibility.Private) || p.UserId == userId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            if (!posts.Any())
            {
                return Error.NotFound();
            }

            posts = posts.Where(post => post.ExcludedFriends == null || !post.ExcludedFriends.Contains(user.Id)).ToList();

            return new PaginationResponse { TotalCount = totalCount, Posts = posts };
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

}