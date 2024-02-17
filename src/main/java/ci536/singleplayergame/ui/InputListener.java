package ci536.singleplayergame.ui;

import javafx.event.EventHandler;
import javafx.scene.input.KeyEvent;
import javafx.scene.input.MouseEvent;

public record InputListener(EventHandler<MouseEvent> mouseClickedHandler, EventHandler<MouseEvent> mouseDraggedHandler,
                            EventHandler<MouseEvent> mouseReleasedHandler, EventHandler<KeyEvent> keyPressedHandler,
                            EventHandler<KeyEvent> keyReleasedHandler) {

}
