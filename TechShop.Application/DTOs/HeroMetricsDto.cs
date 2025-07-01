using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Application.DTOs
{
    public class HeroMetricsDto
    {
        public int TotalCertificados { get; set; }
        public int CursosAsignados { get; set; }
        public double HorasTotales { get; set; }
        public double PorcentajeAprobados { get; set; }
    }
}
