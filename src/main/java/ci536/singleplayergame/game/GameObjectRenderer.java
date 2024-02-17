package ci536.singleplayergame.game;

import javafx.scene.canvas.Canvas;

public abstract class GameObjectRenderer<T extends GameObject> {
    public abstract void render(T object, Canvas canvas, Camera camera);
}
