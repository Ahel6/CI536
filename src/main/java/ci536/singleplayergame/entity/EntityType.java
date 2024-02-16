package ci536.singleplayergame.entity;

import java.util.Objects;

// TODO: This could also be used for things such as spawn rules
//  or anything else that should be specific to an entity type instead of an entity
public class EntityType<T extends Entity> {
    public static final EntityType<Player> PLAYER = new EntityType<>(Player.class, "player");

    private final Class<? extends T> entityClass;
    private final String name;

    public EntityType(Class<? extends T> entityClass, String name) {
        this.entityClass = entityClass;
        this.name = name;
    }

    public String getName() {
        return name;
    }

    public Class<? extends T> getEntityClass() {
        return entityClass;
    }

    @Override
    public String toString() {
        return "EntityType{" +
                "entityClass=" + entityClass +
                ", name='" + name + "'" +
                "}";
    }

    @Override
    public boolean equals(Object obj) {
        if (obj == this)
            return true;

        if (obj == null || obj.getClass() != this.getClass())
            return false;

        EntityType<?> other = (EntityType<?>) obj;
        return this.entityClass.equals(other.entityClass) && this.name.equals(other.name);
    }

    @Override
    public int hashCode() {
        return Objects.hash(entityClass, name);
    }
}
