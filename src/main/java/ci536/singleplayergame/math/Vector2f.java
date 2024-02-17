package ci536.singleplayergame.math;

public sealed class Vector2f permits Vector2f.Mutable {
    float x;
    float y;

    public Vector2f(float x, float y) {
        this.x = x;
        this.y = y;
    }

    public float getX() {
        return x;
    }

    public float getY() {
        return y;
    }

    public float length() {
        return (float) Math.sqrt(x * x + y * y);
    }

    public float dot(Vector2f other) {
        return x * other.x + y * other.y;
    }

    public Vector2f add(Vector2f other) {
        return new Vector2f(x + other.x, y + other.y);
    }

    public Vector2f subtract(Vector2f other) {
        return new Vector2f(x - other.x, y - other.y);
    }

    public Vector2f multiply(float scalar) {
        return new Vector2f(x * scalar, y * scalar);
    }

    public Vector2f divide(float scalar) {
        return new Vector2f(x / scalar, y / scalar);
    }

    public Vector2f normalize() {
        float length = length();
        return new Vector2f(x / length, y / length);
    }

    public Vector2f rotate(float angle) {
        float cos = (float) Math.cos(angle);
        float sin = (float) Math.sin(angle);
        return new Vector2f(x * cos - y * sin, x * sin + y * cos);
    }

    public Vector2f project(Vector2f other) {
        return other.multiply(dot(other) / other.dot(other));
    }

    public Vector2f reflect(Vector2f normal) {
        return normal.multiply(2 * dot(normal)).subtract(this);
    }

    public float angle(Vector2f other) {
        return (float) Math.acos(dot(other) / (length() * other.length()));
    }

    public float distance(Vector2f other) {
        return subtract(other).length();
    }

    public float distanceSquared(Vector2f other) {
        return subtract(other).dot(subtract(other));
    }

    public Vector2f lerp(Vector2f other, float alpha) {
        return new Vector2f(x + alpha * (other.x - x), y + alpha * (other.y - y));
    }

    public Vector2f abs() {
        return new Vector2f(Math.abs(x), Math.abs(y));
    }

    public Vector2f floor() {
        return new Vector2f((float) Math.floor(x), (float) Math.floor(y));
    }

    public Vector2f ceil() {
        return new Vector2f((float) Math.ceil(x), (float) Math.ceil(y));
    }

    public Vector2f round() {
        return new Vector2f(Math.round(x), Math.round(y));
    }

    public Vector2f max(Vector2f other) {
        return new Vector2f(Math.max(x, other.x), Math.max(y, other.y));
    }

    public Vector2f min(Vector2f other) {
        return new Vector2f(Math.min(x, other.x), Math.min(y, other.y));
    }

    public Vector2f clamp(Vector2f min, Vector2f max) {
        return max(min).min(max);
    }

    public Vector2f negate() {
        return new Vector2f(-x, -y);
    }

    public static final class Mutable extends Vector2f {
        public Mutable(float x, float y) {
            super(x, y);
        }

        public void setX(float x) {
            this.x = x;
        }

        public void setY(float y) {
            this.y = y;
        }
    }
}
