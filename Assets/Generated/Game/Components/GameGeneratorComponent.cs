//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity generatorEntity { get { return GetGroup(GameMatcher.Generator).GetSingleEntity(); } }
    public GeneratorComponent generator { get { return generatorEntity.generator; } }
    public bool hasGenerator { get { return generatorEntity != null; } }

    public GameEntity SetGenerator(int newSeed) {
        if (hasGenerator) {
            throw new Entitas.EntitasException("Could not set Generator!\n" + this + " already has an entity with GeneratorComponent!",
                "You should check if the context already has a generatorEntity before setting it or use context.ReplaceGenerator().");
        }
        var entity = CreateEntity();
        entity.AddGenerator(newSeed);
        return entity;
    }

    public void ReplaceGenerator(int newSeed) {
        var entity = generatorEntity;
        if (entity == null) {
            entity = SetGenerator(newSeed);
        } else {
            entity.ReplaceGenerator(newSeed);
        }
    }

    public void RemoveGenerator() {
        generatorEntity.Destroy();
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

    public GeneratorComponent generator { get { return (GeneratorComponent)GetComponent(GameComponentsLookup.Generator); } }
    public bool hasGenerator { get { return HasComponent(GameComponentsLookup.Generator); } }

    public void AddGenerator(int newSeed) {
        var index = GameComponentsLookup.Generator;
        var component = CreateComponent<GeneratorComponent>(index);
        component.seed = newSeed;
        AddComponent(index, component);
    }

    public void ReplaceGenerator(int newSeed) {
        var index = GameComponentsLookup.Generator;
        var component = CreateComponent<GeneratorComponent>(index);
        component.seed = newSeed;
        ReplaceComponent(index, component);
    }

    public void RemoveGenerator() {
        RemoveComponent(GameComponentsLookup.Generator);
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

    static Entitas.IMatcher<GameEntity> _matcherGenerator;

    public static Entitas.IMatcher<GameEntity> Generator {
        get {
            if (_matcherGenerator == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Generator);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherGenerator = matcher;
            }

            return _matcherGenerator;
        }
    }
}