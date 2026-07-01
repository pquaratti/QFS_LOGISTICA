using System.Collections.Generic;

namespace FrontEnd.Models
{
    public class DepositoOcupacionViewModel
    {
        public Entidades.Deposito Deposito { get; set; }
        public List<ZonaOcupacionViewModel> Zonas { get; set; }
        public int TotalLugares { get; set; }
        public int LugaresOcupados { get; set; }
        public int LugaresLibres { get; set; }
        public int LugaresBloqueados { get; set; }
        public decimal PorcentajeOcupacion { get; set; }
        public bool TieneSaldosPorUbicacion { get; set; }

        public DepositoOcupacionViewModel()
        {
            Deposito = new Entidades.Deposito();
            Zonas = new List<ZonaOcupacionViewModel>();
        }
    }

    public class ZonaOcupacionViewModel
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public List<PasilloOcupacionViewModel> Pasillos { get; set; }
        public int TotalLugares { get; set; }
        public int LugaresOcupados { get; set; }
        public int LugaresLibres { get; set; }
        public int LugaresBloqueados { get; set; }
        public decimal PorcentajeOcupacion { get; set; }

        public ZonaOcupacionViewModel()
        {
            Pasillos = new List<PasilloOcupacionViewModel>();
        }
    }

    public class PasilloOcupacionViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Posiciones { get; set; }
        public int Alturas { get; set; }
        public decimal AlturaNivel { get; set; }
        public decimal PesoMaximo { get; set; }
        public int TotalLugares { get; set; }
        public int LugaresOcupados { get; set; }
        public int LugaresLibres { get; set; }
        public int LugaresBloqueados { get; set; }
        public decimal PorcentajeOcupacion { get; set; }
        public List<UbicacionOcupacionViewModel> Ubicaciones { get; set; }

        public PasilloOcupacionViewModel()
        {
            Ubicaciones = new List<UbicacionOcupacionViewModel>();
        }
    }

    public class UbicacionOcupacionViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int Posicion { get; set; }
        public string Nivel { get; set; }
        public decimal Altura { get; set; }
        public decimal Longitud { get; set; }
        public decimal Anchura { get; set; }
        public decimal CapacidadCubica { get; set; }
        public decimal PesoMaximo { get; set; }
        public string Estado { get; set; }
        public int PorcentajeOcupacion { get; set; }
        public bool EsOcupada { get; set; }
        public bool EsBloqueada { get; set; }
        public bool EsUbicacionReal { get; set; }
    }

    public class WmsEstadoUbicacionRequest
    {
        public int UbicacionID { get; set; }
        public int PasilloID { get; set; }
        public int Posicion { get; set; }
        public string Nivel { get; set; }
        public string Estado { get; set; }
    }

}