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

import java.util.Map;
import java.util.Random;

public class LambdaJavaCommonMisconceptionsMultiplePaths implements RequestHandler<Map<String, String>, String> {

    private final Logger logger;
    private final ObjectMapper objectMapper =
        new ObjectMapper().configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
    private final long constructTimeStart;
    private final long constructTimeEnd;
    private int invocations = 0;

    public LambdaJavaCommonMisconceptionsMultiplePaths() {
        this.constructTimeStart = System.currentTimeMillis();
        this.logger = LogManager.getLogger(LambdaJavaCommonMisconceptionsMultiplePaths.class);
        this.constructTimeEnd = System.currentTimeMillis();

        logger.info("Constructor took {} ms", constructTimeEnd - constructTimeStart);
    }

    @Override
    public String handleRequest(Map<String, String> input, Context context) {
        ++invocations;
        long start = System.currentTimeMillis();

        String result = calculateResult(input);

        logger.info("handleRequest {} ms after the constructor, {} invocation", start - constructTimeEnd, invocations);

        return result;
    }

    private String calculateResult(Map<String, String> input) {
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
