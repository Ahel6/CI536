package ci536.singleplayergame.entity;

import javafx.scene.canvas.Canvas;

public abstract class EntityRenderer<T extends Entity> {
    public abstract void render(T entity, Canvas canvas);
}
