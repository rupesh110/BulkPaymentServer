from config import load_settings
from kafka_client import create_retry_consumer
from kafka_client import create_producer
from services import retry_payment
import os
import time


def run_retry_worker():
    print("Retry worker starting...")

    while True:
        try:
            print("Loading Kafka settings from Azure Key Vault...")
            settings = load_settings()

            consumer = create_retry_consumer(settings)
            producer = create_producer(settings)

            retry_topic = settings["topic_retry1"]
            dead_letter_topic = settings["topic_deadletter"]

            print("Retry Consumer connected. Listening for messages...")
            print("Retry Worker is running...", retry_topic)

            for msg in consumer:
                key = msg.key.decode("utf-8") if msg.key else None

                print(
                    f"PID={os.getpid()} | "
                    f"Partition={msg.partition} | "
                    f"Key={key}"
                )

                event = msg.value
                retry_payment(event, producer, retry_topic, dead_letter_topic, key)

        except Exception as e:
            print("Retry worker error, restarting loop in 5s:", e)
            time.sleep(5)
