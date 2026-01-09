import logging
import os
import time
from datetime import datetime, timezone

from config import load_settings
from kafka_client import create_consumer, create_producer
from services import process_payment
from exceptions import BusinessHandled  # wherever you defined it

logger = logging.getLogger(__name__)


def run_worker():
    logger.info("Main worker starting")

    while True:
        try:
            settings = load_settings()
            logger.info("Kafka settings loaded", extra={"bootstrap": settings["bootstrap_servers"]})

            consumer = create_consumer(settings)
            producer = create_producer(settings)

            retry_topic = settings["topic_retry1"]
            dead_letter_topic = settings["topic_deadletter"]
            invalid_payments_topic = settings["topic_invalidPayments"]

            logger.info("Consumer connected. Listening for messages")

            for msg in consumer:
                key = msg.key.decode("utf-8") if msg.key else None

                logger.info(
                    "Message received",
                    extra={
                        "pid": os.getpid(),
                        "partition": msg.partition,
                        "key": key,
                    },
                )

                try:
                    event = normalize_event(msg.value)

                    process_payment(
                        event,
                        producer,
                        retry_topic,
                        dead_letter_topic,
                        invalid_payments_topic,
                        key,
                    )

                except BusinessHandled as e:
                    # Expected business outcome (retry / DLQ)
                    logger.warning("Business flow handled", extra={"reason": str(e)})
                    continue

        except Exception as e:
            # Infrastructure / unexpected error
            logger.exception("INFRA error, restarting worker in 5s")
            time.sleep(5)


def normalize_event(event):
    meta = event.get("Meta")

    if not isinstance(meta, dict):
        logger.debug("Meta missing or invalid, initializing")
        meta = {}
        event["Meta"] = meta

    meta.setdefault("RetryCount", 0)
    meta.setdefault("LastFailureReason", None)
    meta.setdefault("CreatedAt", datetime.now(timezone.utc).isoformat())

    return event
