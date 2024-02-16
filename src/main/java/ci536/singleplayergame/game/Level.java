package ci536.singleplayergame.game;

import ci536.singleplayergame.Input;
import ci536.singleplayergame.entity.*;
import ci536.singleplayergame.ui.Gui;
import javafx.scene.canvas.Canvas;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class Level {
    private final List<Entity> entities = new ArrayList<>();
    private final Map<EntityType<?>, EntityRenderer<?>> entityRenderers = new HashMap<>();
    private final Player player;

    public Level(Player player) {
        this.entityRenderers.put(EntityType.PLAYER, new PlayerRenderer());

        this.player = player;
        addEntity(player);
    }

    public boolean isPaused() {
        return player.isPaused();
    }

    public void addEntity(Entity entity) {
        if(entity instanceof Player && this.player.getLevel() != null)
            throw new IllegalArgumentException("Player entity already exists in level");

        if(!this.entityRenderers.containsKey(entity.getType()))
            throw new IllegalArgumentException("No renderer for entity type: " + entity.getType());

        if(entity.isDead())
            throw new IllegalArgumentException("Attempted to add dead entity to level");

        if(entity.getLevel() != null && entity.getLevel() != this)
            entity.getLevel().removeEntity(entity);

        this.entities.add(entity);
        entity.setLevel(this);
    }

    public void removeEntity(Entity entity) {
        if(!this.entities.remove(entity))
            throw new IllegalArgumentException("Attempted to remove entity that does not exist in level");

        entity.setLevel(null);
    }

    public void update() {
        if(isPaused())
            return;

        for (Entity entity : this.entities) {
            entity.update();
        }
    }

    @SuppressWarnings({"rawtypes", "unchecked"})
    public void render(Canvas canvas) {
        for (Entity entity : this.entities) {
            EntityRenderer renderer = getEntityRenderer(entity);
            renderer.render(entity, canvas);
        }

        Input input = Input.INSTANCE;
        for(Gui gui : this.player.getGuiStack()) {
            gui.render(canvas, input.getMouseX(), input.getMouseY());
        }
    }

    @SuppressWarnings("rawtypes")
    public EntityRenderer getEntityRenderer(Entity entity) {
        return this.entityRenderers.get(entity.getType());
    }
}
