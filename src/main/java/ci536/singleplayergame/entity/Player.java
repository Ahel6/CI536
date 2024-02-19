package ci536.singleplayergame.entity;

import ci536.singleplayergame.ui.Gui;
import ci536.singleplayergame.ui.PauseGui;
import javafx.beans.value.ObservableValue;

import java.util.Stack;

public class Player extends Entity {
    private final Stack<Gui> guiStack = new Stack<>();
    private final ObservableValue<Integer> screenWidth;
    private final ObservableValue<Integer> screenHeight;

    public Player(ObservableValue<Integer> screenWidth, ObservableValue<Integer> screenHeight) {
        super(EntityType.PLAYER);

        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;

        setAttributeValue(EntityAttribute.HEALTH, 10.0f);
        setAttributeValue(EntityAttribute.SPEED, 5.0f);
    }

    public Player(ObservableValue<Integer> screenWidth, ObservableValue<Integer> screenHeight, float x, float y) {
        this(screenWidth, screenHeight);
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

    public void pushGui(Gui gui) {
        this.guiStack.push(gui);
        gui.init(this.screenWidth, this.screenHeight);
    }

    public void popGui() {
        Gui gui = this.guiStack.pop();
        gui.destroy();
    }

    public void pauseGame() {
        pushGui(new PauseGui(this));
    }
}
