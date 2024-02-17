package ci536.singleplayergame.game;

import ci536.singleplayergame.math.AABB;

public class GameObject {
    protected final AABB boundingBox;

    public GameObject() {
        this.boundingBox = new AABB(0, 0, 1, 1);
    }

    public GameObject(AABB boundingBox) {
        this.boundingBox = boundingBox;
    }

    public AABB getBoundingBox() {
        return boundingBox;
    }
}
