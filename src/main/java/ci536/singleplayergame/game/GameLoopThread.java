package ci536.singleplayergame.game;

import ci536.singleplayergame.Input;
import ci536.singleplayergame.entity.Player;
import ci536.singleplayergame.jfx.GamePane;
import javafx.scene.Scene;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.paint.Color;

// TODO: Figure out why the loop sometimes completely locks up
public class GameLoopThread extends Thread {
    private final Scene scene;
    private final GamePane gamePane;
    private static final long NANOS_PER_FRAME = 1_000_000_000 / 60;
    private static final boolean CAP_FPS = true; // Set to false to disable frame rate capping (not recommended because of how JavaFX handles rendering)
    private long lastFrameTime = 0;
    private int frames = 0;

    public GameLoopThread(Scene scene, GamePane gamePane) {
        this.scene = scene;
        this.gamePane = gamePane;
    }

    @Override
    public void run() {
        Input input = Input.INSTANCE;
        input.init(this.scene);

        var player = new Player(400, 300);

        var camera = new Camera(this.scene, player);
        var level = new Level(player);

        Canvas canvas = gamePane.getCanvas();
        GraphicsContext gc = canvas.getGraphicsContext2D();

        long lastTime = System.nanoTime();
        long currentTime;
        long elapsedTime;
        long unprocessedTime = 0;
        while (true) {
            currentTime = System.nanoTime();
            elapsedTime = currentTime - lastTime;
            lastTime = currentTime;
            if(CAP_FPS) {
                unprocessedTime += elapsedTime;
            }

            if(CAP_FPS) {
                while (unprocessedTime >= NANOS_PER_FRAME) {
                    unprocessedTime -= NANOS_PER_FRAME;

                    player.takeInput(input.getHorizontalInput(), input.getVerticalInput());
                    level.update();
                }
            } else {
                player.takeInput(input.getHorizontalInput(), input.getVerticalInput());
                level.update();
            }

            gc.setFill(Color.BLACK);
            gc.fillRect(0, 0, canvas.getWidth(), canvas.getHeight());

            level.render(canvas, camera);

            // Calculate FPS
            long now = System.nanoTime();
            if (now - this.lastFrameTime >= NANOS_PER_FRAME) {
                this.frames = 1000 / (int)((now - this.lastFrameTime) / 1_000_000); // Calculate FPS
                this.lastFrameTime = now;
            }

            // Display FPS (you may change this according to your needs)
            gc.setFill(Color.WHITE);
            gc.fillText("FPS: " + this.frames, 10, 20);

            if(CAP_FPS) {
                // Ensure smooth animation by sleeping for remaining time if there is any
                long sleepTime = NANOS_PER_FRAME - unprocessedTime;
                if (sleepTime > 0) {
                    try {
                        sleep(sleepTime / 1000000, (int) (sleepTime % 1000000));
                    } catch (InterruptedException exception) {
                        throw new RuntimeException("Game loop thread was interrupted", exception);
                    }
                }
            }
        }
    }
}
