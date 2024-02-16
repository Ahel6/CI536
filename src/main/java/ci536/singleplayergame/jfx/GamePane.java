package ci536.singleplayergame.jfx;

import ci536.singleplayergame.game.GameLoopThread;
import ci536.singleplayergame.util.ShutdownHooks;
import javafx.scene.Scene;
import javafx.scene.canvas.Canvas;
import javafx.scene.layout.BorderPane;

public class GamePane extends BorderPane {
    private final Canvas canvas;

    public GamePane() {
        canvas = new Canvas(800, 600);

        setCenter(canvas);
    }

    public void startGameLoop(Scene scene) {
        var thread = new GameLoopThread(scene, this);
        thread.start();
        ShutdownHooks.addShutdownHook(thread::interrupt);
    }

    public Canvas getCanvas() {
        return canvas;
    }
}
