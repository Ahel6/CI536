package ci536.singleplayergame.game;

import ci536.singleplayergame.entity.Player;
import ci536.singleplayergame.math.AABB;
import javafx.beans.property.IntegerProperty;
import javafx.beans.property.SimpleIntegerProperty;
import javafx.scene.Scene;

public class Camera {
    private final IntegerProperty x = new SimpleIntegerProperty(0);
    private final IntegerProperty y = new SimpleIntegerProperty(0);
    private final IntegerProperty width = new SimpleIntegerProperty(800);
    private final IntegerProperty height = new SimpleIntegerProperty(600);

    public Camera(Scene scene, Player player) {
        this.width.bind(scene.widthProperty());
        this.height.bind(scene.heightProperty());
        this.x.bind(player.getBoundingBox().xProperty().map(x -> x.doubleValue() - width.get() / 2 + player.getBoundingBox().getWidth() / 2));
        this.y.bind(player.getBoundingBox().yProperty().map(y -> y.doubleValue() - height.get() / 2 + player.getBoundingBox().getHeight() / 2));
    }

    public IntegerProperty xProperty() {
        return x;
    }

    public IntegerProperty yProperty() {
        return y;
    }

    public IntegerProperty widthProperty() {
        return width;
    }

    public IntegerProperty heightProperty() {
        return height;
    }

    public int getX() {
        return x.get();
    }

    public void setX(int x) {
        this.x.set(x);
    }

    public int getY() {
        return y.get();
    }

    public void setY(int y) {
        this.y.set(y);
    }

    public int getWidth() {
        return width.get();
    }

    public int getHeight() {
        return height.get();
    }

    public boolean isOnScreen(double x, double y, double width, double height) {
        return x + width > this.x.get() && x < this.x.get() + this.width.get() &&
                y + height > this.y.get() && y < this.y.get() + this.height.get();
    }

    public boolean isOnScreen(GameObject obj) {
        AABB bounds = obj.getBoundingBox();
        return isOnScreen(bounds.getX(), bounds.getY(), bounds.getWidth(), bounds.getHeight());
    }
}
