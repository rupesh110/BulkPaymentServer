import json
import random
import time
from datetime import datetime, timezone
import logging

from services.helper import retry_or_dlq

logger = logging.getLogger(__name__)

def process_payment(event, producer, retry_topic, dead_letter, invalid_payment, key):
    payload = event.get("Payload")

    meta = event.get("Meta")
    if not isinstance(meta, dict):
        meta = {}
        event["Meta"] = meta

    meta.setdefault("RetryCount", 0)
    meta.setdefault("LastFailureReason", None)
    meta.setdefault("CreatedAt", datetime.now(timezone.utc).isoformat())

    if isinstance(payload, str):
        try:
            payload = json.loads(payload)
        except Exception:
            logger.warning(
                "Failed to deserialize payload",
                extra={
                    "key": key,
                    "retryCount": meta["RetryCount"],
                    "payloadPreview": payload[:200] if payload else None,
                },
            )

            meta["RetryCount"] += 1
            meta["LastFailureReason"] = "Invalid JSON"

            producer.send(
                invalid_payment,
                value=event,
                key=key.encode("utf-8"),
            )

            return {"status": "ERROR", "reason": "Invalid JSON"}

    logger.info(
        "Processing payment",
        extra={
            "key": key,
            "retryCount": meta["RetryCount"],
            "amount": payload.get("Amount"),
        },
    )

    time.sleep(random.uniform(0.5, 2.5))

    amount = payload.get("Amount", 0)

    if amount > 15000:
        logger.warning(
            "Payment rejected — amount exceeds limit",
            extra={
                "key": key,
                "amount": amount,
                "retryCount": meta["RetryCount"],
            },
        )

        return retry_or_dlq(
            producer,
            event,
            retry_topic,
            dead_letter,
            meta,
            key=key,
            reason="Amount exceeds limit",
        )

    if random.random() < 0.15:
        
        logger.error(
            "Payment failed — key=%s retry=%d reason=%s",
            key,
            meta["RetryCount"],
            "Random simulated error",
            extra={
                "key": key,
                "retryCount": meta["RetryCount"],
                "reason": "Random simulated error",
            },
        )


        return retry_or_dlq(
            producer,
            event,
            retry_topic,
            dead_letter,
            meta,
            key=key,
            reason="Random simulated error",
        )

    logger.info(
        "Payment approved",
        extra={
            "key": key,
            "amount": amount,
        },
    )

    return {
        "status": "APPROVED",
        "payload": payload,
    }

