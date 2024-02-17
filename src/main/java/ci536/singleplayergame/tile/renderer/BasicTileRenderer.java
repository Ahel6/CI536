package ci536.singleplayergame.tile.renderer;

import ci536.singleplayergame.game.Camera;
import ci536.singleplayergame.math.AABB;
import ci536.singleplayergame.tile.Tile;
import javafx.scene.canvas.Canvas;
import javafx.scene.image.Image;

public class BasicTileRenderer<T extends Tile> extends TileRenderer<T> {
    private final Image texture;

    public BasicTileRenderer(String texturePath) {
        this.texture = loadImageFromPath(texturePath);
    }

    protected static Image loadImageFromPath(String path) {
        return new Image("file:src/main/resources/" + path);
    }

    @Override
    public void render(T tile, Canvas canvas, Camera camera) {
        var gc = canvas.getGraphicsContext2D();

        AABB boundingBox = tile.getBoundingBox();
        double textureWidth = texture.getWidth();
        double textureHeight = texture.getHeight();

        gc.drawImage(texture, 0, 0, textureWidth, textureHeight, boundingBox.getX() - camera.getX(), boundingBox.getY() - camera.getY(), boundingBox.getWidth(), boundingBox.getHeight());
    }
}
