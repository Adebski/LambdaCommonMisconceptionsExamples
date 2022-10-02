package com.adebski;

import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.lambda.runtime.RequestHandler;
import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.apache.commons.math3.stat.descriptive.DescriptiveStatistics;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.util.Collections;
import java.util.Map;
import java.util.Random;

public class LambdaJavaCommonMisconceptionsMultiplePathsProvisionedConcurrency implements RequestHandler<Map<String, String>, String> {
    private static final int SAMPLE_ITERATIONS = 2000;
    private final Logger logger;
    private final ObjectMapper objectMapper =
        new ObjectMapper().configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
    private final long constructTimeStart;
    private final long constructTimeEnd;
    private int invocations = 0;

    public LambdaJavaCommonMisconceptionsMultiplePathsProvisionedConcurrency() throws InterruptedException {
        this.constructTimeStart = System.currentTimeMillis();
        this.logger = LogManager.getLogger(LambdaJavaCommonMisconceptionsMultiplePathsProvisionedConcurrency.class);

        // To verify that the init/constructor can run for more than 10 seconds
        // we will just sleep here before we proceed to execute sample requests
        logger.info("Sleep start");
        Thread.sleep(10000);
        logger.info("Sleep end");

        // Pre-warming the most common code paths in the function handler constructor to take advantage
        // of the provisioned concurrency function lifecycle
        for(int i = 0; i < SAMPLE_ITERATIONS; ++i) {
            Map<String, String> exampleInput = Collections.singletonMap("testNumber", String.valueOf(i));
            logger.info("Sample result: {}", calculateResult(exampleInput, objectMapper));
        }
        this.constructTimeEnd = System.currentTimeMillis();

        System.out.println("Env: " + System.getenv());
        logger.info("Constructor took {} ms", constructTimeEnd - constructTimeStart);
    }

    @Override
    public String handleRequest(Map<String, String> input, Context context) {
        ++invocations;
        long start = System.currentTimeMillis();

        String result = calculateResult(input, objectMapper);

        logger.info(
            "handleRequest, invocation type {}, {} ms after the constructor, {} invocation",
            System.getenv("AWS_LAMBDA_INITIALIZATION_TYPE"),
            start - constructTimeEnd,
            invocations);

        return result;
    }

    private static String calculateResult(Map<String, String> input, ObjectMapper objectMapper) {
        int numberReceived = Integer.parseInt(input.get("testNumber"));
        if (numberReceived % 2 == 0) {
            int numbersToGenerate = 1000;
            DescriptiveStatistics descriptiveStatistics =
                new DescriptiveStatistics(new Random().doubles(numbersToGenerate).toArray());
            return String.format(
                "std-dev %f geo-mean %f skewness %f",
                descriptiveStatistics.getStandardDeviation(),
                descriptiveStatistics.getGeometricMean(),
                descriptiveStatistics.getSkewness());
        } else {
            ParsedInput parsedInput = objectMapper.convertValue(input, ParsedInput.class);
            return parsedInput.toString();
        }
    }

    public static final class ParsedInput {
        private final int testNumber;

        @JsonCreator
        public ParsedInput(@JsonProperty("testNumber") int testNumber) {
            this.testNumber = testNumber;
        }

        @Override
        public String toString() {
            return "ParsedInput{" +
                "testNumber=" + testNumber +
                '}';
        }
    }
}
