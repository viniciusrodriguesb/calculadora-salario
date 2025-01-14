using Application.DTO.Request;
using System.Globalization;

namespace Application.Services
{
    public class CalculoService
    {

        #region Construtor
        private const int horasMensaisPrevistas = 220;
        private const decimal pisoINSS = 1518.00m;
        private const decimal tetoINSS = 8157.40m;
        public CalculoService()
        {

        }
        #endregion

        public string ObterValorSalarioLiquido(CalculoSalarioRequest request)
        {
            var salarioHora = request.SalarioBase / horasMensaisPrevistas;

            var valorHoras50 = request.HorasCinquentaPorcento * (salarioHora * 1.5m);
            var valorHoras100 = request.HorasCemPorcento * (salarioHora * 2.0m);

            var totalBruto = request.SalarioBase + valorHoras50 + valorHoras100;

            decimal descontoInss = CalcularInss(totalBruto);

            var baseIRRF = totalBruto - descontoInss;

            decimal descontoIRRF = CalcularIRRF(baseIRRF);

            decimal salarioLiquido = totalBruto - descontoInss - descontoIRRF;

            return salarioLiquido.ToString("N2", new CultureInfo("pt-BR"));
        }

        #region Métodos privados
        private static decimal CalcularInss(decimal salarioBruto)
        {
            // Definindo os valores das faixas e alíquotas
            const decimal faixa1Teto = pisoINSS;
            const decimal faixa2Teto = 2571.29m;
            const decimal faixa3Teto = 3856.94m;
            const decimal faixa4Teto = tetoINSS;

            // Alíquotas para cada faixa
            const decimal aliquotaFaixa1 = 0.075m;
            const decimal aliquotaFaixa2 = 0.09m;
            const decimal aliquotaFaixa3 = 0.12m;
            const decimal aliquotaFaixa4 = 0.14m;

            if (salarioBruto <= faixa1Teto)
                return salarioBruto * aliquotaFaixa1;
            else if (salarioBruto <= faixa2Teto)
                return (faixa1Teto * aliquotaFaixa1) + ((salarioBruto - faixa1Teto) * aliquotaFaixa2);
            else if (salarioBruto <= faixa3Teto)
                return (faixa1Teto * aliquotaFaixa1) + ((faixa2Teto - faixa1Teto) * aliquotaFaixa2) + ((salarioBruto - faixa2Teto) * aliquotaFaixa3);
            else if (salarioBruto <= faixa4Teto)
                return (faixa1Teto * aliquotaFaixa1) + ((faixa2Teto - faixa1Teto) * aliquotaFaixa2) + ((faixa3Teto - faixa2Teto) * aliquotaFaixa3) + ((salarioBruto - faixa3Teto) * aliquotaFaixa4);
            else if (salarioBruto > faixa4Teto)
                return 877.24m;

            return 0;
        }

        private static decimal CalcularIRRF(decimal baseIRRF)
        {
            // Faixas e alíquotas
            const decimal faixa1Teto = 1903.98m;     // Isento
            const decimal faixa2Teto = 2826.65m;     // 7,5%
            const decimal faixa3Teto = 3751.05m;     // 15%
            const decimal faixa4Teto = 4664.68m;     // 22,5%

            const decimal aliquotaFaixa2 = 0.075m;
            const decimal aliquotaFaixa3 = 0.15m;
            const decimal aliquotaFaixa4 = 0.225m;
            const decimal aliquotaFaixa5 = 0.275m;

            const decimal deducaoFaixa2 = 142.80m;
            const decimal deducaoFaixa3 = 354.80m;
            const decimal deducaoFaixa4 = 636.13m;
            const decimal deducaoFaixa5 = 869.36m;

            // Calculando IRRF com base nas faixas
            if (baseIRRF <= faixa1Teto)
                return 0; // Isento            
            else if (baseIRRF <= faixa2Teto)
                return (baseIRRF * aliquotaFaixa2) - deducaoFaixa2;
            else if (baseIRRF <= faixa3Teto)
                return (baseIRRF * aliquotaFaixa3) - deducaoFaixa3;
            else if (baseIRRF <= faixa4Teto)
                return (baseIRRF * aliquotaFaixa4) - deducaoFaixa4;
            else
                return (baseIRRF * aliquotaFaixa5) - deducaoFaixa5;
        }
        #endregion

    }
}
