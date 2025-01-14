using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class CalculoSalarioRequest
    {
        [Range(1, 1000000, ErrorMessage = "O valor do salário está acima do suportado.")]
        public decimal SalarioBase { get; set; }

        [Range(0, 730000, ErrorMessage = "O valor digitado das horas está acima da quantidade mensal.")]
        public int HorasCinquentaPorcento { get; set; }

        [Range(0, 730000, ErrorMessage = "O valor digitado das horas está acima da quantidade mensal.")]
        public int HorasCemPorcento { get; set; }
    }
}
