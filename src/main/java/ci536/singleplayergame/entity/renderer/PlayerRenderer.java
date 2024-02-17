package ci536.singleplayergame.entity.renderer;

import ci536.singleplayergame.entity.Player;

public class PlayerRenderer extends TexturedEntityRenderer<Player> {
    @Override
    public String getTexturePath(Player entity) {
        return "textures/player.png";
    }
}
