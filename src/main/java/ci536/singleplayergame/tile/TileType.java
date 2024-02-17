package ci536.singleplayergame.tile;

public class TileType<T extends Tile> {
    public static final TileType<Tile> DIRT = new TileType<>(Tile::new, "dirt", true);

    private final TileFactory<T> factory;
    private final String name;
    private final boolean solid;

    public TileType(TileFactory<T> factory, String name, boolean solid) {
        this.factory = factory;
        this.name = name;
        this.solid = solid;
    }

    public String getName() {
        return name;
    }

    public boolean isSolid() {
        return solid;
    }

    @Override
    public String toString() {
        return "TileType{" + "name='" + this.name + '\'' + "solid=" + solid + '}';
    }

    public T create(int x, int y) {
        return this.factory.create(this, x, y);
    }

    @FunctionalInterface
    public interface TileFactory<T extends Tile> {
        T create(TileType<T> type, int x, int y);
    }
}
