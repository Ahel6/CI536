package ci536.singleplayergame;

import ci536.singleplayergame.math.Vector2f;
import ci536.singleplayergame.ui.Gui;
import ci536.singleplayergame.ui.InputListener;
import javafx.animation.AnimationTimer;
import javafx.event.EventHandler;
import javafx.scene.Scene;
import javafx.scene.input.KeyEvent;
import javafx.scene.input.MouseEvent;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.CopyOnWriteArrayList;

public class Input {
    public static final Input INSTANCE = new Input();

    private final Map<String, Boolean> currentlyActiveKeys = new HashMap<>();

    private final List<EventHandler<MouseEvent>> mouseMovedHandlers = new CopyOnWriteArrayList<>();
    private final List<EventHandler<MouseEvent>> mouseClickedHandlers = new CopyOnWriteArrayList<>();
    private final List<EventHandler<MouseEvent>> mouseDraggedHandlers = new CopyOnWriteArrayList<>();
    private final List<EventHandler<MouseEvent>> mouseReleasedHandlers = new CopyOnWriteArrayList<>();
    private final List<EventHandler<KeyEvent>> keyPressedHandlers = new CopyOnWriteArrayList<>();
    private final List<EventHandler<KeyEvent>> keyReleasedHandlers = new CopyOnWriteArrayList<>();

    private int mouseX, mouseY;

    public void init(Scene scene) {
        scene.setOnKeyPressed(event -> {
            String codeString = event.getCode().toString();
            if (!currentlyActiveKeys.containsKey(codeString)) {
                currentlyActiveKeys.put(codeString, true);
            }

            for (EventHandler<KeyEvent> handler : keyPressedHandlers) {
                handler.handle(event);
            }
        });

        scene.setOnKeyReleased(event -> {
            currentlyActiveKeys.remove(event.getCode().toString());
            for (EventHandler<KeyEvent> handler : keyReleasedHandlers) {
                handler.handle(event);
            }
        });

        scene.setOnMouseMoved(event -> {
            this.mouseX = (int) event.getX();
            this.mouseY = (int) event.getY();

            for (EventHandler<MouseEvent> handler : mouseMovedHandlers) {
                handler.handle(event);
            }
        });

        scene.setOnMouseClicked(event -> {
            for (EventHandler<MouseEvent> handler : mouseClickedHandlers) {
                handler.handle(event);
            }
        });

        scene.setOnMouseDragged(event -> {
            mouseX = (int) event.getX();
            mouseY = (int) event.getY();

            for (EventHandler<MouseEvent> handler : mouseDraggedHandlers) {
                handler.handle(event);
            }
        });

        scene.setOnMouseReleased(event -> {
            for (EventHandler<MouseEvent> handler : mouseReleasedHandlers) {
                handler.handle(event);
            }
        });

        new AnimationTimer() {
            @Override
            public void handle(long now) {
                try {
                    Thread.sleep(1000 / 60);
                } catch (InterruptedException e) {
                    throw new RuntimeException(e);
                }

                removeActiveKey("W");
                removeActiveKey("A");
                removeActiveKey("S");
                removeActiveKey("D");

                removeActiveKey("UP");
                removeActiveKey("LEFT");
                removeActiveKey("DOWN");
                removeActiveKey("RIGHT");
            }
        }.start();
    }

    private boolean removeActiveKey(String codeString) {
        Boolean isActive = currentlyActiveKeys.get(codeString);

        if (isActive != null && isActive) {
            currentlyActiveKeys.put(codeString, false);
            return true;
        } else {
            return false;
        }
    }

    public boolean isKeyPressed(String codeString) {
        return currentlyActiveKeys.getOrDefault(codeString, false);
    }

    public boolean isAnyKeyPressed() {
        return currentlyActiveKeys.values().stream().anyMatch(Boolean::booleanValue);
    }

    public boolean isAnyKeyPressed(String... codeStrings) {
        for (String codeString : codeStrings) {
            if (isKeyPressed(codeString)) {
                return true;
            }
        }
        return false;
    }

