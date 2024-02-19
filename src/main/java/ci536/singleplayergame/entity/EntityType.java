package ci536.singleplayergame.entity;

import ci536.singleplayergame.math.Vector2f;

// TODO: This could also be used for things such as spawn rules
//  or anything else that should be specific to an entity type instead of an entity
public class EntityType<T extends Entity> {
    public static final EntityType<Player> PLAYER = new EntityType<>(type -> null, "player", new Vector2f(100, 100));

    private final EntityFactory<T> factory;
    private final String name;
    private final Vector2f size;

    public EntityType(EntityFactory<T> factory, String name, Vector2f size) {
        this.factory = factory;
        this.name = name;
        this.size = size;
    }

    public Vector2f getSize() {
        return size;
    }

    public T create() {
        return factory.create(this);
    }

    public String getName() {
        return name;
    }

    @Override
    public String toString() {
        return "EntityType{" + "name='" + this.name + "'" + ", size=" + size + "}";
    }

    @FunctionalInterface
    public interface EntityFactory<T extends Entity> {
        T create(EntityType<T> type);
    }
}
