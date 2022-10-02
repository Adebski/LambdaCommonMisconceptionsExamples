package com.adebski;

import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

import java.util.UUID;

/**
 * Entry point for external extension
 */
public class ExtensionMain {
    public static void main(String[] args) {
        String extensionInstanceIdentifier = UUID.randomUUID().toString();
        // Register the extension for "INVOKE" and "SHUTDOWN" events
        final String extension = ExtensionClient.registerExtension();
        System.out.println(
            String.format(
                "%s: Extension registration complete, extensionID: %s",
                extensionInstanceIdentifier,
                extension)
        );
        int eventsReceived = 0;
        while (true) {
            try {
                String response = ExtensionClient.getNext(extension);
                if (response != null && !response.isEmpty()) {
                    ++eventsReceived;
                    System.out.printf("%s: Extension %s received event %d%n", extensionInstanceIdentifier, extension, eventsReceived);
                    JsonObject eventJsonObject = new Gson().fromJson(response, JsonObject.class);
                    JsonElement eventTypeElement = eventJsonObject.get("eventType");

                    // Depending upon event type perform corresponding actions
                    if (eventTypeElement != null) {
                        final String eventType = eventTypeElement.getAsString();
                        switch (eventTypeElement.getAsString()) {
                            case "INVOKE":
                                handleInvoke(response, extensionInstanceIdentifier, extension);
                                break;
                            case "SHUTDOWN":
                                handleShutDown(extensionInstanceIdentifier, extension);
                                break;
                            default:
                                System.err.println("Invalid event type received " + eventType);
                        }
                    }
                }
            } catch (Exception e) {
                System.out.println("Error while processing extension -" + e.getMessage());
                e.printStackTrace();
            }
        }
    }

    /**
     * Shutdown extension if we receive a shutdown event from lambda container
     */
    private static void handleShutDown(String uuid, String extensionId) {
        System.out.printf("%s: Shutting down the extension %s%n", uuid, extensionId);
        System.exit(0);
    }

    /**
     * Process payload
     *
     * @param payload event payload
     */
    public static void handleInvoke(String payload, String uuid, String extensionId) throws InterruptedException {
        System.out.printf(
            "%s: Handling invoke from extension %s, payload %s%n", uuid, extensionId, payload);
/*        Thread.sleep(3000);
        System.out.printf("%s: Sleep has ended %s%n", uuid, extensionId);*/
    }
}
