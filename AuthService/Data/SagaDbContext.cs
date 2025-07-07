using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit;
using AuthAPI.Sagas.Instances;

namespace AuthAPI.Data
{
    //public class SagaContext : SagaDbContext
    //{
    //    public SagaContext(DbContextOptions options) : base(options) { }

    //    protected override IEnumerable<ISagaClassMap> Configurations =>
    //        new ISagaClassMap[] { new UserStateMap() };
    //}

    //public class UserStateMap : SagaClassMap<UserState>
    //{
    //    protected override void Configure(EntityTypeBuilder<UserState> entity, ModelBuilder model)
    //    {
    //        entity.Property(x => x.CurrentState);
    //        entity.Property(x => x.UserName);
    //        entity.Property(x => x.CreatedAt);
    //    }
    //}
}
