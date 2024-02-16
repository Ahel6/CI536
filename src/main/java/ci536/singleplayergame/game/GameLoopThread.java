package ci536.singleplayergame.game;

import ci536.singleplayergame.Input;
import ci536.singleplayergame.entity.Player;
import ci536.singleplayergame.jfx.GamePane;
import javafx.scene.Scene;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.paint.Color;

public class GameLoopThread extends Thread {
    private final Scene scene;
    private final GamePane gamePane;

    public GameLoopThread(Scene scene, GamePane gamePane) {
        this.scene = scene;
        this.gamePane = gamePane;
    }

    @Override
    public void run() {
        super.run();

        var input = new Input();
        input.init(this.scene);

        var player = new Player();
        var level = new Level(player);

        Canvas canvas = gamePane.getCanvas();
        GraphicsContext gc = canvas.getGraphicsContext2D();
        while (true) {
            player.takeInput(input.getHorizontalInput(), input.getVerticalInput());

            gc.setFill(Color.BLACK);
            gc.fillRect(0, 0, 800, 600);

            level.update();
            level.render(canvas);

            // Run at 60 fps
            try {
                Thread.sleep(1000 / 60);
            } catch (InterruptedException exception) {
                throw new RuntimeException("Game loop thread was interrupted", exception);
            }
        }
    }
}
