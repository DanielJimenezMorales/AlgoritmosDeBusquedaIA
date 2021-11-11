using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.DataStructures;

public class OnlineSearch : MonoBehaviour
{
    private EnemyBehaviour[] enemies = null;

    //Este método recopila todos los enemigos existentes en el tablero al principio del juego
    private void Start()
    {
        enemies = GameObject.FindObjectsOfType<EnemyBehaviour>();
    }

    public Nodo SearchOnline(BoardInfo board, CellInfo start, CellInfo[] goals)
    {
        bool isEnemyArrayEmpty = isEnemyListEmpty();                  //Esta variable nos indica si existe algún enemigo vivo
        EnemyBehaviour currentEnemy = null;                           //Esta variable guardará el CellInfo del enemigo más cercano
                                                                      //(En caso de haber alguno vivo)

        //Comprobamos si quedan enemigos vivos
        if (!isEnemyArrayEmpty)
        {
            currentEnemy = GetClosestEnemy(start);
        }

        List<Nodo> open = new List<Nodo>();                             //Lista abierta

        int h = 0;                                                      //h(n)
        Nodo nodoStart = new Nodo(null, h, start);                      //Nodo inicial

        // Se añade nodo inicial a la lista abierta.
        open.Add(nodoStart);

        // Sacar el primer nodo de la lista
        Nodo primerNodo = open[0];
        open.RemoveAt(0);

        //Si quedan enemigos en el tablero, el nodo meta será el enemigo más cercano. En caso contrario el nodo meta será goal
        if (isEnemyArrayEmpty)
        {
            // Si el primer nodo es goal, devolverlo
            if (primerNodo.getInfo().RowId == goals[0].RowId && primerNodo.getInfo().ColumnId == goals[0].ColumnId)
            {
                return primerNodo;
            }
        }
        else
        {
            // Si el primer nodo es un enemigo, devolverlo
            if (primerNodo.getInfo().RowId == currentEnemy.CurrentPosition().RowId && primerNodo.getInfo().ColumnId == currentEnemy.CurrentPosition().ColumnId)
            {
                return primerNodo;
            }
        }

        //Averiguar los hijos del nodo a expandir
        CellInfo[] hijos = primerNodo.getInfo().WalkableNeighbours(board);

        //Meter hijos en la lista abierta open
        for (int i = 0; i < hijos.Length; i++)
        {
            if (hijos[i] != null)
            {
                //Si no quedan enemigos vivos en el tablero calculamos la heurística desde
                //el hijo hasta goal. Sino hasta el enemigo más cercano
                if (isEnemyArrayEmpty)
                {
                    //Calcular la heuristica del nodo hijo i
                    h = CalculateHeuristic(goals[0], hijos[i]);
                }
                else
                {
                    //Calcular la heuristica del nodo hijo i
                    h = CalculateHeuristic(currentEnemy.CurrentPosition(), hijos[i]);
                }

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
        return open[0];
    }

    //Este método devuelve el enemigo más cercano (En caso de haberlo)
    private EnemyBehaviour GetClosestEnemy(CellInfo playerPos)
    {
        int enemyIndex = -1;
        int currentDistance = 1000;

        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null)
            {
                int newDistance = CalculateHeuristic(enemies[i].CurrentPosition(), playerPos);

                if(newDistance < currentDistance)
                {
                    currentDistance = newDistance;
                    enemyIndex = i;
                }
            }
        }

        return enemies[enemyIndex];
    }

    //Este método, calcula si existe algún enemigo con vida en el tablero
    private bool isEnemyListEmpty()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null)
            {
                return false;
            }
        }

        return true;
    }

    //Este método calcula la heuristica de la distancia de Manhattam desde un nodo currentNode hasta un nodo goal
    private int CalculateHeuristic(CellInfo goal, CellInfo currentNode)
    {
        Vector2 goalPosition = new Vector2(goal.ColumnId, goal.RowId);
        Vector2 currentNodePosition = new Vector2(currentNode.ColumnId, currentNode.RowId);
        return (int)Mathf.Abs(goalPosition.x - currentNodePosition.x) + (int)Mathf.Abs(goalPosition.y - currentNodePosition.y);
    }
}