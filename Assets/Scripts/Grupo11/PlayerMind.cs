using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts.SampleMind
{
    public enum SearchType
    {
        OnlineSearch = 0,
        OfflineSearch = 1
    }

    public class PlayerMind : AbstractPathMind
    {
        private Stack<Locomotion.MoveDirection> currentPlan = new Stack<Locomotion.MoveDirection>();
        private OfflineSearch offlineSearch = null;
        private OnlineSearch onlineSearch = null;
        [SerializeField] private SearchType type = SearchType.OfflineSearch;

        private void Awake()
        {
            offlineSearch = GetComponent<OfflineSearch>();
            onlineSearch = GetComponent<OnlineSearch>();
        }

        public override void Repath()
        {
            currentPlan.Clear();
        }

        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            // si la Stack no está vacía, hacer siguiente movimiento
            if (currentPlan.Count != 0)
            {
                return currentPlan.Pop();
            }

            Nodo searchResult = null;
            if (type.Equals(SearchType.OfflineSearch))
            {
                // calcular camino, devuelve resultado de A*
                searchResult = offlineSearch.Search(boardInfo, currentPos, goals);
            }
            else if (type.Equals(SearchType.OnlineSearch))
            {
                // calcular camino, devuelve resultado de búsqueda en línea
                searchResult = onlineSearch.SearchOnline(boardInfo, currentPos, goals);
            }

            // recorre searchResult and copia el camino a currentPlan
            while (searchResult.getNodoPadre() != null)
            {
                currentPlan.Push(CalculateMovement(searchResult.getInfo(), searchResult.getNodoPadre().getInfo()));
                searchResult = searchResult.getNodoPadre();
            }

            // returns next move (pop Stack)
            if (currentPlan.Count != 0)
            {
                return currentPlan.Pop();
            }

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
    }
}