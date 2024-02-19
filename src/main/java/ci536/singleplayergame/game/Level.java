package ci536.singleplayergame.game;

import ci536.singleplayergame.Input;
import ci536.singleplayergame.entity.Entity;
import ci536.singleplayergame.entity.EntityType;
import ci536.singleplayergame.entity.Player;
import ci536.singleplayergame.entity.renderer.EntityRenderer;
import ci536.singleplayergame.entity.renderer.PlayerRenderer;
import ci536.singleplayergame.tile.Tile;
import ci536.singleplayergame.tile.TileType;
import ci536.singleplayergame.tile.Updatable;
import ci536.singleplayergame.tile.renderer.BasicTileRenderer;
import ci536.singleplayergame.tile.renderer.TileRenderer;
import ci536.singleplayergame.ui.DrawLayer;
import ci536.singleplayergame.ui.Gui;
import ci536.singleplayergame.ui.HUDElement;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.paint.Color;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class Level {
    private final List<Tile> tiles = new ArrayList<>();
    private final List<Entity> entities = new ArrayList<>();
    private final List<Updatable> updatables = new ArrayList<>();

    private final Map<EntityType<?>, EntityRenderer<?>> entityRenderers = new HashMap<>();
    private final Map<TileType<?>, TileRenderer<?>> tileRenderers = new HashMap<>();
    private final Map<DrawLayer, List<HUDElement>> hudElements = new HashMap<>();

    private final Player player;

    public Level(Player player) {
        this.player = player;
        this.entityRenderers.put(EntityType.PLAYER, new PlayerRenderer());
        addEntity(player);

        this.tileRenderers.put(TileType.DIRT, new BasicTileRenderer<>("textures/dirt.png"));
        for (int x = 0; x < 100; x++) {
            for (int y = 0; y < 100; y++) {
                addTile(new Tile(TileType.DIRT, x * 10, y * 10));
            }
        }

        addHUDElement((canvas) -> {
            GraphicsContext gc = canvas.getGraphicsContext2D();
            gc.setFill(Color.WHITE);
            gc.fillText("Health: " + player.getHealth(), 10, 20);
        }, DrawLayer.AFTER_ENTITY);

        addHUDElement((canvas) -> {
            GraphicsContext gc = canvas.getGraphicsContext2D();
            double y = canvas.getHeight() - 20;
            gc.setFill(Color.WHITE);
            gc.fillText("FPS: " + GameLoopThread.getFPS(), 10, y);
        }, DrawLayer.TOP);
    }

    public boolean isPaused() {
        return player.isPaused();
    }

    public Entity addEntity(Entity entity) {
        if (entity instanceof Player && this.player.getLevel() != null)
            throw new IllegalArgumentException("Player entity already exists in level");

        if (!this.entityRenderers.containsKey(entity.getType()))
            throw new IllegalArgumentException("No renderer for entity type: " + entity.getType());

        if (entity.isDead())
            throw new IllegalArgumentException("Attempted to add dead entity to level");

        if (entity.getLevel() != null && entity.getLevel() != this)
            entity.getLevel().removeEntity(entity);

        this.entities.add(entity);
        this.updatables.add(entity);
        entity.setLevel(this);

        return entity;
    }

    public void removeEntity(Entity entity) {
        if (!this.entities.remove(entity))
            throw new IllegalArgumentException("Attempted to remove entity that does not exist in level");

        if (!this.updatables.remove(entity))
            throw new IllegalArgumentException("Attempted to remove entity");

        entity.setLevel(null);
    }

    public Tile addTile(Tile tile) {
        if (!this.tileRenderers.containsKey(tile.getType()))
            throw new IllegalArgumentException("No renderer for tile type: " + tile.getType());

        this.tiles.add(tile);
        if (tile instanceof Updatable updatable)
            this.updatables.add(updatable);

        return tile;
    }

    public void removeTile(Tile tile) {
        if (!this.tiles.remove(tile))
            throw new IllegalArgumentException("Attempted to remove tile that does not exist in level");

        if (tile instanceof Updatable updatable)
            this.updatables.remove(updatable);
    }

    public void addHUDElement(HUDElement element, DrawLayer layer) {
        this.hudElements.computeIfAbsent(layer, k -> new ArrayList<>()).add(element);
    }

    public void removeHUDElement(HUDElement element, DrawLayer layer) {
        this.hudElements.get(layer).remove(element);
    }

    public List<HUDElement> getHUDElements(DrawLayer layer) {
        return this.hudElements.get(layer);
    }

    public void update() {
        if (isPaused())
            return;

        for (Updatable updatable : this.updatables) {
            updatable.update();
        }
    }

    @SuppressWarnings({"rawtypes", "unchecked"})
    public void render(Canvas canvas, Camera camera) {
        List<HUDElement> bottom = getHUDElements(DrawLayer.BOTTOM);
        if (bottom != null) {
            for (HUDElement element : bottom) {
                element.draw(canvas);
            }
        }

        for (Tile tile : this.tiles) {
            if(camera.isOnScreen(tile)) {
                TileRenderer renderer = getTileRenderer(tile);
                renderer.render(tile, canvas, camera);
            }
        }

        List<HUDElement> beforeEntity = getHUDElements(DrawLayer.BEFORE_ENTITY);
        if (beforeEntity != null) {
            for (HUDElement element : beforeEntity) {
                element.draw(canvas);
            }
        }

        for (Entity entity : this.entities) {
            if(camera.isOnScreen(entity)) {
                EntityRenderer renderer = getEntityRenderer(entity);
                renderer.render(entity, canvas, camera);
            }
        }

        List<HUDElement> afterEntity = getHUDElements(DrawLayer.AFTER_ENTITY);
        if (afterEntity != null) {
            for (HUDElement element : afterEntity) {
                element.draw(canvas);
            }
        }

        Input input = Input.INSTANCE;
        for (Gui gui : this.player.getGuiStack()) {
            gui.render(canvas, input.getMouseX(), input.getMouseY());
        }

        List<HUDElement> top = getHUDElements(DrawLayer.TOP);
        if (top != null) {
            for (HUDElement element : top) {
                element.draw(canvas);
            }
        }
    }

    @SuppressWarnings("rawtypes")
    private EntityRenderer getEntityRenderer(Entity entity) {
        return this.entityRenderers.get(entity.getType());
    }

    @SuppressWarnings("rawtypes")
    private TileRenderer getTileRenderer(Tile tile) {
        return this.tileRenderers.get(tile.getType());
    }
}
