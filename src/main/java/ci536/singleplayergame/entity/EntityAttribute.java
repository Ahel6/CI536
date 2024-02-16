package ci536.singleplayergame.entity;

public class EntityAttribute<T extends Number> {
    public static final EntityAttribute<Float> HEALTH = new EntityAttribute<>(10f);
    public static final EntityAttribute<Float> SPEED = new EntityAttribute<>(1f);

    private final T baseValue;

    private EntityAttribute(T baseValue) {
        this.baseValue = baseValue;
    }

    public T getBaseValue() {
        return baseValue;
    }

    public record AttributeValue<T extends Number>(EntityAttribute<T> attribute, T value) {
        public boolean isFloat() {
            return value instanceof Float;
        }

        public boolean isInteger() {
            return value instanceof Integer || value instanceof Long;
        }

        public boolean isDouble() {
            return value instanceof Double;
        }

        public boolean isLong() {
            return value instanceof Long;
        }

        public boolean isFloatOrDouble() {
            return isFloat() || isDouble();
        }

        public <C extends Number> C unsafeCastTo(Class<C> clazz) {
            return clazz.cast(this.value);
        }

        public int getAsInt() {
            return value.intValue();
        }

        public long getAsLong() {
            return value.longValue();
        }

        public float getAsFloat() {
            return value.floatValue();
        }

        public T getValue() {
            return value;
        }

        public double getAsDouble() {
            return value.doubleValue();
        }

        public String toString() {
            return value.toString();
        }
    }
}
