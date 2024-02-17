package ci536.singleplayergame.tile;

import ci536.singleplayergame.game.GameObject;
import ci536.singleplayergame.math.AABB;

public class Tile extends GameObject {
    private final TileType<?> type;

    public Tile(TileType<?> type, int x, int y) {
        super(new AABB(x, y, 10, 10));
        this.type = type;
    }

    public Tile(TileType<?> type) {
        this(type, 0, 0);
    }

    public TileType<?> getType() {
        return type;
    }
}
