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
            return Locomotion.MoveDirection.Up;
        }

        
    }
}
