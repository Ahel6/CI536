package ci536.singleplayergame.ui;

import ci536.singleplayergame.entity.Player;
import javafx.beans.property.IntegerProperty;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.input.KeyCode;
import javafx.scene.paint.Color;

public class PauseGui extends Gui {
    private final Player player;

    public PauseGui(Player player) {
        this.player = player;
    }

    @Override
    public void render(Canvas canvas, int mouseX, int mouseY) {
        GraphicsContext gc = canvas.getGraphicsContext2D();
        gc.setFill(Color.color(0.0, 0.0, 0.0, 0.5));
        gc.fillRect(0, 0, getWidth(), getHeight());

        gc.setFill(Color.WHITE);
        gc.fillText("Paused", getWidth() / 2f, getHeight() / 2f);

        super.render(canvas, mouseX, mouseY);
    }

    @Override
    public void onKeyPress(KeyCode keyCode) {
        super.onKeyPress(keyCode);

        if (keyCode == KeyCode.ESCAPE) {
            this.player.popGui();
        }
    }

    @Override
    public int getWidth() {
        return this.screenWidth.get();
    }

    @Override
    public int getHeight() {
        return this.screenHeight.get();
    }

    @Override
    public boolean shouldPauseGame() {
        return true;
    }
}
