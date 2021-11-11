using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.DataStructures;

public class OnlineSearch : MonoBehaviour
{
    public Nodo SearchOnline(BoardInfo board, CellInfo start, CellInfo[] goals)
    {

        // Inicializamos parámetros necesarios.
        List<Nodo> open = new List<Nodo>();                             //Lista abierta

        int h = 0;                                                      //h(n)
        Nodo nodoStart = new Nodo(null, h, start);            //Nodo inicial

        // Se añade nodo inicial a la lista abierta.
        open.Add(nodoStart);

        // Sacar el primer nodo de la lista
        Nodo primerNodo = open[0];
        open.RemoveAt(0);

        // Si el primer nodo es goal, devolverlo
        if (primerNodo.getInfo().RowId == goals[0].RowId && primerNodo.getInfo().ColumnId == goals[0].ColumnId)
        {
            return primerNodo;
        }
        else
        {
            //Averiguar los hijos del nodo a expandir
            CellInfo[] hijos = primerNodo.getInfo().WalkableNeighbours(board);

            //Meter hijos en la lista abierta open
            for (int i = 0; i < hijos.Length; i++)
            {
                if (hijos[i] != null)
                {
                    //Calcular la heuristica del nodo hijo i
                    h = CalculateHeuristic(goals[0], hijos[i]);

                    //Creamos el nodo hijo i y lo añadimos a la lista abierta además de a la lista de nodos ya expandidos
                    Nodo hijoNodo = new Nodo(primerNodo, h, hijos[i]);
                    open.Add(hijoNodo);
                }
            }

            // Ordenar la lista abierta.
            open.Sort(delegate (Nodo a, Nodo b)
            {
                return a.getfEstrella().CompareTo(b.getfEstrella());
            });
        }
        return open[0];
    }

    private int CalculateHeuristic(CellInfo goal, CellInfo currentNode)
    {
        Vector2 goalPosition = new Vector2(goal.ColumnId, goal.RowId);
        Vector2 currentNodePosition = new Vector2(currentNode.ColumnId, currentNode.RowId);
        return (int)Mathf.Abs(goalPosition.x - currentNodePosition.x) + (int)Mathf.Abs(goalPosition.y - currentNodePosition.y);
    }
}