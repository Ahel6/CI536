package ci536.singleplayergame.math;

import javafx.beans.property.DoubleProperty;
import javafx.beans.property.SimpleDoubleProperty;

public sealed class AABB permits AABB.Mutable {
    final DoubleProperty x = new SimpleDoubleProperty();
    final DoubleProperty y = new SimpleDoubleProperty();
    final DoubleProperty width = new SimpleDoubleProperty();
    final DoubleProperty height = new SimpleDoubleProperty();

    public AABB(double x, double y, double width, double height) {
        this.x.set(x);
        this.y.set(y);
        this.width.set(width);
        this.height.set(height);
    }

    public DoubleProperty xProperty() {
        return x;
    }

    public DoubleProperty yProperty() {
        return y;
    }

    public DoubleProperty widthProperty() {
        return width;
    }

    public DoubleProperty heightProperty() {
        return height;
    }

    public boolean intersects(AABB other) {
        return (getX() < other.getX() + other.getWidth() && getX() + getWidth() > other.getX() && getY() < other.getY() + other.getHeight() && getY() + getHeight() > other.getY());
    }

    public boolean contains(AABB other) {
        return (getX() <= other.getX() && getX() + getWidth() >= other.getX() + other.getWidth() && getY() <= other.getY() && getY() + getHeight() >= other.getY() + other.getHeight());
    }

    public AABB inflate(double x, double y) {
        return new AABB(getX() - x, getY() - y, getWidth() + x * 2, getHeight() + y * 2);
    }

    public AABB inflate(double magnitude) {
        return inflate(magnitude, magnitude);
    }

    public AABB offset(double x, double y) {
        return new AABB(getX() + x, getY() + y, getWidth(), getHeight());
    }

    public AABB copy() {
        return new AABB(getX(), getY(), getWidth(), getHeight());
    }

    public double getX() {
        return x.get();
    }

    public double getY() {
        return y.get();
    }

    public double getWidth() {
        return width.get();
    }

    public double getHeight() {
        return height.get();
    }

    public Vector2f getCenter() {
        return new Vector2f((float) (getX() + getWidth() / 2), (float) (getY() + getHeight() / 2));
    }

    public static final class Mutable extends AABB {
        public Mutable(double x, double y, double width, double height) {
            super(x, y, width, height);
        }

        public void setX(double x) {
            this.x.set(x);
        }

        public void setY(double y) {
            this.y.set(y);
        }

        public void setWidth(double width) {
            this.width.set(width);
        }

        public void setHeight(double height) {
            this.height.set(height);
        }

        public void setPosition(double x, double y) {
            setX(x);
            setY(y);
        }

        public void setSize(double width, double height) {
            setWidth(width);
            setHeight(height);
        }

        public void set(double x, double y, double width, double height) {
            setPosition(x, y);
            setSize(width, height);
        }
    }
}
