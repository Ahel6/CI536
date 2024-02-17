package ci536.singleplayergame.ui;

import javafx.scene.canvas.Canvas;
import javafx.scene.input.KeyCode;

public abstract class GuiWidget {
    private final Gui parent;
    private int x, y, width, height;

    public GuiWidget(Gui parent, int x, int y, int width, int height) {
        this.parent = parent;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public abstract void render(Canvas canvas, int mouseX, int mouseY);

    public abstract void renderTooltip(Canvas canvas, int mouseX, int mouseY);

    public boolean isMouseOver(int mouseX, int mouseY) {
        return mouseX >= x && mouseX < x + width && mouseY >= y && mouseY < y + height;
    }

    public void onMouseClick(int mouseX, int mouseY) {
        // Handle the click
    }

    public void onMouseDrag(int mouseX, int mouseY) {
        // Handle the drag
    }

    public void onMouseRelease(int mouseX, int mouseY) {
        // Handle the release
    }

    public void onKeyPress(KeyCode keyCode) {
        // Handle the key press
    }

    public void onKeyRelease(KeyCode keyCode) {
        // Handle the key release
    }

    public Gui getParent() {
        return parent;
    }

    public int getX() {
        return x;
    }

    public void setX(int x) {
        this.x = x;
    }

    public int getY() {
        return y;
    }

    public void setY(int y) {
        this.y = y;
    }

    public int getWidth() {
        return width;
    }

    public void setWidth(int width) {
        this.width = width;
    }

    public int getHeight() {
        return height;
    }

    public void setHeight(int height) {
        this.height = height;
    }

    public void setBounds(int x, int y, int width, int height) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public void copyBounds(GuiWidget widget) {
        setBounds(widget.getX(), widget.getY(), widget.getWidth(), widget.getHeight());
    }
}
