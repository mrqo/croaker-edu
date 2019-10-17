using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using edu_croaker.Data;
using edu_croaker.Dtos;
using LiteDB;

namespace edu_croaker.DataAccess
{
    public interface IRepository
    {
        #region Croaks
        Task<int> AddCroak(Croak croak);

        Task<Croak> FindCroak(int id);

        Task<IEnumerable<Croak>> FindCroaks();

        Task<IEnumerable<Croak>> FindCroaks(IEnumerable<int> ids);

        Task<IEnumerable<Croak>> FindCroaksByAuthor(string authorId);

        Task<bool> UpdateCroak(Croak croak);

        Task<bool> RemoveCroak(int id);
        #endregion

        #region Likes
        Task<int> AddLike(Like like);

        Task<Like> FindLike(string userId, int postId);

        Task<bool> RemoveLike(Like like);

        #endregion

        #region Hashtags
        Task<int> AddHashtag(Hashtag hashtag);

        Task<Hashtag> FindHashtag(string caption);

        Task<IEnumerable<Hashtag>> FindHashtags(IEnumerable<string> captions);

        Task<bool> UpdateHashtag(Hashtag ht);
        
        Task<bool> RemoveHashtag(int id);
        
        Task<IEnumerable<int>> FindCroakIdsWithHashtag(int id);

        Task<IEnumerable<HashtagPopularity>> GetHashtagPopularities(int maxCount);
        #endregion

        #region Users
        Task<PublicUserData> FindUser(string userId);

        Task<PublicUserData> FindUserWithDetails(string userId);

        Task<PublicUserData> FindUserDetails(string userId);

        Task<bool> UpdateUserDetails(PublicUserData userData);

        Task<int> AddFollower(Follower follower);

        Task<bool> RemoveFollower(Follower follower);

        Task<Follower> FindFollower(string followedUserId, string followingUserId);

        Task<IEnumerable<string>> FindAllFollowers(string followedUserId);

        Task<IEnumerable<string>> FindAllFollowedBy(string followingUserId);
        #endregion
    }
}