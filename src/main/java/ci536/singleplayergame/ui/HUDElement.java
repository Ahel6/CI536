package ci536.singleplayergame.ui;

import javafx.scene.canvas.Canvas;

@FunctionalInterface
public interface HUDElement {
    void draw(Canvas canvas);
}
