using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository.Post;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
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
                .ToListAsync();

            return posts;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
    public async Task<ErrorOr<Unit>> UpdatePostAsync(PostEntity post)
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

            return Unit.Value;
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
                .Where(p => p.Tags.Contains(tag))
                .ToListAsync();
            
            return posts;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}