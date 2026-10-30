using System;
using System.Collections.Generic;
using System.Text;

namespace PUchuariExamen.Models
{
    public static class ReglasNegocio
    {
        public const decimal CostoMembresiaAnual = 1200m;
        public const decimal PorcentajeMinimoInicial = 0.10m;
        public const decimal PorcentajeMaximoInicial = 0.50m;
        public const int NumeroCuotas = 5;
        public const decimal PorcentajeRecargoCuota = 0.02m;
        public const decimal AdicionalCasillero = 10m;
        public const int EdadMinima = 16;

        public static decimal MontoInicialMinimo => CostoMembresiaAnual * PorcentajeMinimoInicial;   // 120
        public static decimal MontoInicialMaximo => CostoMembresiaAnual * PorcentajeMaximoInicial;   // 600
        public static decimal RecargoPorCuota => CostoMembresiaAnual * PorcentajeRecargoCuota;       // 24

        public static readonly string[] Planes = { "Musculación", "Cardio", "Crossfit", "Full" };

        public static bool MontoInicialValido(decimal montoInicial) =>
            montoInicial >= MontoInicialMinimo && montoInicial <= MontoInicialMaximo;

        /// <summary>Cuota = (1200 − inicial) / 5 + 24 + (10 si casillero), redondeada a 2 decimales.</summary>
        public static decimal CalcularPagoMensual(decimal montoInicial, bool casillero)
        {
            decimal cuota = (CostoMembresiaAnual - montoInicial) / NumeroCuotas
                            + RecargoPorCuota
                            + (casillero ? AdicionalCasillero : 0m);
            return Math.Round(cuota, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>Pago total = monto inicial + 5 × cuota mensual.</summary>
        public static decimal CalcularPagoTotal(decimal montoInicial, decimal pagoMensual) =>
            montoInicial + NumeroCuotas * pagoMensual;
    }
}
