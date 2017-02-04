namespace Enemy
{
    public class FleeingState : FSMState
    {
        private double timer;
        private const float FLEE_SPEED = 50.0f;
        private const float HEAL_SPEED = 30.0f;

        protected override void exit()
        {

        }

        protected override void init()
        {
            parent.Speed = FLEE_SPEED;
            parent.FinalTargetWaypoint = parent.CurrentWaypoint.GetFurthestNeighbour();
        }

        protected override void update()
        {
            if (parent.FinalTargetWaypoint == null)
            {
                // If timer is more than 3 seconds, enemy will heal itself
                timer += TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
                if (timer >= 1)
                {
                    healenemy();
                    timer = 0;
                }                
            }

            if (parent.Health == parent.MaxHealth)
            {
                parent.changeCurrentState(new PatrolState());
                return;
            }
        }

        private void healenemy()
        {
            // Will heal the enemy for a certain amount
            parent.Heal((int)(HEAL_SPEED * TimeManager.GetDeltaTime(TimeManager.TimeType.Game)));
        }
    }
}
