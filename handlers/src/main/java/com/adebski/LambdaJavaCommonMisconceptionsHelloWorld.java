package com.adebski;

import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.lambda.runtime.RequestHandler;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.util.Map;

public class LambdaJavaCommonMisconceptionsHelloWorld implements RequestHandler<Map<String, String>, String> {
    private static final int DUMMY_INT_STATIC;
    private static final String DUMMY_STRING_STATIC = getStringAndPrint("DUMMY_STRING_STATIC");

    private static String getStringAndPrint(String variableName) {
        System.out.println(String.format("Initializing %s during the init phase", variableName));
        return variableName + "foo";
    }

    static {
        System.out.println("Static initializer");
        DUMMY_INT_STATIC = 5;
    }

    private final String dummyStringNonStatic = getStringAndPrint("dummyStringNonStatic");
    private final Logger logger;
    private final long constructTimeStart;
    private final long constructTimeEnd;
    private int invocations = 0;

    public LambdaJavaCommonMisconceptionsHelloWorld() {
        this.constructTimeStart = System.currentTimeMillis();
        this.logger = LogManager.getLogger(LambdaJavaCommonMisconceptionsHelloWorld.class);
        this.constructTimeEnd = System.currentTimeMillis();

        System.out.println("Env: " + System.getenv());

        logger.info("Constructor took {} ms", constructTimeEnd - constructTimeStart);
    }

    @Override
    public String handleRequest(Map<String, String> input, Context context) {
        ++invocations;
        long start = System.currentTimeMillis();
        int numberReceived = Integer.parseInt(input.get("testNumber"));
        String result =
            numberReceived % 2 == 0
                ? String.format("%d is even", numberReceived)
                : String.format("%d is odd", numberReceived);

        logger.info(
            "handleRequest {} ms after the constructor, {} invocation",
            start - constructTimeEnd,
            invocations);

        return result;
    }

}
