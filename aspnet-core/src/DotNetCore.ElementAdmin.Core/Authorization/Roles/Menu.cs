using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DotNetCore.ElementAdmin.EntityFrameworkCore.EntityFrameworkCore.Repositories.RoleExtendRepository
{
    public class MenuByRole :
        IEntity<long>,
        IHasCreationTime,
        IHasModificationTime,
        IHasDeletionTime,
        ISoftDelete
    {
        public bool IsTransient()
        {
            throw new NotImplementedException();
        }
        public long Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public DateTime? LastModificationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? DeletionTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    }
}