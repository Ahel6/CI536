package ci536.singleplayergame.entity;

import ci536.singleplayergame.game.Level;

import java.util.HashMap;
import java.util.Map;

public abstract class Entity {
    private final Map<EntityAttribute<?>, EntityAttribute.AttributeValue<?>> attributes = new HashMap<>();
    private final EntityType<?> type;
    private Level level;
    protected float xPos, yPos;

    public Entity(EntityType<?> type, float xPos, float yPos) {
        this.type = type;
        this.xPos = xPos;
        this.yPos = yPos;
    }

    public Entity(EntityType<?> type) {
        this(type, 0, 0);
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

    public float getXPos() {
        return xPos;
    }

    public float getYPos() {
        return yPos;
    }

    public void setXPos(float xPos) {
        this.xPos = xPos;
    }

    public void setYPos(float yPos) {
        this.yPos = yPos;
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
        this.attributes.put(attribute, new EntityAttribute.AttributeValue<T>(attribute, value));
    }

    public abstract void update();
}
