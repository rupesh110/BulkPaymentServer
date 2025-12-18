from config import load_settings
from kafka_client import create_consumer
from services import process_payment
from kafka_client import create_producer
from datetime import datetime, timezone

def run_worker():
    print("Loading Kafka settings from Azure Key Vault...")

    settings = load_settings()

    consumer = create_consumer(settings)
    producer = create_producer(settings)

    retry_topic = settings["topic_retry1"]
    dead_letter_topic = settings["topic_deadletter"]
    invalid_payments_topic = settings["topic_invalidPayments"]

    print("Consumer connected. Listening for messages...")

    for msg in consumer:
        event = normalize_event(msg.value)
        process_payment(event, producer, retry_topic, dead_letter_topic, invalid_payments_topic)
        consumer.commit()  


def normalize_event(event):
    meta = event.get("Meta")

    if not isinstance(meta, dict):
        meta = {}
        event["Meta"] = meta

    # retryCount (existing logic)
    meta.setdefault("RetryCount", 0)
    meta.setdefault("LastFailureReason", None)

    # createdAt (NEW, safe default)
    meta.setdefault(
        "CreatedAt",
        datetime.now(timezone.utc).isoformat()
    )

    return event


if __name__ == "__main__":
    run_worker()
