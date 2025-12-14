using CoursesApi.Data.Entities;
using CoursesApi.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Common;
using Shared.Models.DTOs.Course;

namespace CoursesApi.Data.Repositories
{
    public class ParticipiantsRepository(CourseContext _context) : IParticipantsRepository
    {
        public async Task<Participant?> GetEntityAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Participants.FindAsync(id, cancellationToken);
        }

        public async Task<Participant?> GetEntityByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Participants.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public void AddEntity(Participant participant)
        {
            _context.Participants.Add(participant);
        }

        public void RemoveEntity(Participant participant)
        {
            _context.Participants.Remove(participant);
        }

        public void UpdateEntity(Participant participant)
        {
            _context.Participants.Update(participant);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
