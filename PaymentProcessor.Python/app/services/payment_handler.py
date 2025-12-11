import json
import random
import time


def process_payment(event, producer, retry_topic): ## TODO: integrate with real payment gateway
    payload = event.get("Payload")

    # Deserialize if needed
    if isinstance(payload, str):
        try:
            payload = json.loads(payload)
        except Exception as e:
            print("Failed to deserialize payload:", payload, e)
            return {"status": "ERROR", "reason": "Invalid JSON"}

    print("\n Received Event:")
    print("  UploadId:", event.get("UploadId"))
    print("  EventType:", event.get("EventType"))
    print("  Payload:", payload)

    delay = random.uniform(0.5, 2.5) 
    time.sleep(delay)


    # FAKE PAYMENT APPROVAL / REJECTION LOGIC
    amount = payload.get("Amount", 0)


    # If amount > 15000 → auto reject
    if amount > 15000:
        print(f"Payment REJECTED — Amount too high: {amount}")
        producer.send(retry_topic, event)

        return {
            "status": "REJECTED",
            "reason": "Amount exceeds limit",
            "payload": payload
        }

    # Random failure 
    if random.random() < 0.15:  
        print("Payment FAILED — Random simulated failure", retry_topic)

        producer.send(retry_topic, event)
        return {
            "status": "FAILED",
            "reason": "Random simulated error",
            "payload": payload
        }

    # Otherwise APPROVE
    print("Payment APPROVED")
    return {
        "status": "APPROVED",
        "payload": payload
    }
