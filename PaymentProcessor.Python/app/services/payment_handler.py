import json
import random
import time
from datetime import datetime, timezone

from .helper import _retry_or_dlq

MAX_RETRIES = 2


def process_payment(event, producer, retry_topic, dead_letter, invalid_payment):
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
        except Exception as e:
            print("Failed to deserialize payload:", payload, e)

            meta["RetryCount"] += 1
            meta["LastFailureReason"] = "Invalid JSON"

            key = str(payload.get("InvoiceNumber", "invalid")).encode()
            producer.send(invalid_payment, value=event, key=key)
            return {"status": "ERROR", "reason": "Invalid JSON"}

    key = str(payload["InvoiceNumber"]).encode()

    print("\nReceived Event:")
    print("  Payload:", payload)
    print("  RetryCount:", meta["RetryCount"])

    time.sleep(random.uniform(0.5, 2.5))

    amount = payload.get("Amount", 0)

    if amount > 15000:
        print(f"Payment REJECTED — Amount too high: {amount}")

        return _retry_or_dlq(
            producer,
            event,
            retry_topic,
            dead_letter,
            meta,
            key=key,
            reason="Amount exceeds limit"
        )

    if random.random() < 0.15:
        print("Payment FAILED — Random simulated failure")

        return _retry_or_dlq(
            producer,
            event,
            retry_topic,
            dead_letter,
            meta,
            key=key,
            reason="Random simulated error"
        )

    print("Payment APPROVED")
    return {
        "status": "APPROVED",
        "payload": payload
    }
