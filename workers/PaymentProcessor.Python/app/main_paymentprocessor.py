from config import load_settings
from kafka_client import create_consumer
from services import process_payment
from kafka_client import create_producer
from datetime import datetime, timezone
import os
import time


def run_worker():
    print("Main worker starting...")

    while True:
        try:
            print("Loading Kafka settings from Azure Key Vault...")
            settings = load_settings()

            consumer = create_consumer(settings)
            producer = create_producer(settings)

            retry_topic = settings["topic_retry1"]
            dead_letter_topic = settings["topic_deadletter"]
            invalid_payments_topic = settings["topic_invalidPayments"]

            print("Consumer connected. Listening for messages...")

            for msg in consumer:
                key = msg.key.decode("utf-8") if msg.key else None

                print(
                    f"PID={os.getpid()} | "
                    f"Partition={msg.partition} | "
                    f"Key={key}"
                )

                event = normalize_event(msg.value)

                process_payment(
                    event,
                    producer,
                    retry_topic,
                    dead_letter_topic,
                    invalid_payments_topic,
                    key
                )

        except Exception as e:
            print("Main worker error, restarting loop in 5s:", e)
            time.sleep(5)


def normalize_event(event):
    meta = event.get("Meta")

    if not isinstance(meta, dict):
        meta = {}
        event["Meta"] = meta

    meta.setdefault("RetryCount", 0)
    meta.setdefault("LastFailureReason", None)
    meta.setdefault("CreatedAt", datetime.now(timezone.utc).isoformat())

    return event
