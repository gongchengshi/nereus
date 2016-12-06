using System.ComponentModel.DataAnnotations;

namespace Nereus.Models
{
   public class Url
   {
      public long Id { get; set; }
      [Required]
      [MaxLength(1024)]
      //[Url]
      public string Path { get; set; }
   }
}