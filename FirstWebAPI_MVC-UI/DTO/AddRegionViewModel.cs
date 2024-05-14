using System.ComponentModel.DataAnnotations;

namespace FirstWebAPI_MVC_UI.DTO
{
    public class AddRegionViewModel

        {
            [Required]
            [MinLength(3, ErrorMessage = "Code has to be minimum of 3 Characters!")]
            public string Code { get; set; }
            [Required]
            [MaxLength(30, ErrorMessage = "Name has to be maximum of 30 Characters!")]
            public string Name { get; set; }
            public string RegionImageUrl { get; set; }
        }
    }

