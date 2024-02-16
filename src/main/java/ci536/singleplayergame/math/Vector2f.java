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
