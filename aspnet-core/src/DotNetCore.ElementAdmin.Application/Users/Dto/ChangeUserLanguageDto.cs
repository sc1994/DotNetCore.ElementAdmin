using System.ComponentModel.DataAnnotations;

namespace DotNetCore.ElementAdmin.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}