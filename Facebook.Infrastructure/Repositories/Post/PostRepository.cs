using ErrorOr;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.Post;

public class PostRepository : IPostRepository
{
    private readonly FacebookDbContext _context;

    public PostRepository(FacebookDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<IEnumerable<PostEntity>>> GetAllPostsAsync()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<ErrorOr<PostEntity>> GetPostByIdAsync(Guid id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
        {
            return Error.Failure("Post not found");
        }

        return post;
    }

    public async Task<ErrorOr<Guid>> CreatePostAsync(PostEntity post)
    {
        try
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post.Id;
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
            var existingPost = await _context.Posts.FindAsync(post.Id);

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

            _context.Posts.Update(existingPost);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message); 
        }
    }

    public async Task<ErrorOr<bool>> DeletePostAsync(Guid postId)
    {
        try
        {
            var post = await _context.Posts.FindAsync(postId);
            
            if (post == null)
            {
                return Error.Failure("Post not found");
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<Guid>> SavePostAsync(PostEntity post)
    {
        try
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post.Id;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

}