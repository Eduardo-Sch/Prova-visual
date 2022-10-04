using System.Collections.Generic;
using System.Linq;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/folhas")]
    public class FolhaController : ControllerBase
    {
        private readonly DataContext _context;
        public FolhaController(DataContext context) =>
            _context = context;

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar() => Ok(_context.Folhas.ToList());

        [HttpPost]
        [Route("cadastrar")]
        public IActionResult Cadastrar([FromBody] FolhaPagamento folhaPagamento)
        {
            //calculo salario bruto
            folhaPagamento.SalarioBruto = folhaPagamento.ValorHora * folhaPagamento.QuantidadeHoras;

            //calculo imposto de renda
            if(folhaPagamento.SalarioBruto <= 1903.98)
            {
                folhaPagamento.ImpostoRenda = folhaPagamento.SalarioBruto;
            }

            else if(folhaPagamento.SalarioBruto >= 1903.99 && folhaPagamento.SalarioBruto <= 2826.65)
            {
                folhaPagamento.ImpostoRenda =   folhaPagamento.SalarioBruto * 0.075;
            } 

            else if(folhaPagamento.SalarioBruto >= 2826.66 && folhaPagamento.SalarioBruto <= 3751.05)
            {
                folhaPagamento.ImpostoRenda =   folhaPagamento.SalarioBruto * 0.15;
            } 

            else if(folhaPagamento.SalarioBruto >= 3751.06 && folhaPagamento.SalarioBruto <= 4664.68)
            {
                folhaPagamento.ImpostoRenda =    folhaPagamento.SalarioBruto * 0.225;
            }

            else 
            {
                folhaPagamento.ImpostoRenda =  folhaPagamento.SalarioBruto * 0.275;
            }

            //calculo INSS
             if(folhaPagamento.SalarioBruto <= 1693.72)
            {
                folhaPagamento.ImpostoINSS = folhaPagamento.SalarioBruto * 0.08;
            }

            else if(folhaPagamento.SalarioBruto >= 1693.73  && folhaPagamento.SalarioBruto <= 2822.90)
            {
                folhaPagamento.ImpostoINSS =   folhaPagamento.SalarioBruto * 0.09;
            } 

            else if(folhaPagamento.SalarioBruto >= 2822.91  && folhaPagamento.SalarioBruto <= 5645.80)
            {
                folhaPagamento.ImpostoINSS =   folhaPagamento.SalarioBruto * 0.11;
            } 

            else 
            {
                folhaPagamento.ImpostoINSS = folhaPagamento.SalarioBruto - 621.03 ;
            }

            //calculo FGTS

            folhaPagamento.ImpostoFGTS = folhaPagamento.SalarioBruto * 0.08;

            //calculo Salario Liquido

            folhaPagamento.SalarioLiquido = folhaPagamento.SalarioBruto - folhaPagamento.ImpostoRenda - folhaPagamento.ImpostoINSS;

            folhaPagamento.funcionario = _context.Funcionarios.Find(folhaPagamento.FuncionarioId);
            _context.Folhas.Add(folhaPagamento);
            _context.SaveChanges();
            return Created("", folhaPagamento);
        }

        [HttpGet]
        [Route("buscar/{cpf}/{MesCompetencia}/{AnoCompetencia}")]
        public IActionResult Buscar([FromRoute] string cpf, int MesCompetencia, int AnoCompetencia)
        {
            
            FolhaPagamento folhaPagamento = _context.Folhas.
                FirstOrDefault(f => f.MesCompetencia.Equals(MesCompetencia) && f.AnoCompetencia.Equals(AnoCompetencia));
            folhaPagamento.funcionario = _context.Funcionarios.Find(folhaPagamento.FuncionarioId);
            Funcionario funcionario = _context.Funcionarios.FirstOrDefault(f => f.Cpf.Equals(cpf));

            return folhaPagamento != null ? Ok(folhaPagamento) : NotFound();

        }

        [HttpGet]
        [Route("buscar1/{MesCompetencia}/{AnoCompetencia}")]
        public IActionResult Buscar1([FromRoute]  int MesCompetencia, int AnoCompetencia)
        {

            FolhaPagamento folhaPagamento = _context.Folhas.FirstOrDefault(f => f.MesCompetencia.Equals(MesCompetencia) && f.AnoCompetencia.Equals(AnoCompetencia));
            folhaPagamento.funcionario = _context.Funcionarios.Find(folhaPagamento.FuncionarioId);

            return folhaPagamento !=null? Ok(_context.Folhas.ToList().Where(f => f.MesCompetencia.Equals(MesCompetencia) && f.AnoCompetencia.Equals(AnoCompetencia))) : NotFound();

        }

    }
}