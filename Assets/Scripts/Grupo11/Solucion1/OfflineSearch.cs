using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.DataStructures;

public class OfflineSearch : MonoBehaviour
{
    public Nodo Search(BoardInfo board, CellInfo start, CellInfo[] goals)
    {
        // Inicializamos parámetros necesarios.
        List<Nodo> open = new List<Nodo>();                             //Lista abierta
        List<Vector2> previousPositionsList = new List<Vector2>();      //Lista de estados repetidos

        int g = 0;                                                      //g(n)
        int h = 0;                                                      //h(n)
        Nodo nodoStart = new Nodo(null, g + h, start);                  //Nodo inicial

        // Se añade nodo inicial a la lista abierta.
        open.Add(nodoStart);
        previousPositionsList.Add(new Vector2(nodoStart.getInfo().RowId, nodoStart.getInfo().ColumnId));

        // Mientras la lista no esté vacia...
        while (open.Count != 0)
        {

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

                //Se aumenta la g en 1 (Ya que el coste entre dos casillas siempre es 1)
                g++;

                //Meter hijos en la lista abierta open
                for (int i = 0; i < hijos.Length; i++)
                {
                    if (hijos[i] != null)
                    {
                        //Comprobamos si los nodos hijos han sido ya expandidos.
                        bool isRepeated = false;

                        for (int j = 0; j < previousPositionsList.Count; j++)
                        {
                            if (hijos[i].RowId == previousPositionsList[j].x && hijos[i].ColumnId == previousPositionsList[j].y)
                            {
                                isRepeated = true;
                            }
                        }

                        if (!isRepeated)
                        {
                            //Calcular la heuristica del nodo hijo i
                            h = CalculateHeuristic(goals[0], hijos[i]);

                            //Creamos el nodo hijo i y lo añadimos a la lista abierta además de a la lista de nodos ya expandidos
                            Nodo hijoNodo = new Nodo(primerNodo, g + h, hijos[i]);
                            open.Add(hijoNodo);
                            previousPositionsList.Add(new Vector2(hijoNodo.getInfo().RowId, hijoNodo.getInfo().ColumnId));
                        }
                    }
                }

                // Ordenar la lista abierta.
                open.Sort(delegate (Nodo a, Nodo b)
                {
                    return a.getfEstrella().CompareTo(b.getfEstrella());
                });
            }
        }
        return null;
    }

    private int CalculateHeuristic(CellInfo goal, CellInfo currentNode)
    {
        Vector2 goalPosition = new Vector2(goal.ColumnId, goal.RowId);
        Vector2 currentNodePosition = new Vector2(currentNode.ColumnId, currentNode.RowId);
        return (int)Mathf.Abs(goalPosition.x - currentNodePosition.x) + (int)Mathf.Abs(goalPosition.y - currentNodePosition.y);
    }
}
