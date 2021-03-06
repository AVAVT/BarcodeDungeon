//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity enemyModelEntity { get { return GetGroup(GameMatcher.EnemyModel).GetSingleEntity(); } }
    public EnemyModelComponent enemyModel { get { return enemyModelEntity.enemyModel; } }
    public bool hasEnemyModel { get { return enemyModelEntity != null; } }

    public GameEntity SetEnemyModel(EnemyModel newValue) {
        if (hasEnemyModel) {
            throw new Entitas.EntitasException("Could not set EnemyModel!\n" + this + " already has an entity with EnemyModelComponent!",
                "You should check if the context already has a enemyModelEntity before setting it or use context.ReplaceEnemyModel().");
        }
        var entity = CreateEntity();
        entity.AddEnemyModel(newValue);
        return entity;
    }

    public void ReplaceEnemyModel(EnemyModel newValue) {
        var entity = enemyModelEntity;
        if (entity == null) {
            entity = SetEnemyModel(newValue);
        } else {
            entity.ReplaceEnemyModel(newValue);
        }
    }

    public void RemoveEnemyModel() {
        enemyModelEntity.Destroy();
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

    public EnemyModelComponent enemyModel { get { return (EnemyModelComponent)GetComponent(GameComponentsLookup.EnemyModel); } }
    public bool hasEnemyModel { get { return HasComponent(GameComponentsLookup.EnemyModel); } }

    public void AddEnemyModel(EnemyModel newValue) {
        var index = GameComponentsLookup.EnemyModel;
        var component = CreateComponent<EnemyModelComponent>(index);
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceEnemyModel(EnemyModel newValue) {
        var index = GameComponentsLookup.EnemyModel;
        var component = CreateComponent<EnemyModelComponent>(index);
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveEnemyModel() {
        RemoveComponent(GameComponentsLookup.EnemyModel);
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

    static Entitas.IMatcher<GameEntity> _matcherEnemyModel;

    public static Entitas.IMatcher<GameEntity> EnemyModel {
        get {
            if (_matcherEnemyModel == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.EnemyModel);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherEnemyModel = matcher;
            }

            return _matcherEnemyModel;
        }
    }
}
