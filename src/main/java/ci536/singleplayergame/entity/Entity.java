package ci536.singleplayergame.entity;

import ci536.singleplayergame.game.GameObject;
import ci536.singleplayergame.game.Level;
import ci536.singleplayergame.math.AABB;
import ci536.singleplayergame.math.Vector2f;
import ci536.singleplayergame.tile.Updatable;

import java.util.HashMap;
import java.util.Map;

public abstract class Entity extends GameObject implements Updatable {
    private final Map<EntityAttribute<?>, EntityAttribute.AttributeValue<?>> attributes = new HashMap<>();
    private final EntityType<?> type;
    protected final AABB.Mutable boundingBox;
    private Level level;

    public Entity(EntityType<?> type, float xPos, float yPos) {
        this.type = type;
        this.boundingBox = new AABB.Mutable(xPos, yPos, type.getSize().getX(), type.getSize().getY());
    }

    public Entity(EntityType<?> type) {
        this(type, 0, 0);
    }

    @Override
    public AABB getBoundingBox() {
        return this.boundingBox;
    }

    public EntityType<?> getType() {
        return type;
    }

    public Level getLevel() {
        return level;
    }

    public void setLevel(Level level) {
        this.level = level;
    }

    public double getXPos() {
        return this.boundingBox.getX();
    }

    public double getYPos() {
        return this.boundingBox.getY();
    }

    public Vector2f getSize() {
        return this.type.getSize();
    }

    public boolean isDead() {
        return getAttributeValue(EntityAttribute.HEALTH, Float.class) <= 0f;
    }

    public EntityAttribute.AttributeValue<?> getAttribute(EntityAttribute<?> attribute) {
        return this.attributes.get(attribute);
    }

    public <T extends Number> T getAttributeValue(EntityAttribute<T> attribute, Class<T> type) {
        EntityAttribute.AttributeValue<?> value = this.attributes.get(attribute);
        if (value == null)
            return attribute.getBaseValue();
        return value.unsafeCastTo(type);
    }

    public <T extends Number> void setAttributeValue(EntityAttribute<T> attribute, T value) {
        this.attributes.put(attribute, new EntityAttribute.AttributeValue<>(attribute, value));
    }

    public void setPosition(double x, double y) {
        this.boundingBox.setX(x);
        this.boundingBox.setY(y);
    }
}
