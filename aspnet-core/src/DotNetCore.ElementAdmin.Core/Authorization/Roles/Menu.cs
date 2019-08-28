using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace DotNetCore.ElementAdmin.EntityFrameworkCore.EntityFrameworkCore.Repositories.RoleExtendRepository
{
    public class Menu :
        Entity<long>,
        IHasCreationTime,
        IHasModificationTime,
        IHasDeletionTime,
        ISoftDelete
    {
        public DateTime CreationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? LastModificationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? DeletionTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}