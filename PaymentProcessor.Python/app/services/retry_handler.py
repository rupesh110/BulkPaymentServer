import json
import random
import time
from datetime import datetime, timezone

from .helper import _retry_or_dlq

MAX_RETRIES = 2

def retry_payment(event, producer, retry_topic, dead_letter): ## TODO: integrate with real payment gateway
    payload = event.get("Payload")

    print("\nProcessing payment...", payload)

  
    meta = event.get("Meta")
    if not isinstance(meta, dict):
        meta = {}
        event["Meta"] = meta

    meta.setdefault("RetryCount", 0)
    meta.setdefault("LastFailureReason", None)
    meta.setdefault("CreatedAt", datetime.now(timezone.utc).isoformat())

    
    if isinstance(payload, str):
        payload = json.loads(payload)
 

    key = str(payload["InvoiceNumber"]).encode()

  
    #delay = random.uniform(0.5, 2.5) 
    #time.sleep(delay)


    # FAKE REJECTION LOGIC
    amount = payload.get("Amount", 0)

    # If amount > 15000 → auto reject
    if amount > 15000:
        return _retry_or_dlq(
            producer,
            event,
            retry_topic,
            dead_letter,
            meta,
            key,
            reason="Amount exceeds limit"
        )

    # Random failure 
    if random.random() < 0.15:  
        return _retry_or_dlq(
            producer,
            event,
            retry_topic,
            dead_letter,
            meta,
            key,
            reason="Random simulated failure"
        )

    # Otherwise APPROVE
    print("Payment APPROVED")
    return {
        "status": "APPROVED",
        "payload": payload
    }
