using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts.SampleMind
{
    public class RandomMind : AbstractPathMind
    {
        private Stack<Locomotion.MoveDirection> currentPlan = new Stack<Locomotion.MoveDirection>();

        public override void Repath()
        {
            currentPlan.Clear();
        }

        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            // si la Stack no está vacía, hacer siguiente movimiento
            if (currentPlan.Count != 0)
                return currentPlan.Pop();

            Debug.Log("No he encontrado la solución");
            // calcular camino, devuelve resultado de A*
            Nodo searchResult = Search(boardInfo, currentPos, goals);
            if (searchResult == null)
            {
                Debug.Log("Nodo nulo");
            }
            // recorre searchResult and copia el camino a currentPlan
            while (searchResult.getNodoPadre() != null)
            {
                //currentPlan.Push(searchResult.ProducedBy);
                currentPlan.Push(CalculateMovement(searchResult.getInfo(), searchResult.getNodoPadre().getInfo()));
                searchResult = searchResult.getNodoPadre();
            }

            // returns next move (pop Stack)
            if (currentPlan.Count != 0)
                return currentPlan.Pop();

            return Locomotion.MoveDirection.None;
        }

        private Locomotion.MoveDirection CalculateMovement(CellInfo target, CellInfo currentPos)
        {
            Vector2 targetPosition = new Vector2(target.ColumnId, target.RowId);
            Vector2 curentPosPosition = new Vector2(currentPos.ColumnId, currentPos.RowId);

            if (targetPosition.x < curentPosPosition.x) { return Locomotion.MoveDirection.Left; }
            if (targetPosition.x > curentPosPosition.x) { return Locomotion.MoveDirection.Right; }
            if (targetPosition.y < curentPosPosition.y) { return Locomotion.MoveDirection.Down; }
            if (targetPosition.y > curentPosPosition.y) { return Locomotion.MoveDirection.Up; }

            return Locomotion.MoveDirection.None;
        }

        private Nodo SearchOnline(BoardInfo board, CellInfo start, CellInfo[] goals)
        {
            // crea una lista vacía de nodos
            List<Nodo> open = new List<Nodo>();

            // node inicial
            // la g(n) en el inicial es 0 pero en los demás es 1, la h*(n) la resta de x e y del objetivo hasta la pos aactual y sumas ambas.
            CellInfo[] hijos = null;
            int h = 0;
            Nodo nodoStart = new Nodo(null, h, start, 0, h);

            // añade nodo inicial a la lista
            open.Add(nodoStart);

            // Sacar el primer nodo de la lista
            Nodo primerNodo = open[0];
            open.RemoveAt(0);

            // si el primer nodo es goal, returns current node
            if (primerNodo.getInfo().RowId == goals[0].RowId && primerNodo.getInfo().ColumnId == goals[0].ColumnId)
            {
                return primerNodo;
            }

            else
            {
                // expande vecinos del primer nodo de la lista abierta(calcula coste de cada uno, etc)y los añade en la lista
                //Vaciar hijos de nodos expandidos anteriormente
                if (hijos != null)
                {
                    for (int i = 0; i < hijos.Length; i++)
                    {
                        hijos[i] = null;
                    }
                    hijos = null;
                }
            }

            //Averiguar los hijos del nodo a expandir
            hijos = primerNodo.getInfo().WalkableNeighbours(board);

            //Meter hijos en la lista abierta open
            for (int i = 0; i < hijos.Length; i++)
            {
                if (hijos[i] == null)
                {
                }
                else
                {
                    //Calcular la heuristica del nodo hijo i
                    h = CalculateHeuristic(goals[0], hijos[i]);

                    //Creamos el nodo hijo i y lo añadimos a la lista abierta
                    //Nodo hijoNodo = new Nodo(primerNodo, h, hijos[i]);
                    //open.Add(hijoNodo);
                }
            }

            // ordena lista
            open.Sort(delegate (Nodo a, Nodo b)
            {
                return a.getfEstrella().CompareTo(b.getfEstrella());
            });

            return open[0];
        }

        private Nodo Search(BoardInfo board, CellInfo start, CellInfo[] goals)
        {
            // crea una lista vacía de nodos
            List<Nodo> open = new List<Nodo>();

            // node inicial
            // la g(n) en el inicial es 0 pero en los demás es 1, la h*(n) la resta de x e y del objetivo hasta la pos aactual y sumas ambas.
            int g = 0;
            int h = 0;
            Nodo nodoStart = new Nodo(null, g + h, start, g, h);

            // añade nodo inicial a la lista
            open.Add(nodoStart);

            int k = 0;
            // mientras la lista no esté vacia
            while (open.Count != 0 && k < 100)
            {
                //Debug.Log(open.Count);
                // Sacar el primer nodo de la lista
                Nodo primerNodo = open[0];
                open.RemoveAt(0);
                Debug.Log("Paso por " + primerNodo.getInfo().RowId + ", " + primerNodo.getInfo().ColumnId + " | f = " + primerNodo.getfEstrella() + " | g = " + primerNodo.g + " | h = " + primerNodo.h);

                // si el primer nodo es goal, returns current node
                if (primerNodo.getInfo().RowId == goals[0].RowId && primerNodo.getInfo().ColumnId == goals[0].ColumnId)
                {
                    Debug.Log("Encontradoooooo");
                    return primerNodo;
                }
                else
                {
                    // expande vecinos del primer nodo de la lista aierta(calcula coste de cada uno, etc)y los añade en la lista
                    //Vaciar hijos de nodos expandidos anteriormente
                    
                    //Averiguar los hijos del nodo a expandir
                    CellInfo[] mishijos = primerNodo.getInfo().WalkableNeighbours(board);

                    //Se aumenta la g en 1 (Ya que el coste entre dos casillas siempre es 1)
                    g++;

                    //Meter hijos en la lista abierta open
                    for (int i = 0; i < mishijos.Length; i++)
                    {
                        if (mishijos[i] == null)
                        {
                            Debug.Log("dddddd");
                        }
                        else
                        {
                            Debug.Log("a");
                            //Calcular la heuristica del nodo hijo i
                            h = CalculateHeuristic(goals[0], mishijos[i]);

                            //Creamos el nodo hijo i y lo añadimos a la lista abierta
                            Nodo hijoNodo = new Nodo(primerNodo, g + h, mishijos[i], g, h);
                            open.Add(hijoNodo);
                        }
                    }

                    // ordena lista
                    open.Sort(delegate (Nodo a, Nodo b)
                    {
                        return a.getfEstrella().CompareTo(b.getfEstrella());
                    });
                }
                k++;
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
}
