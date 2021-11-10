using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
    public class Nodo
    {
        Nodo nodoPadre = null;
        int f = 0;
        public int h = 0;
        public int g = 0;
        CellInfo info;

        public Nodo(Nodo p, int f_, CellInfo info_, int gg, int hh)
        {
            g = gg;
            h = hh;
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