    public float getHorizontalInput() {
        float horizontalInput = 0;
        if (isKeyPressed("A") || isKeyPressed("LEFT")) {
            horizontalInput -= 1;
        }

        if (isKeyPressed("D") || isKeyPressed("RIGHT")) {
            horizontalInput += 1;
        }

        return horizontalInput;
    }

    public float getVerticalInput() {
        float verticalInput = 0;
        if (isKeyPressed("W") || isKeyPressed("UP")) {
            verticalInput -= 1;
        }

        if (isKeyPressed("S") || isKeyPressed("DOWN")) {
            verticalInput += 1;
        }

        return verticalInput;
    }

    public Vector2f getMovementInput() {
        return new Vector2f(getHorizontalInput(), getVerticalInput());
    }

    public int getMouseX() {
        return this.mouseX;
    }

    public int getMouseY() {
        return this.mouseY;
    }

    public void addMouseMovedHandler(EventHandler<MouseEvent> handler) {
        this.mouseMovedHandlers.add(handler);
    }

    public void addMouseClickedHandler(EventHandler<MouseEvent> handler) {
        this.mouseClickedHandlers.add(handler);
    }

    public void addMouseDraggedHandler(EventHandler<MouseEvent> handler) {
        this.mouseDraggedHandlers.add(handler);
    }

    public void addMouseReleasedHandler(EventHandler<MouseEvent> handler) {
        this.mouseReleasedHandlers.add(handler);
    }

    public void addKeyPressedHandler(EventHandler<KeyEvent> handler) {
        this.keyPressedHandlers.add(handler);
    }

    public void addKeyReleasedHandler(EventHandler<KeyEvent> handler) {
        this.keyReleasedHandlers.add(handler);
    }

    public void removeMouseMovedHandler(EventHandler<MouseEvent> handler) {
        this.mouseMovedHandlers.remove(handler);
    }

    public void removeMouseClickedHandler(EventHandler<MouseEvent> handler) {
        this.mouseClickedHandlers.remove(handler);
    }

    public void removeMouseDraggedHandler(EventHandler<MouseEvent> handler) {
        this.mouseDraggedHandlers.remove(handler);
    }

    public void removeMouseReleasedHandler(EventHandler<MouseEvent> handler) {
        this.mouseReleasedHandlers.remove(handler);
    }

    public void removeKeyPressedHandler(EventHandler<KeyEvent> handler) {
        this.keyPressedHandlers.remove(handler);
    }

    public void removeKeyReleasedHandler(EventHandler<KeyEvent> handler) {
        this.keyReleasedHandlers.remove(handler);
    }

    public InputListener addListener(Gui gui) {
        var listener = new InputListener(
                event -> gui.onMouseClick((int) event.getSceneX(), (int) event.getSceneY()),
                event -> gui.onMouseDrag((int) event.getSceneX(), (int) event.getSceneY()),
                event -> gui.onMouseRelease((int) event.getSceneX(), (int) event.getSceneY()),
                event -> gui.onKeyPress(event.getCode()),
                event -> gui.onKeyRelease(event.getCode()));

        addMouseClickedHandler(listener.mouseClickedHandler());
        addMouseDraggedHandler(listener.mouseDraggedHandler());
        addMouseReleasedHandler(listener.mouseReleasedHandler());
        addKeyPressedHandler(listener.keyPressedHandler());
        addKeyReleasedHandler(listener.keyReleasedHandler());

        return listener;
    }

    public void removeListener(InputListener listener) {
        removeMouseClickedHandler(listener.mouseClickedHandler());
        removeMouseDraggedHandler(listener.mouseDraggedHandler());
        removeMouseReleasedHandler(listener.mouseReleasedHandler());
        removeKeyPressedHandler(listener.keyPressedHandler());
        removeKeyReleasedHandler(listener.keyReleasedHandler());
    }
}
