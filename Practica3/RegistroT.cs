using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica3
{
    class RegistroT
    {
        public string inicio;
        public int size = 0;
        string objCode;


        public RegistroT()
        {

        }
        public RegistroT(string inicio)
        {
            if (inicio.Length > 6)
            {
                inicio = inicio.Substring(0, 6);
            }
            else
            {
                inicio = inicio.PadLeft(6, '0');
            }
            this.inicio = inicio;
        }

        public void setInicio(String inicio)
        {
            if (inicio.Length > 6)
            {
                inicio = inicio.Substring(0, 6);
            }
            else
            {
                inicio = inicio.PadLeft(6, '0');
            }
            this.inicio = inicio;
        }

        public void addObjCode(int size, string objC)
        {
            this.size += size;
            if (objC.Contains("*")){
                objC = objC.Remove(objC.Length - 1);
            }
            objCode += objC;
        }

        public string returnRegister()
        {
            if (size < 16)
                return (inicio + "0" + Convert.ToString(size, 16) /* convertir el size a hexadecimal+ */ + objCode);
            else
                return (inicio + Convert.ToString(size, 16) /* convertir el size a hexadecimal+ */ + objCode);
        }
    }
}
