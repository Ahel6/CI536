package ci536.singleplayergame.entity.renderer;

import ci536.singleplayergame.entity.Entity;
import ci536.singleplayergame.game.Camera;
import ci536.singleplayergame.math.AABB;
import javafx.scene.canvas.Canvas;
import javafx.scene.image.Image;

public abstract class TexturedEntityRenderer<T extends Entity> extends EntityRenderer<T> {
    private Image texture;

    protected static Image loadImageFromPath(String path) {
        return new Image("file:src/main/resources/" + path);
    }

    public abstract String getTexturePath(T entity);

    @Override
    public void render(T entity, Canvas canvas, Camera camera) {
        var gc = canvas.getGraphicsContext2D();
        Image texture = getTexture(entity);

        AABB boundingBox = entity.getBoundingBox();
        double textureWidth = texture.getWidth();
        double textureHeight = texture.getHeight();

        gc.drawImage(texture, 0, 0, textureWidth, textureHeight, boundingBox.getX() - camera.getX(), boundingBox.getY() - camera.getY(), boundingBox.getWidth(), boundingBox.getHeight());
    }

    public Image getTexture(T entity) {
        if (this.texture == null) {
            this.texture = loadImageFromPath(getTexturePath(entity));
        }

        return this.texture;
    }
}
