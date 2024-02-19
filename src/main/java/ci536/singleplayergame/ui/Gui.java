package ci536.singleplayergame.ui;

import ci536.singleplayergame.Input;
import javafx.beans.property.IntegerProperty;
import javafx.beans.property.SimpleIntegerProperty;
import javafx.beans.value.ObservableValue;
import javafx.scene.canvas.Canvas;
import javafx.scene.input.KeyCode;

import java.util.ArrayList;
import java.util.List;

public abstract class Gui {
    protected final IntegerProperty screenWidth = new SimpleIntegerProperty(800);
    protected final IntegerProperty screenHeight = new SimpleIntegerProperty(600);

    private final List<GuiWidget> widgets = new ArrayList<>();
    private final InputListener inputListener;

    protected int leftPos, topPos;

    public Gui() {
        this.inputListener = Input.INSTANCE.addListener(this);
    }

    public void init(ObservableValue<Integer> screenWidth, ObservableValue<Integer> screenHeight) {
        this.screenWidth.bind(screenWidth);
        this.screenHeight.bind(screenHeight);

        this.leftPos = (this.screenWidth.get() - getWidth()) / 2;
        this.topPos = (this.screenHeight.get() - getHeight()) / 2;
    }

    public void destroy() {
        Input.INSTANCE.removeListener(this.inputListener);
    }

    public final void addWidget(GuiWidget widget) {
        this.widgets.add(widget);
    }

    public boolean shouldPauseGame() {
        return false;
    }

    public void render(Canvas canvas, int mouseX, int mouseY) {
        for (GuiWidget widget : this.widgets) {
            widget.render(canvas, mouseX, mouseY);
        }

        for (GuiWidget widget : this.widgets) {
            widget.renderTooltip(canvas, mouseX, mouseY);
        }
    }

    public void onMouseClick(int mouseX, int mouseY) {
        for (GuiWidget widget : this.widgets) {
            widget.onMouseClick(mouseX, mouseY);
        }
    }

    public void onMouseDrag(int mouseX, int mouseY) {
        for (GuiWidget widget : this.widgets) {
            widget.onMouseDrag(mouseX, mouseY);
        }
    }

    public void onMouseRelease(int mouseX, int mouseY) {
        for (GuiWidget widget : this.widgets) {
            widget.onMouseRelease(mouseX, mouseY);
        }
    }

    public void onKeyPress(KeyCode keyCode) {
        for (GuiWidget widget : this.widgets) {
            widget.onKeyPress(keyCode);
        }
    }

    public void onKeyRelease(KeyCode keyCode) {
        for (GuiWidget widget : this.widgets) {
            widget.onKeyRelease(keyCode);
        }
    }

    public abstract int getWidth();

    public abstract int getHeight();
}
