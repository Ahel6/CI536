package ci536.singleplayergame.entity;

import ci536.singleplayergame.ui.Gui;

import java.util.Stack;

public class Player extends Entity {
    private final Stack<Gui> guiStack = new Stack<>();

    public Player() {
        super(EntityType.PLAYER);

        setAttributeValue(EntityAttribute.HEALTH, 10.0f);
        setAttributeValue(EntityAttribute.SPEED, 5.0f);
    }

    public Player(int x, int y) {
        this();
        setPosition(x, y);
    }

    @Override
    public void update() {

    }

    public void takeInput(float x, float y) {
        float speed = getAttributeValue(EntityAttribute.SPEED, Float.class);
        setPosition(getXPos() + (x * speed), getYPos() + (y * speed));
    }

    public boolean isPaused() {
        return !this.guiStack.isEmpty() && this.guiStack.stream().anyMatch(Gui::shouldPauseGame);
    }

    public Stack<Gui> getGuiStack() {
        return this.guiStack;
    }
}
