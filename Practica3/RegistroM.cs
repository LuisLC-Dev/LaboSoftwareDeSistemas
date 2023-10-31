using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica3
{
    class RegistroM
    {
        string inicio;
        int cantNibb;
        string band;
        string simb;

        public RegistroM(string inicio, int cantNibb, string band, string simb) {
            if (inicio.Length > 6)
            {
                inicio = inicio.Substring(0, 6);
            }
            else
            {
                inicio = inicio.PadLeft(6, '0');
            }
            this.inicio = inicio;
            this.cantNibb = cantNibb;
            this.band = band;
            if (simb.Length > 6)
            {
                simb = simb.Substring(0, 6);
            }
            else
            {
                simb = simb.PadRight(6, '_');
            }
            this.simb = simb;
        }

        public string returnRegister()
        {
            return (inicio + "0" + cantNibb.ToString() + band + simb);
        }
    }
}
