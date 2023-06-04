FROM openjdk:8-jdk-alpine
WORKDIR /app
COPY /server/src /app/src
COPY /common /app/src
RUN mkdir bin
RUN javac -d bin src/*.java
CMD ["java", "-cp", "bin", "Server"]
