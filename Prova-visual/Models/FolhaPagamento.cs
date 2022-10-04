using System;

namespace API.Models
{
    public class FolhaPagamento
    {
        public int Id { get; set; }

        public virtual Funcionario funcionario { get; set; }
        public int FuncionarioId { get; set; }

        public double ValorHora { get; set; }

        public double QuantidadeHoras { get; set; }

        public int AnoCompetencia { get; set; }

        public int MesCompetencia { get; set; }
        public double SalarioBruto { get; set; }

        public double  ImpostoRenda { get; set; }

        public double ImpostoINSS { get; set; }

        public double ImpostoFGTS { get; set; }

        public double SalarioLiquido { get; set; }

    }
}