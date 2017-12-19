//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity turnEntity { get { return GetGroup(GameMatcher.Turn).GetSingleEntity(); } }
    public TurnComponent turn { get { return turnEntity.turn; } }
    public bool hasTurn { get { return turnEntity != null; } }

    public GameEntity SetTurn(TurnType newType) {
        if (hasTurn) {
            throw new Entitas.EntitasException("Could not set Turn!\n" + this + " already has an entity with TurnComponent!",
                "You should check if the context already has a turnEntity before setting it or use context.ReplaceTurn().");
        }
        var entity = CreateEntity();
        entity.AddTurn(newType);
        return entity;
    }

    public void ReplaceTurn(TurnType newType) {
        var entity = turnEntity;
        if (entity == null) {
            entity = SetTurn(newType);
        } else {
            entity.ReplaceTurn(newType);
        }
    }

    public void RemoveTurn() {
        turnEntity.Destroy();
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public TurnComponent turn { get { return (TurnComponent)GetComponent(GameComponentsLookup.Turn); } }
    public bool hasTurn { get { return HasComponent(GameComponentsLookup.Turn); } }

    public void AddTurn(TurnType newType) {
        var index = GameComponentsLookup.Turn;
        var component = CreateComponent<TurnComponent>(index);
        component.type = newType;
        AddComponent(index, component);
    }

    public void ReplaceTurn(TurnType newType) {
        var index = GameComponentsLookup.Turn;
        var component = CreateComponent<TurnComponent>(index);
        component.type = newType;
        ReplaceComponent(index, component);
    }

    public void RemoveTurn() {
        RemoveComponent(GameComponentsLookup.Turn);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherTurn;

    public static Entitas.IMatcher<GameEntity> Turn {
        get {
            if (_matcherTurn == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Turn);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTurn = matcher;
            }

            return _matcherTurn;
        }
    }
}
