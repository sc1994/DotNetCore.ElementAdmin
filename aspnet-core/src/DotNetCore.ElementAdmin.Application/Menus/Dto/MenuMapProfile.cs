using AutoMapper;
using DotNetCore.ElementAdmin.Authorization.Menus;

namespace DotNetCore.ElementAdmin.Application.Menus.Dto
{
    public class MenuMapProfile: Profile
    {
        public MenuMapProfile()
        {
            // CreateMap<MenuDto, Menu>();

            // CreateMap<Menu, MenuDto>();

            // CreateMap<Role, RoleDto>().ForMember(x => x.GrantedPermissions,
            //     opt => opt.MapFrom(x => x.Permissions.Where(p => p.IsGranted)));
        }
    }
}