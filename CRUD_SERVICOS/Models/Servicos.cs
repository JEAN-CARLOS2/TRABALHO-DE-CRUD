using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_SERVICOS.Models
{
    internal class Servicos
    {
        public int id_servico { get; set; }

        public double valor { get; set; }

        public string descricao { get; set; }

        public TimeOnly tempo { get; set; }
    }
}
