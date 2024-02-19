package ci536.singleplayergame.game;

import ci536.singleplayergame.Input;
import ci536.singleplayergame.entity.Player;
import ci536.singleplayergame.jfx.GamePane;
import javafx.application.Platform;
import javafx.scene.Scene;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.input.KeyCode;
import javafx.scene.paint.Color;

public class GameLoopThread extends Thread {
    private final Scene scene;
    private final GamePane gamePane;
    private static final long NANOS_PER_FRAME = 1_000_000_000 / 60;
    private static final boolean CAP_FPS = true; // Set to 'false' to disable frame rate capping (not recommended because of how JavaFX handles rendering)
    private long lastFrameTime = 0;
    private static int FRAMES = 0;

    public GameLoopThread(Scene scene, GamePane gamePane) {
        this.scene = scene;
        this.gamePane = gamePane;
    }

    @Override
    public void run() {
        Input input = Input.INSTANCE;
        input.init(this.scene);

        var player = new Player(
                this.scene.widthProperty().map(Number::intValue),
                this.scene.heightProperty().map(Number::intValue),
                400, 300);

        var camera = new Camera(this.scene, player);
        var level = new Level(player);

        input.addKeyPressedHandler(event -> {
            if (event.getCode() == KeyCode.ESCAPE) {
                if(!level.isPaused()) {
                    player.pauseGame();
                }
            }
        });

        this.scene.getWindow().focusedProperty().addListener((observable, oldValue, newValue) -> {
            if (!newValue) {
                if(!level.isPaused()) {
                    player.pauseGame();
                }
            }
        });

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
            if (CAP_FPS) {
                unprocessedTime += elapsedTime;
            }

            if (CAP_FPS) {
                while (unprocessedTime >= NANOS_PER_FRAME) {
                    unprocessedTime -= NANOS_PER_FRAME;

                    if(!level.isPaused()) {
                        player.takeInput(input.getHorizontalInput(), input.getVerticalInput());
                    }

                    level.update();
                }
            } else {
                if(!level.isPaused()) {
                    player.takeInput(input.getHorizontalInput(), input.getVerticalInput());
                }

                level.update();
            }

            Platform.runLater(() -> {
                gc.setFill(Color.BLACK);
                gc.fillRect(0, 0, canvas.getWidth(), canvas.getHeight());
            });

            Platform.runLater(() -> level.render(canvas, camera));

            // Calculate FPS
            long now = System.nanoTime();
            if (now - this.lastFrameTime >= NANOS_PER_FRAME) {
                FRAMES = 1000 / (int) ((now - this.lastFrameTime) / 1_000_000); // Calculate FPS
                this.lastFrameTime = now;
            }

            if (CAP_FPS) {
                // Ensure smooth animation by sleeping for remaining time if there is any
                long sleepTime = NANOS_PER_FRAME - unprocessedTime;
                if (sleepTime > 0) {
                    try {
                        sleep(sleepTime / 1_000_000, (int) (sleepTime % 1_000_000));
                    } catch (InterruptedException exception) {
                        throw new RuntimeException("Game loop thread was interrupted", exception);
                    }
                }
            }
        }
    }

    public static int getFPS() {
        return FRAMES;
    }
}
