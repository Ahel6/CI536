package ci536.singleplayergame;

import ci536.singleplayergame.jfx.GamePane;
import javafx.application.Application;
import javafx.scene.Scene;
import javafx.stage.Stage;

public class SinglePlayerGame extends Application {
    @Override
    public void start(Stage primaryStage) {
        var gamePane = new GamePane();
        var scene = new Scene(gamePane, 800, 600);
        primaryStage.setTitle("Single Player Game");
        primaryStage.setScene(scene);
        primaryStage.show();

        gamePane.startGameLoop(scene);

        primaryStage.setOnCloseRequest(event -> System.exit(0));
    }
}
