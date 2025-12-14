using CoursesApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursesApi.Data.Repositories.Interfaces
{
    public interface IParticipantsRepository
    {
        public Task<Participant?> GetEntityAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<Participant?> GetEntityByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        public void AddEntity(Participant participant);
        public void RemoveEntity(Participant participant);
        public void UpdateEntity(Participant participant);

        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
