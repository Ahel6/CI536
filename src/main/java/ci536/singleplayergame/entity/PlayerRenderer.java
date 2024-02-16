package ci536.singleplayergame.entity;

import javafx.scene.canvas.Canvas;
import javafx.scene.paint.Color;

public class PlayerRenderer extends EntityRenderer<Player> {
    @Override
    public void render(Player entity, Canvas canvas) {
        var gc = canvas.getGraphicsContext2D();
        gc.setFill(Color.ALICEBLUE);
        gc.fillOval(entity.getXPos() - 25, entity.getYPos() - 25, 50, 50);
    }
}
