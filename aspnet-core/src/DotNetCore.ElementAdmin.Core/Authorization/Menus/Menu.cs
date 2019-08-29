using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace DotNetCore.ElementAdmin.Authorization.Menus
{
    public class Menu : FullAuditedEntity<long>
    {
        /// <summary>
        /// 对于单用户这个key应该是唯一的
        /// </summary>
        /// <value></value>
        [Required]
        public string Key { get; set; }

        public int RoleId { get; set; }

        public long EfDemoId { get; set; }
    }
}