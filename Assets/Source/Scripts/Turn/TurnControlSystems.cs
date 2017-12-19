using Entitas;

public class TurnControlSystems : Feature
{
	public TurnControlSystems(Contexts contexts) : base("TurnControl Systems")
	{
		Add (new ChangeToEnemyTurnSystem (contexts));
		Add (new ChangeToPlayerTurnSystem (contexts));
		Add (new StartTurnSystem (contexts));
	}
}