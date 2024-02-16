package ci536.singleplayergame.util;

import java.util.ArrayList;
import java.util.List;

public class ShutdownHooks {
    private static final List<Runnable> SHUTDOWN_HOOKS = new ArrayList<>();

    static {
        Runtime.getRuntime().addShutdownHook(new Thread(ShutdownHooks::runShutdownHooks));
    }

    public static void addShutdownHook(Runnable runnable) {
        SHUTDOWN_HOOKS.add(runnable);
    }

    public static void runShutdownHooks() {
        SHUTDOWN_HOOKS.forEach(Runnable::run);
    }
}
