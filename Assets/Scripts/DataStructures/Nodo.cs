using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
    public class Nodo
    {
        Nodo nodoPadre = null;
        int f = 0;
        CellInfo info;

        public Nodo(Nodo p, int f_, CellInfo info_)
        {
            nodoPadre = p;
            f = f_;
            info = info_;
        }

        public Nodo getNodoPadre()
        {
            return this.nodoPadre;
        }

        public int getfEstrella()
        {
            return this.f;
        }

        public CellInfo getInfo()
        {
            return this.info;
        }
    }
}