using HB.Case.Models.Enums;
using HB.Case.Models.PostProduct;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HB.Case.Api.Models.Dtos
{
    //Note: Normalde her endpoint için kendine özgü request ve response dto'ları oluşturmak daha sade ve temiz bir çözüm.
    //Çünkü api büyüdükçe bu dto'lara binen yük ağırlaşıyor ve farklı case'ler için karmaşa oluşmaya başlıyor.
    //Örn; Post ve Put için farklı validasyon seçenekleri gerekebiliyor.Mesela Put'da Name değişemez gibi.
    //Ancak mevcut iş kapsamında bu haliyle işimi görüyor. 
    public class ProductDto : Dto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public Currency Currency { get; set; }
        public string CategoryId { get; set; }

        public CategoryDto Category { get; internal set; }

        [JsonIgnore]
        public override string RedisKey => $"product:{Id}";
    }
}
